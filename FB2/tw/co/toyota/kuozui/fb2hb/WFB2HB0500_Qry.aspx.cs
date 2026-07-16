using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HB0500_Qry : BasePage
{
    //Service 物件
    private CFB2HB0500BO service = new CFB2HB0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            //技能專長類別
            getSKILL_TYPE();

            //外語等級/證照等級
            getSKILL_GRADE();
            if (Session["HB0500_Is_Search"] == "Y")
            {
                getQryField();
            }
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "session"
    private void getQryField()
    {
        txt_EMP_ID.Text = Session["HB0500_DATA_YEAR"].ToString();
        txt_EMP_NAME.Text = Session["HB0500_LEVEL_CD"].ToString();
        ddl_SKILL_TYPE.SelectedValue = Session["HB0500_EDUCATION_CD"].ToString();
        ddl_SKILL_GRADE.SelectedValue = Session["HB0500_WS_CD"].ToString();
        ViewState["PerPageRow"] = Session["HB0500_ddlPerPageRow"].ToString();
        WFB2HA0500Search_Click(null, null);
        Session["HB0500_Is_Search"] = "N";
    }

    private void setQryField()
    {
        Session["HB0500_EMP_ID"] = txt_EMP_ID.Text;
        Session["HB0500_EMP_NAME"] = txt_EMP_NAME.Text;
        Session["HB0500_ddl_SKILL_TYPE"] = ddl_SKILL_TYPE.SelectedValue;
        Session["HB0500_ddl_SKILL_GRADE"] = ddl_SKILL_GRADE.SelectedValue;
    }
    #endregion
    private void getSKILL_GRADE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSKILL_GRADE();
            ddl_SKILL_GRADE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SKILL_GRADE.Items.Add(new ListItem(dt.Rows[i]["SKILL_GRADE"].ToString(), dt.Rows[i]["SKILL_GRADE"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getSKILL_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SKILL_TYPE", "", "");
            ddl_SKILL_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SKILL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID,SKILL_TYPE,SKILL_DESC");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "SKILL_TYPE", "SKILL_DESC" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HB0500_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SKILL_TYPE", "SKILL_DESC" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

        }
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_SKILL_TYPE");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("SKILL_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SKILL_TYPE", "SKILL_DESC" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    protected void WFB2HA0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,SKILL_TYPE,SKILL_DESC", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,SKILL_TYPE,SKILL_DESC", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2HB0500Add.Visible = true;
                WFB2HB0500Edit.Visible = true;
                WFB2HB0500Delete.Visible = true;
                HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0500Upload_Click(object sender, EventArgs e)
    {

    }
    protected void WFB2HB0500Add_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2HB0500Search.Enabled = false;
            btn_clear.Disabled = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,SKILL_TYPE,SKILL_DESC", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,SKILL_TYPE,SKILL_DESC", 0, 10);

            WFB2HB0500Save.Visible = true;
            WFB2HB0500Cancel.Visible = true;

            WFB2HB0500Add.Visible = false;
            WFB2HB0500Edit.Visible = false;
            WFB2HB0500Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2HB0500Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<Tuple<string, string,string>> emp_id = new List<Tuple<string, string,string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string,string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["SKILL_TYPE"].ToString(), gv_result.DataKeys[i].Values["SKILL_DESC"].ToString()));

                }
            }
            string msg = service.delete_Skill(emp_id);
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
    protected void WFB2HB0500Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //disable查詢清除按鈕
            WFB2HB0500Search.Enabled = false;
            btn_clear.Disabled = false;

            WFB2HB0500Save.Visible = true;
            WFB2HB0500Cancel.Visible = true;

            WFB2HB0500Add.Visible = false;
            WFB2HB0500Edit.Visible = false;
            WFB2HB0500Delete.Visible = false;
            HID_Freeze.Value = "N";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                DropDownList ddl_NEW_SKILL_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_SKILL_TYPE");
                TextBox txt_NEW_SKILL_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SKILL_DESC");
                TextBox txt_NEW_SKILL_GRADE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SKILL_GRADE");
                TextBox txt_NEW_SKILL_ORG = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SKILL_ORG");
                TextBox txt_NEW_AWARD_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AWARD_DT");

                CFB2HB0500DAO fb2hb050 = new CFB2HB0500DAO();
                fb2hb050.EMP_ID = txt_NEW_EMP_ID.Text;
                fb2hb050.SKILL_TYPE = ddl_NEW_SKILL_TYPE.SelectedValue;
                fb2hb050.SKILL_DESC = txt_NEW_SKILL_DESC.Text;
                fb2hb050.SKILL_GRADE = txt_NEW_SKILL_GRADE.Text;
                fb2hb050.SKILL_ORG = txt_NEW_SKILL_ORG.Text;
                fb2hb050.AWARD_DT = txt_NEW_AWARD_DT.Text;

                fb2hb050.CREATED_BY = SessionHandle.Current.emp_id;
                fb2hb050.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2hb050.FUNC_ID = "FB2HB050";
                string msg = service.addSkill(fb2hb050);
                if (msg != "0")
                {
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
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID");
                    DropDownList ddl_NEW_SKILL_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_SKILL_TYPE");
                    TextBox txt_NEW_SKILL_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SKILL_DESC");
                    TextBox txt_NEW_SKILL_GRADE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SKILL_GRADE");
                    TextBox txt_NEW_SKILL_ORG = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SKILL_ORG");
                    TextBox txt_NEW_AWARD_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AWARD_DT");

                    CFB2HB0500DAO fb2hb050 = new CFB2HB0500DAO();
                    fb2hb050.EMP_ID = txt_NEW_EMP_ID.Text;
                    fb2hb050.SKILL_TYPE = ddl_NEW_SKILL_TYPE.SelectedValue;
                    fb2hb050.SKILL_DESC = txt_NEW_SKILL_DESC.Text;
                    fb2hb050.SKILL_GRADE = txt_NEW_SKILL_GRADE.Text;
                    fb2hb050.SKILL_ORG = txt_NEW_SKILL_ORG.Text;
                    fb2hb050.AWARD_DT = txt_NEW_AWARD_DT.Text;

                    fb2hb050.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2hb050.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2hb050.FUNC_ID = "FB2HB050";


                    string msg = service.addSkill(fb2hb050);
                    if (msg != "0")
                    {
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
                    //更新
                    string EMP_ID = gv_result.DataKeys[gv_result.EditIndex].Values[0].ToString();
                    string SKILL_TYPE =gv_result.DataKeys[gv_result.EditIndex].Values[1].ToString();
                    string SKILL_DESC = gv_result.DataKeys[gv_result.EditIndex].Values[2].ToString();
                    TextBox txt_EDIT_SKILL_GRADE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_SKILL_GRADE");
                    TextBox txt_EDIT_SKILL_ORG = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_SKILL_ORG");
                    TextBox txt_EDIT_AWARD_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_AWARD_DT");

                    CFB2HB0500DAO fb2hb050 = new CFB2HB0500DAO();
                    fb2hb050.EMP_ID = EMP_ID;
                    fb2hb050.SKILL_TYPE = SKILL_TYPE;
                    fb2hb050.SKILL_DESC = SKILL_DESC;
                    fb2hb050.SKILL_GRADE = txt_EDIT_SKILL_GRADE.Text;
                    fb2hb050.SKILL_ORG = txt_EDIT_SKILL_ORG.Text;
                    fb2hb050.AWARD_DT = txt_EDIT_AWARD_DT.Text;

                    fb2hb050.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2hb050.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2hb050.FUNC_ID = "FB2HB050";


                    string msg = service.updateSkill(fb2hb050);
                    if (msg != "0")
                    {
                        showMessage("modFailMessage", msg);
                        return;
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
            gv_result.DataKeyNames = new string[] { "EMP_ID", "SKILL_TYPE", "SKILL_DESC" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HB0500Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2HB0500Save.Visible = false;
            WFB2HB0500Cancel.Visible = false;
            WFB2HB0500Add.Visible = true;
            WFB2HB0500Edit.Visible = true;
            WFB2HB0500Delete.Visible = true;
            HID_Freeze.Value = "Y";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0500Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HB0500Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HB0500Edit.Visible = true;
            WFB2HB0500Delete.Visible = true;
        }

        WFB2HB0500Save.Visible = false;
        WFB2HB0500Cancel.Visible = false;
        WFB2HB0500Add.Visible = true;
        HID_Freeze.Value = "Y";
    }
}