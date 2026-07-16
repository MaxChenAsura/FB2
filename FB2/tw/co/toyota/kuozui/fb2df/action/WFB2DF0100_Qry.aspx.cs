using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2df_WFB2DF0100_Qry : BasePage
{
    //Service 物件
    private CFB2DF0100BO service = new CFB2DF0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            ViewState["NewPageIndex"] = 0;
            getDate();
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
                getSortDirection("BASE_NO");

            //GridView基本設定
           
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "BASE_NO" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "BASE_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
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
        gv_result.DataKeyNames = new string[] { "BASE_NO" }; //設定GridView Key
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
    protected void getDate()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("BASE_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("BASE_NO", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DF0100Add.Visible = true;
                WFB2DF0100Edit.Visible = true;
                WFB2DF0100Delete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料');", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DF0100Add_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("BASE_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("BASE_NO", 0, 10);

            WFB2DF0100Save.Visible = true;
            WFB2DF0100Cancel.Visible = true;

            WFB2DF0100Add.Visible = false;
            WFB2DF0100Edit.Visible = false;
            WFB2DF0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            gv_result.PagerSettings.Visible = false;

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2DF0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> BASE_NO = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    BASE_NO.Add(gv_result.DataKeys[i].Values["BASE_NO"].ToString());

                }
            }
            if (BASE_NO.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            string msg = service.delete_BaseNO(BASE_NO);
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
    protected void WFB2DF0100Edit_Click(object sender, EventArgs e)
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
            WFB2DF0100Save.Visible = true;
            WFB2DF0100Cancel.Visible = true;

            WFB2DF0100Add.Visible = false;
            WFB2DF0100Edit.Visible = false;
            WFB2DF0100Delete.Visible = false;
            gv_result.PagerSettings.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DF0100Save_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_BASE_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BASE_NO");
                TextBox txt_NEW_BASE_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BASE_NAME");
                TextBox txt_NEW_AMOUNT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AMOUNT");


                CFB2DF0100DAO dao = new CFB2DF0100DAO();
                dao.BASE_NO = txt_NEW_BASE_NO.Text.ToUpper();
                dao.BASE_NAME = txt_NEW_BASE_NAME.Text.ToUpper();
                dao.AMOUNT = txt_NEW_AMOUNT.Text.Replace(",","");
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2DF010";
                //if (!utilities.IsNatural_Number(dao.BASE_NO))
                //{
                //    err += "住宿費基準只能輸入英數字 \\n";
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                //    return;
                //}

                string msg = service.addBASE_NO(dao);
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
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_BASE_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_BASE_NO");
                    TextBox txt_NEW_BASE_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_BASE_NAME");
                    TextBox txt_NEW_AMOUNT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AMOUNT");

                    CFB2DF0100DAO dao = new CFB2DF0100DAO();
                    dao.BASE_NO = txt_NEW_BASE_NO.Text.ToUpper();
                    dao.BASE_NAME = txt_NEW_BASE_NAME.Text.ToUpper();
                    dao.AMOUNT = txt_NEW_AMOUNT.Text.Replace(",", "");
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2DF010";
                    //if (!utilities.IsNatural_Number(dao.BASE_NO))
                    //{
                    //    err += "住宿費基準只能輸入英數字 \\n";
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    //    return;
                    //}
                    string msg = service.addBASE_NO(dao);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false; 
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage", "");
                    }

                }
                else
                {
                    //更新

                    TextBox txt_EDIT_BASE_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_BASE_NAME");
                    TextBox txt_EDIT_AMOUNT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_AMOUNT");

                    CFB2DF0100DAO dao = new CFB2DF0100DAO();
                    dao.BASE_NO = gv_result.DataKeys[gv_result.EditIndex].Values["BASE_NO"].ToString();
                    dao.BASE_NAME = txt_EDIT_BASE_NAME.Text;
                    dao.AMOUNT = txt_EDIT_AMOUNT.Text.Replace(",", "");
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2DF010";
                  
                    string msg = service.updateBASE_NO(dao);
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
            gv_result.DataKeyNames = new string[] { "BASE_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

           

            WFB2DF0100Save.Visible = false;
            WFB2DF0100Cancel.Visible = false;
            WFB2DF0100Add.Visible = true;
            WFB2DF0100Edit.Visible = true;
            WFB2DF0100Delete.Visible = true;


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DF0100Cancel_Click(object sender, EventArgs e)
    {
        

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DF0100Edit.Visible = true;
            WFB2DF0100Delete.Visible = true;
        }

        WFB2DF0100Save.Visible = false;
        WFB2DF0100Cancel.Visible = false;
        WFB2DF0100Add.Visible = true;
        
    }
}