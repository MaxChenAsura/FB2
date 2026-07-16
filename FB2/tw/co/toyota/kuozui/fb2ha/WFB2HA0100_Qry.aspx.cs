using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0100_Qry : BasePage
{
    //Service 物件
    private CFB2HA0100BO service = new CFB2HA0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        GetResourceMessageToJavaScript();        
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生部門層級下拉式選單
            createDeptLevel();
            
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        gv_result.PagerSettings.Visible = true;
    }

    //將 Resources 訊息存入物件
    private void GetResourceMessageToJavaScript()
    {
        hid_cancel_ConfirmMessage.Value = Resources.Resource.wfb2hc_Cancel_Confirm_Message;
        hid_delete_ConfirmMessage.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;
        hid_notChooseMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
        hid_chooseOneMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage;
    }

    private void createDeptLevel()
    {
        try
        {
            ddl_DEPT_LEVEL.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getDeptLevel();
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
                getSortDirection("DEPT_LEVEL");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_LEVEL" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "DEPT_LEVEL" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //部門層級
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_EDIT_LEVEL_TYPE");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_EDIT_LEVEL_TYPE");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("LEVEL_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid != null)
                    ddl.SelectedValue = hid.Value;
            }
            //是否使用
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_EDIT_IS_VALID");
            HiddenField hid2 = (HiddenField)e.Row.FindControl("hid_EDIT_IS_VALID");
            if (ddl2 != null)
            {
                if (hid2 != null)
                    ddl2.SelectedValue = hid2.Value;
            }


            

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //部門明細
            HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("btn_Detail");
            if (btn != null)
            {
                btn.Attributes.Add("onclick", "openHA020('" + gv_result.DataKeys[e.Row.RowIndex].Value + "');");
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
        

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_TYPE");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("LEVEL_TYPE", "", "");
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
        gv_result.DataKeyNames = new string[] { "DEPT_LEVEL" }; //設定GridView Key
    }

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
    protected void WFB2HA010Search_Click(object sender, EventArgs e)
    {
        try
        {
            hid_DEPT_LEVEL.Value = ddl_DEPT_LEVEL.Text;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO", 0, 10);
            //end
            
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0100Add.Visible = true;
                WFB2HA0100Edit.Visible = true;
                WFB2HA0100Delete.Visible = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA010Add_Click(object sender, EventArgs e)
    {
        try
        {
            hid_DEPT_LEVEL.Value = ddl_DEPT_LEVEL.Text;
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2HA0100Search.Enabled = false;
            btn_clear.Disabled = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO", 0, 10);

            WFB2HA0100Save.Visible = true;
            WFB2HA0100Cancel.Visible = true;

            WFB2HA0100Add.Visible = false;
            WFB2HA0100Edit.Visible = false;
            WFB2HA0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;


        }
        catch (Exception)
        {
            
            throw;
        }
    }
    protected void WFB2HA010Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> dept_level = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dept_level.Add(gv_result.DataKeys[i].Values["DEPT_LEVEL"].ToString());
                   
                }
            }
            string msg = service.delete_DeptLevel(dept_level);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                createDeptLevel();
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
    protected void WFB2HA010Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2HA0100Search.Enabled = false;
            gv_result.PagerSettings.Visible = false;
            btn_clear.Disabled = true;

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
            WFB2HA0100Save.Visible = true;
            WFB2HA0100Cancel.Visible = true;

            WFB2HA0100Add.Visible = false;
            WFB2HA0100Edit.Visible = false;
            WFB2HA0100Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA010Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_DEPT_LEVEL = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_LEVEL");
                DropDownList ddl_NEW_LEVEL_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_LEVEL_TYPE");
                TextBox txt_NEW_DEPT_LEVEL_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_LEVEL_DESC");
                DropDownList ddl_NEW_IS_VALID = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_IS_VALID");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");


                CFB2HA0100DAO fb2ha010 = new CFB2HA0100DAO();
                fb2ha010.DEPT_LEVEL = txt_NEW_DEPT_LEVEL.Text;
                fb2ha010.DEPT_LEVEL_DESC = txt_NEW_DEPT_LEVEL_DESC.Text;
                fb2ha010.LEVEL_TYPE = ddl_NEW_LEVEL_TYPE.SelectedValue;
                fb2ha010.IS_VALID = ddl_NEW_IS_VALID.SelectedValue;
                fb2ha010.REMARK = txt_NEW_REMARK.Text;

                fb2ha010.CREATED_BY = SessionHandle.Current.emp_id;
                fb2ha010.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2ha010.FUNC_ID = "FB2HA010";
                string msg = service.addDept_Level(fb2ha010);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    createDeptLevel();
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_DEPT_LEVEL = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_LEVEL");
                    DropDownList ddl_NEW_LEVEL_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_LEVEL_TYPE");
                    TextBox txt_NEW_DEPT_LEVEL_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_LEVEL_DESC");
                    DropDownList ddl_NEW_IS_VALID = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_IS_VALID");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    CFB2HA0100DAO fb2ha010 = new CFB2HA0100DAO();
                    fb2ha010.DEPT_LEVEL = txt_NEW_DEPT_LEVEL.Text;
                    fb2ha010.DEPT_LEVEL_DESC = txt_NEW_DEPT_LEVEL_DESC.Text;
                    fb2ha010.LEVEL_TYPE = ddl_NEW_LEVEL_TYPE.SelectedValue;
                    fb2ha010.IS_VALID = ddl_NEW_IS_VALID.SelectedValue;
                    fb2ha010.REMARK = txt_NEW_REMARK.Text;

                    fb2ha010.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2ha010.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2ha010.FUNC_ID = "FB2HA010";
                    string msg = service.addDept_Level(fb2ha010);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        createDeptLevel();
                        showMessage("addSuccessMessage", " \\n 新增層級之相關部門資料，請使用【部門基本資料維護】功能進行維護");
                    }
                    
                }
                else
                {
                    //更新
                    DropDownList ddl_EDIT_LEVEL_TYPE = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_LEVEL_TYPE");
                    TextBox txt_EDIT_DEPT_LEVEL_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_DEPT_LEVEL_DESC");
                    DropDownList ddl_EDIT_IS_VALID = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_IS_VALID");
                    TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");

                    CFB2HA0100DAO fb2ha010 = new CFB2HA0100DAO();
                    fb2ha010.DEPT_LEVEL = gv_result.DataKeys[gv_result.EditIndex].Values["DEPT_LEVEL"].ToString();
                    fb2ha010.DEPT_LEVEL_DESC = txt_EDIT_DEPT_LEVEL_DESC.Text;
                    fb2ha010.LEVEL_TYPE = ddl_EDIT_LEVEL_TYPE.SelectedValue;
                    fb2ha010.IS_VALID = ddl_EDIT_IS_VALID.SelectedValue;
                    fb2ha010.REMARK = txt_EDIT_REMARK.Text;
                    fb2ha010.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2ha010.FUNC_ID = "FB2HA010";
                    string msg = service.updateDept_Level(fb2ha010);
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
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_LEVEL"};
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HA0100Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2HA0100Save.Visible = false;
            WFB2HA0100Cancel.Visible = false;
            WFB2HA0100Add.Visible = true;
            WFB2HA0100Edit.Visible = true;
            WFB2HA0100Delete.Visible = true;


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA010Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HA0100Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HA0100Edit.Visible = true;
            WFB2HA0100Delete.Visible = true;
        }

        WFB2HA0100Save.Visible = false;
        WFB2HA0100Cancel.Visible = false;
        WFB2HA0100Add.Visible = true;
       
    }
   
}