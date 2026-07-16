using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0600_Qry : BasePage
{
    CFB2DE0600BO service = new CFB2DE0600BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value
            createCLOCK_NO();           
            
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            
            
           getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }    

    private void createCLOCK_NO()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCLOCKData("-1");
            ddl_CLOCK_NO.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CLOCK_NO.Items.Add(new ListItem(dt.Rows[i]["clock_no"].ToString() + "-" + dt.Rows[i]["clock_desc"].ToString(), dt.Rows[i]["clock_no"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createddl_CLOCK_NO(DropDownList ddl_CLOCK_NO)
    {
        DataTable dt = new DataTable();
        dt = service.getALLCLOCKData();
        ddl_CLOCK_NO.Items.Add(new ListItem("", "-1"));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_CLOCK_NO.Items.Add(new ListItem(dt.Rows[i]["clock_no"].ToString()+"-"+dt.Rows[i]["clock_desc"].ToString(), dt.Rows[i]["clock_no"].ToString()));
            }
        }
    }

    protected void WFB2DE0600Search_Click(object sender, EventArgs e)
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
                getGridView("CLOCK_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CLOCK_NO", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DE0600Add.Visible = true;
                WFB2DE0600Edit.Visible = true;
                WFB2DE0600Delete.Visible = true;
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
                getSortDirection("CLOCK_NO");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                WFB2DE0600Edit.Visible = false;
                WFB2DE0600Delete.Visible = false;

                showMessage("QryNotFoundMessage");
            }
            

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DE060_ddlPerPageRow"] = ViewState["PerPageRow"];
            
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //給下拉選單預設值
            //string st1 = ((HiddenField)e.Row.FindControl("hid_IS_CANCEL")).Value;
            //((DropDownList)e.Row.FindControl("ddl_EDIT_IS_CANCEL")).SelectedValue= st1;
            //string st2 = ((HiddenField)e.Row.FindControl("hid_IS_CALCULATE")).Value;
            //((DropDownList)e.Row.FindControl("ddl_EDIT_IS_CALCULATE")).SelectedValue = st1;
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
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl_CLOCK_NO = (DropDownList)e.Row.FindControl("ddl_NEW_CLOCK_NO");
            createddl_CLOCK_NO(ddl_CLOCK_NO);           
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
    }

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

    protected void ddl_CLOCK_NOSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList dp = sender as DropDownList;            
            string s1 = dp.SelectedValue;

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

            DataTable dt = new DataTable();

            dt = service.getCLOCK(s1);
            if (dt.Rows.Count > 0)
            {
                ((Label)KeyinRow.FindControl("lb_NEW_CLOCK_DESC")).Text = dt.Rows[0]["clock_desc"].ToString();
                ((Label)KeyinRow.FindControl("lb_NEW_CLOCK_IP")).Text = dt.Rows[0]["CLOCK_IP"].ToString();
            }
            else
            {
                return;
            }    
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            showMessage("errMessage", ex.Message);
        }

    }

    protected void WFB2DE0600Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;           
                        
            btn_clear.Disabled = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CLOCK_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CLOCK_NO", 0, 10);

            WFB2DE0600Save.Visible = true;
            WFB2DE0600Cancel.Visible = true;

            WFB2DE0600Add.Visible = false;
            WFB2DE0600Delete.Visible = false;
            WFB2DE0600Edit.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2DE0600Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                CFB2DE0600DAO dao = new CFB2DE0600DAO();
                TextBox txt_NEW_MONEY = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_MONEY");
                DropDownList ddl_CLOCK_NO = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CLOCK_NO");


                dao.MONEY = txt_NEW_MONEY.Text;
                dao.CLOCK_NO = ddl_CLOCK_NO.SelectedItem.Value;              
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2DE060";

                gv_result.PagerSettings.Visible = false;
                string msg = service.addClockMoney(dao);

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
                    CFB2DE0600DAO dao = new CFB2DE0600DAO();
                    TextBox txt_NEW_MONEY = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_MONEY");
                    DropDownList ddl_CLOCK_NO = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CLOCK_NO");

                    dao.MONEY = txt_NEW_MONEY.Text;
                    dao.CLOCK_NO = ddl_CLOCK_NO.SelectedItem.Value;          
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2DE060";
                    gv_result.PagerSettings.Visible = false;
                    string msg = service.addClockMoney(dao);
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
                    CFB2DE0600DAO dao = new CFB2DE0600DAO();
                    TextBox txt_NEW_MONEY = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_MONEY");
                    Label lb_NEW_CLOCK_NO = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_NEW_CLOCK_NO");

                    dao.MONEY = txt_NEW_MONEY.Text;
                    dao.CLOCK_NO = lb_NEW_CLOCK_NO.Text;
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2DE060";
                    gv_result.PagerSettings.Visible = false;

                    string msg = service.updateCLOCK_MONEY(dao);
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
            gv_result.DataKeyNames = new string[] { "CLOCK_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2DE0600Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2DE0600Save.Visible = false;
            WFB2DE0600Cancel.Visible = false;
            WFB2DE0600Add.Visible = true;
            WFB2DE0600Delete.Visible = true;
            WFB2DE0600Edit.Visible = true;

            //給FLAG
            //hid_Valid_Flag.Value = "";
            //HID_ISADD.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DE0600Cancel_Click(object sender, EventArgs e)
    {
         //enable查詢清除按鈕
        //WFB2DD0100Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DE0600Add.Visible = true;
            WFB2DE0600Edit.Visible = true;
            WFB2DE0600Delete.Visible = true;            
        }

        WFB2DE0600Save.Visible = false;
        WFB2DE0600Cancel.Visible = false;
        //WFB2DD0100Add.Visible = true;

        
    }

    protected void WFB2DE0600Edit_Click(object sender, EventArgs e)
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

            
           

            //disable查詢清除按鈕
            WFB2DE0600Search.Enabled = false;
            btn_clear.Disabled = false;

            WFB2DE0600Save.Visible = true;
            WFB2DE0600Cancel.Visible = true;

            WFB2DE0600Add.Visible = false;
            WFB2DE0600Delete.Visible = false;
            WFB2DE0600Edit.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2DE0600Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> CLOCK_NO = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    CLOCK_NO.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (CLOCK_NO.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DE0600Edit, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(CLOCK_NO);

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
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DE0600Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}