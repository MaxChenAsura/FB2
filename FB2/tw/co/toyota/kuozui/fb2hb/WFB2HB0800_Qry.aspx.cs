using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2HB0800_Qry : BasePage 
{
    //Service 物件
    private CFB2HB0800BO service = new CFB2HB0800BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            DataTable dt = new DataTable();
            //
           

            //查詢條件及自動查詢
            getQryField();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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
                getSortDirection("EMP_ID", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID"}; //設定GridView Key

            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0130_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2HB0800Search_Click(object sender, EventArgs e)
    {
      
        try
        {
            //保留查詢條件
            setQryField(true);

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
           
            if (gv_result.Rows.Count > 0)
            {
                WFB2HB0800Add.Visible = true;
                WFB2HB0800Edit.Visible = true;
                WFB2HB0800Delete.Visible = true;
            }
            else
            {
                WFB2HB0800Edit.Visible = false;
                WFB2HB0800Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2HB0800Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2HB0800Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);

            WFB2HB0800Save.Visible = true;
            WFB2HB0800Cancel.Visible = true;

            WFB2HB0800Add.Visible = false;
            WFB2HB0800Edit.Visible = false;
            WFB2HB0800Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;

            gv_result.Visible = true; 
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除按鈕事件
    protected void WFB2HB0800Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string>> target_type =
                new List<Tuple<string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    HiddenField hidEmpStatus=(HiddenField)gv_result.Rows[i].FindControl("hid_EMP_STATUS");
                    if (hidEmpStatus.Value != "99")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('在職員工不允許刪除!')", true);
                        return;
                    }
                    target_type.Add(
                        new Tuple<string>(
                            gv_result.DataKeys[i].Values["EMP_ID"].ToString()));

                }
            }
            
            string msg = service.Delete(target_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            
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

    //修改按鈕事件
    protected void WFB2HB0800Edit_Click(object sender, EventArgs e)
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
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2HB0800Search.Visible = false;
            btn_clear.Visible = false;

            WFB2HB0800Save.Visible = true;
            WFB2HB0800Cancel.Visible = true;

            WFB2HB0800Add.Visible = false;
            WFB2HB0800Edit.Visible = false;
            WFB2HB0800Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    
    //確認按鈕
    protected void WFB2HB0800Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID_Add");
                TextBox LANGUAGE_JAPANESE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_LANGUAGE_JAPANESE_Add");
                TextBox LANGUAGE_TOEIC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_LANGUAGE_TOEIC_Add");


                CFB2HB0800DAO wfb2hb = new CFB2HB0800DAO();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ASSESS_YEAR.Text +";"+ASSESS_TYPE.SelectedValue +";"+ WS_CD.SelectedValue +";"+GRP_CD.Text +";"+GRP_NAME.Text +";"+ "');", true);
                wfb2hb.EMP_ID = EMP_ID.Text;
                wfb2hb.LANGUAGE_JAPANESE = LANGUAGE_JAPANESE.Text;
                wfb2hb.LANGUAGE_TOEIC =  LANGUAGE_TOEIC.Text;

                string msg = service.Add(wfb2hb);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
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
                    Control KeyinRow = null;
                    KeyinRow = gv_result.FooterRow;
                    //新增
                    TextBox EMP_ID = (TextBox)KeyinRow.FindControl("txt_EMP_ID_Add");
                    TextBox LANGUAGE_JAPANESE = (TextBox)KeyinRow.FindControl("txt_LANGUAGE_JAPANESE_Add");
                    TextBox LANGUAGE_TOEIC = (TextBox)KeyinRow.FindControl("txt_LANGUAGE_TOEIC_Add");

                    CFB2HB0800DAO wfb2hb = new CFB2HB0800DAO();
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ASSESS_YEAR.Text +";"+ASSESS_TYPE.SelectedValue +";"+ WS_CD.SelectedValue +";"+GRP_CD.Text +";"+GRP_NAME.Text +";"+ "');", true);
                    wfb2hb.EMP_ID = EMP_ID.Text;
                    wfb2hb.LANGUAGE_JAPANESE = LANGUAGE_JAPANESE.Text;
                    wfb2hb.LANGUAGE_TOEIC = LANGUAGE_TOEIC.Text;




                    string msg = service.Add(wfb2hb);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
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
                    Control KeyinRow = null;
                    if (gv_result.Rows.Count == 0)
                        KeyinRow = gv_result.Controls[0].Controls[0];
                    else
                    {
                        if (gv_result.EditIndex == -1)
                            KeyinRow = gv_result.FooterRow;
                        else
                            KeyinRow = gv_result.Rows[gv_result.EditIndex];
                    }
                    //更新

                    TextBox LANGUAGE_JAPANESE = (TextBox)KeyinRow.FindControl("txt_LANGUAGE_JAPANESE_Add");
                    TextBox LANGUAGE_TOEIC = (TextBox)KeyinRow.FindControl("txt_LANGUAGE_TOEIC_Add");
                    CFB2HB0800DAO wfb2hb = new CFB2HB0800DAO();
                    wfb2hb.EMP_ID = gv_result.DataKeys[gv_result.EditIndex].Values["EMP_ID"].ToString();

                    wfb2hb.LANGUAGE_JAPANESE = LANGUAGE_JAPANESE.Text;
                    wfb2hb.LANGUAGE_TOEIC = LANGUAGE_TOEIC.Text;
                    string msg = service.Update(wfb2hb);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2HB0800Search.Visible = true;
            btn_clear.Visible = true;

            WFB2HB0800Save.Visible = false;
            WFB2HB0800Cancel.Visible = false;
            WFB2HB0800Add.Visible = true;
            WFB2HB0800Edit.Visible = true;
            WFB2HB0800Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    

    //取消按鈕
    protected void WFB2HB0800Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2HB0800Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HB0800Edit.Visible = true;
            WFB2HB0800Delete.Visible = true;
        }

        WFB2HB0800Save.Visible = false;
        WFB2HB0800Cancel.Visible = false;
        WFB2HB0800Add.Visible = true;
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            
            
         

        }

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
        //已離職不允許刪除修改
        /**
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hidEmpStatus = (e.Row.FindControl("hid_EMP_STATUS") as HiddenField);
            if (hidEmpStatus.Value == "99" )
            {

                //CheckBox CheckBox2= (e.FindControl("cb_all") as CheckBox); 
                CheckBox CheckBox1 = (e.Row.FindControl("cb_check") as CheckBox);
                CheckBox1.Enabled = false;
                //CheckBox2.Enabled = false;

            }
        }**/
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
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("HB0800_Is_Search").ToString() == "Y")
            {

                txt_EMP_ID.Text = hashtable_get("HB0800_txt_EMP_ID").ToString();

                ViewState["PerPageRow"] = hashtable_get("HB0800_ddlPerPageRow").ToString();
                WFB2HB0800Search_Click(null, null);
                setQryField(false);
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
            //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("HB0800_txt_EMP_ID", txt_EMP_ID.Text);
        }
        else
        {
            hashtable_set("HB0800_Is_Search", "N");
        }
    }




    #endregion
}