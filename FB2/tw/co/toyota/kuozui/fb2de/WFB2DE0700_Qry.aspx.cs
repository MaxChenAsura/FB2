using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0700_Qry : BasePage
{
    CFB2DE0700BO service = new CFB2DE0700BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value                    
            txt_MANAGER_DT_S.Enabled = true;
            txt_MANAGER_DT_E.Enabled = true;
            txt_WORK_DT.Enabled = true;
            rb_dt1.Checked = true;
            getMaxDT();
            txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            getEMP_Data();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            
            
           getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getMaxDT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getMaxDT();            
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txt_WORK_DT.Text = dt.Rows[0]["MANAGER_DT"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getEMP_Data()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEMPData(SessionHandle.Current.emp_id);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DE0700Search_Click(object sender, EventArgs e)
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
                getGridView("MANAGER_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("MANAGER_DT", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
                   
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
            Decimal money = 0;
            Decimal price = 0;
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("MANAGER_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "MANAGER_DT" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {                
                showMessage("QryNotFoundMessage");
                lb_totalMoney_Menu.Visible = false;
                lb_totalMoney.Visible = false;
                lb_every_money.Visible = false;
            }

            if (gv_result.Rows.Count > 0)
            {
                string st = "";
                string EMP_ID = txt_EMP_ID.Text;
                Boolean b1 = rb_dt1.Checked;
                Boolean b2 = rb_dt2.Checked;
                string WORK_DT = txt_WORK_DT.Text;
                string MANAGER_DT_S = txt_MANAGER_DT_S.Text;
                string MANAGER_DT_E = txt_MANAGER_DT_E.Text;
                DataTable dt = new DataTable();

                //取得總金額
                dt = service.getTotalAmount(EMP_ID, b1, b2, WORK_DT, MANAGER_DT_S, MANAGER_DT_E);
                if (dt.Rows.Count > 0)
                {
                    money = Convert.ToDecimal(dt.Rows[0]["MONEY"].ToString());
                }
                dt.Clear();
                //取得各金額的次數  待改
                dt = service.getEveryCount(EMP_ID, b1, b2, WORK_DT, MANAGER_DT_S, MANAGER_DT_E);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        price = Convert.ToDecimal(dt.Rows[i]["PRICE"].ToString());
                        st = st + String.Format("{0:#,##0}", price) + "元消費次數： " + dt.Rows[i]["mcount"].ToString() + "</br>";
                    }
                }
                

                lb_totalMoney.Text = String.Format("{0:#,##0}", money);
                lb_every_money.Text = st;

                lb_totalMoney_Menu.Visible = true;
                lb_totalMoney.Visible = true;
                lb_every_money.Visible = true;        
            }
            
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DE070_ddlPerPageRow"] = ViewState["PerPageRow"];
            
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
        gv_result.DataKeyNames = new string[] { "MANAGER_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "MANAGER_DT" }; //設定GridView Key
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
    
    protected void rb_dt1CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_WORK_DT.Enabled = true;

            rb_dt2.Checked = false;
            txt_MANAGER_DT_S.Text = "";
            txt_MANAGER_DT_E.Text = "";
            txt_MANAGER_DT_S.Enabled = false;
            txt_MANAGER_DT_E.Enabled = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            showMessage("errMessage", ex.Message);
        }

    }

    protected void rb_dt2CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            rb_dt1.Checked = false;
            txt_WORK_DT.Enabled = false;
            txt_WORK_DT.Text = "";
            txt_MANAGER_DT_S.Enabled = true;
            txt_MANAGER_DT_E.Enabled = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            showMessage("errMessage", ex.Message);
        }

    }
    
   
}