using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2df_WFB2DF0300_Qry : BasePage
{
    CFB2DF0300BO service = new CFB2DF0300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {

        }
    }
    protected void WFB2DF0300Execute_Click(object sender, EventArgs e)
    {
        //隱藏按鈕
        WFB2DF0300ExcelDown.Enabled = false;
        WFB2DF0300Execute.Enabled = false;
        WFB2DF0300Search.Enabled = false;

        CFB2DF0300DAO dao = new CFB2DF0300DAO();
        string err = "";
        try { 
            dao.MANAGER_YM = txt_MANAGER_YM.Text.Replace("/","");
            dao.YM = txt_MANAGER_YM.Text.Replace("/", "");
            checkSalaryClose(dao);
            //是否能計算
            if (dao.SALARY_LOCKED == "Y")
            {
                err += "此管理年月的薪資已經鎖定，不可再重複計算\\n";
                WFB2DF0300ExcelDown.Enabled = true;
                WFB2DF0300Execute.Enabled = true;
                WFB2DF0300Search.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }            
            else {
                string msg = service.insertMonth(dao);

                if (msg != "0")
                {
                    showMessage("executeFailMessage", msg);
                    return;
                }
                else
                {
                    
                    showMessage("executeSuccessMessage");
                }
                //顯示按鈕
                WFB2DF0300ExcelDown.Enabled = true;
                WFB2DF0300Execute.Enabled = true;
                WFB2DF0300Search.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);            
        }
        

        
    }

    protected void WFB2DF0300ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {            
            CFB2DF0300DAO dao = new CFB2DF0300DAO();
            dao.MANAGER_YM = txt_MANAGER_YM.Text.Replace("/", "");
            DataTable tmp = dao.selectMonthData();
            if (tmp.Rows.Count > 0)
            {
                service.createExcel(dao, "xlsx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DF0300Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("MANAGER_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("MANAGER_YM", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                //WFB2DF0200Delete.Visible = true;
                //WFB2DF0200Edit.Visible = true;
                //WFB2DF0200ExcelDown.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void checkSalaryClose(CFB2DF0300DAO dao)
    {
        try
        {
            dao.SALARY_LOCKED = "";
            service.checkSalaryClose(dao);

        }
        catch (Exception)
        {

            throw;
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
                getSortDirection("MANAGER_YM", "DESC");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "MANAGER_YM" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "MANAGER_YM" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //判斷button是否disable
            string st = e.Row.Cells[2].Text; ;
            CFB2DF0300DAO dao = new CFB2DF0300DAO();
            int index = Convert.ToInt32(e.Row.RowIndex);
            string txt = e.Row.Cells[1].Text;
            dao.YM = txt;

            checkSalaryClose(dao);
            if (dao.SALARY_LOCKED == "Y")
            {
                Button bt = (Button)e.Row.Cells[3].FindControl("WFB2DF0300SalaryOut");
                bt.Enabled = false;
            }           
            //if (e.Row.Cells[2].Text != "" && e.Row.Cells[2].Text != "9999-12-31" && e.Row.Cells[2].Text != "&nbsp;")
            //{
            //    Button bt = (Button)e.Row.Cells[3].FindControl("WFB2DF0300SalaryOut");
            //    bt.Enabled = false;
            //}
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
        gv_result.DataKeyNames = new string[] { "MANAGER_YM" }; //設定GridView Key
    }

    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "SentToSalary")
        {
            CFB2DF0300DAO dao = new CFB2DF0300DAO();
            string err = "", msg = ""; ;
            int index = Convert.ToInt32(e.CommandArgument);
            //gv_result.Rows[index]
            Button bt = (Button)gv_result.Rows[index].FindControl("WFB2DF0300SalaryOut");
            bt.Enabled = false;


            string txt = gv_result.Rows[index].Cells[1].Text;
            //string st = Convert.ToString(txt);
            dao.YM = txt;
            err = service.getSalaryCode(dao);

            if (!err.Equals(""))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                bt.Enabled = true;
                return;
            }
            else
            {
                err = "";
                DataTable dt = new DataTable();
                dt = service.getSalaryCTL(dao);

                if (dt.Rows.Count > 0)
                {
                    dao.SALARY_LOCKED = dt.Rows[0]["SALARY_LOCKED"].ToString();
                    if (dao.SALARY_LOCKED == "Y")
                    {
                        err += "此月份薪資已經鎖定無法修改\\n";

                        //是否薪資已轉出
                        msg = service.getManagerDT(dao.YM);
                        if (msg == "")
                        {
                            dao.TAKE_OUT_BY = SessionHandle.Current.emp_id;
                            //update
                            service.update_Month(dao);

                            getGridView("MANAGER_YM", 0, 10);
                        }                        

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                        return;
                    }
                    else
                    {
                        //薪資還沒鎖定
                        dao.OPERATION_ID = "D01";
                        dao.FUNC_ID = "FB2DF030";
                        msg = service.updateSALARY_MONTH_CTRL(dao);
                        getGridView("MANAGER_YM", 0, 10);
                    }

                }
                else
                {
                    //新增一筆
                    dao.OPERATION_ID = "D01";
                    dao.FUNC_ID = "FB2DF030";
                    msg = service.insertSALARY_MONTH_CTRL(dao);
                    getGridView("MANAGER_YM", 0, 10);
                }

                if (msg != "0")
                {
                    showMessage("announceFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("announceSuccessMessage");
                }


            }
        }
    }


}