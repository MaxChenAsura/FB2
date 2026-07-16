using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ib_WFB2IB0700_Qry : BasePage
{
    CFB2IB0700BO service = new CFB2IB0700BO();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ScriptManager.RegisterClientScriptBlock(GridView1, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
              
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    protected void WFB2IB0700Search_Click(object sender, EventArgs e)
    {
        CFB2IB0700DAO dao = new CFB2IB0700DAO();
        try
        {
            if (!service.IsNumeric(txt_PAYMENT_DATE_YM.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "下載年度輸入錯誤" + "');", true);
                return;
            }
            else
            {
                if (!service.IsNumeric(txt_EMP_ID.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "工號輸入錯誤" + "');", true);
                    return;
                }else{
                    dao.PAYMENT_DATE_YM = txt_PAYMENT_DATE_YM.Text;
                    dao.EMP_ID = txt_EMP_ID.Text;

                    ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
                    ViewState["SortExpression"] = null; //排序欄位
                    ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

                    //GridView有分頁此段必加 begin
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView("PAYMENT_DATE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView("PAYMENT_DATE", 0, 10000);
                    //end
                    if (gv_result.Rows.Count > 0)
                    {
                        lb_before_adjust.Visible = true;
                        WFB2IB0700Adjust.Visible = true;
                        lb_explain.Visible = false;
                        lb_Minus_Value.Visible = false;
                        //WFB2DF0200ExcelDown.Visible = true;
                    }
                    else
                    {
                        lb_explain.Visible = false;
                        lb_Minus_Value.Visible = false;
                        lb_before_adjust.Visible = false;
                        WFB2IB0700Adjust.Visible = false;
                    }
                }                
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("PAYMENT_DATE,ACCU_AMOUNT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PAYMENT_DATE", "DATA_SOURCE", "SALARY_TYPE", "SALARY_ID", "EMP_ID", "PAY_KIND" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "PAYMENT_DATE", "DATA_SOURCE", "SALARY_TYPE", "SALARY_ID", "EMP_ID" }; //設定GridView Key
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
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PAYMENT_DATE", "DATA_SOURCE", "SALARY_TYPE", "SALARY_ID", "EMP_ID" }; //設定GridView Key
    }

    //取得GridView Function
    private void getGridView1(string SortExpression, int pageindex, Int32 pagesize)
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
                getSortDirection("PAYMENT_DATE,ACCU_AMOUNT");

            //GridView基本設定

            GridView1.PageIndex = 0;
            GridView1.PageSize = pagesize;
            GridView1.DataSourceID = "ods2";
            GridView1.DataKeyNames = new string[] { "PAYMENT_DATE", "DATA_SOURCE", "SALARY_TYPE", "SALARY_ID", "EMP_ID", "PAY_KIND" }; //設定GridView Key
            //gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound1(object sender, GridViewRowEventArgs e)
    {       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int coulumn4 = 0;
            TextBox txt = (TextBox)gv_result.Rows[e.Row.RowIndex].Cells[9].FindControl("txt_adjust");
            if (txt.Text != "")
            {
                coulumn4 = Convert.ToInt32(txt.Text);
                e.Row.Cells[4].Text = String.Format("{0:#,##0}", coulumn4);

                //累計金額資料
                if (e.Row.RowIndex == 0)
                {
                    e.Row.Cells[5].Text = String.Format("{0:#,##0}", coulumn4);
                }
                else
                {
                    int gm = Convert.ToInt32(GridView1.Rows[e.Row.RowIndex - 1].Cells[5].Text.Replace(",", "")) + Convert.ToInt32(e.Row.Cells[4].Text.Replace(",", ""));
                    //string gm1 = String.Format("{0:0,0}", gm);
                    e.Row.Cells[5].Text = String.Format("{0:#,##0}", gm);
                }
                //超過4倍投保金額的獎金           
                if ((Convert.ToInt32(e.Row.Cells[5].Text.Replace(",", "")) - Convert.ToInt32(e.Row.Cells[3].Text.Replace(",", ""))) > 0)
                {
                    int coulumn6 = Convert.ToInt32(e.Row.Cells[5].Text.Replace(",", "")) - Convert.ToInt32(e.Row.Cells[3].Text.Replace(",", ""));
                    e.Row.Cells[6].Text = String.Format("{0:#,##0}", coulumn6);
                }
                else
                {
                    e.Row.Cells[6].Text = "0";
                }
                //本月補充保費基準
                int coulumn7 = Math.Min(Convert.ToInt32(e.Row.Cells[4].Text.Replace(",", "")), Convert.ToInt32(e.Row.Cells[6].Text.Replace(",", "")));
                e.Row.Cells[7].Text = String.Format("{0:#,##0}", coulumn7);

                //本月補充保險費
                string st = e.Row.Cells[0].Text;
                string rate = service.selectPara(st.Replace("-", "").Replace("/", ""));
                //加入四捨五入
                int coulumn8 = Convert.ToInt32(Math.Round((Convert.ToDouble(e.Row.Cells[7].Text.Replace(",", "")) * Convert.ToDouble(rate)) / 100, 0, MidpointRounding.AwayFromZero));
                e.Row.Cells[8].Text = String.Format("{0:#,##0}", coulumn8);
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
    protected void gv_result_RowCreated1(object sender, GridViewRowEventArgs e)
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
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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

    protected void gv_result_Sorting1(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods2";
        gv_result.DataKeyNames = new string[] { "PAYMENT_DATE", "DATA_SOURCE", "SALARY_TYPE", "SALARY_ID", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void WFB2IB0700Adjust_Click(object sender, EventArgs e)
    {
        int after = 0;
        int before = 0;
        int result = 0;
        bool b = true;
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            TextBox txt = (TextBox)gv_result.Rows[i].Cells[9].FindControl("txt_adjust");
            if(txt.Text == ""){
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "請輸入所有的調整金額" + "');", true);
                return;                
            }
            b = int.TryParse(txt.Text ,NumberStyles.AllowLeadingSign, null, out result);
            if (!b)
            {
                break;
            }
        }
        if (!b)
        {
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                TextBox txt = (TextBox)gv_result.Rows[i].Cells[9].FindControl("txt_adjust");
                string txt1 = gv_result.Rows[i].Cells[4].Text;
                txt.Text = txt1.Replace(",","");
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "請輸入合法的調整金額" + "');", true);
            return;
        }

        ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
        ViewState["SortExpression"] = null; //排序欄位
        ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

        //GridView有分頁此段必加 begin
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView1("PAYMENT_DATE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView1("PAYMENT_DATE", 0, 10000);
        //end

        if (gv_result.Rows.Count > 0)
        {
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                before = before + Convert.ToInt32(gv_result.Rows[i].Cells[8].Text.Replace(",",""));//調整前
            }
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                after = after + Convert.ToInt32(GridView1.Rows[i].Cells[8].Text.Replace(",", ""));//調整後
            }
            int money = after - before;
            lb_Minus_Value.Text = String.Format("{0:#,##0}", money);

            lb_after_adjust.Visible = true;
            WFB2IB0700Save.Visible = true;
            //WFB2IB0700Cancel.Visible = true;
            lb_explain.Visible = true;
            lb_Minus_Value.Visible = true;
            WFB2IB0700Search.Visible = false;            
        }
    }
    protected void WFB2IB0700Save_Click(object sender, EventArgs e)
    {

        string msg = service.updateINS2_DETAIL(GridView1);
        if (msg != "0")
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            showMessage("modFailMessage", msg);            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('調整完成!');window.location.href = 'WFB2IB0700_Qry.aspx';", true);
            
        }
    }
    protected void WFB2IB0700Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2IB0700_Qry.aspx");
    }
}