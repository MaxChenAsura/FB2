using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC5B00_Qry : BasePage
{
    private CFB2SC5B00BO service = new CFB2SC5B00BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;

        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            ViewState["NewPageIndex"] = 0;
            getSALARY_TYPE();//發薪類別
        }
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getSALARY_TYPE()
    {
        try
        {
            CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
            DataTable dt = new DataTable();
            dt = fb2sc.getSALARY_TYPE("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

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
            {
                //getSortDirection("RowNumber");
                getSortDirection("SALARY_YM", "DESC");   //SALARY_YM desc,SALARY_TYPE,SALARY_DT
            }



            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "hid_SALARY_TYPE", "hid_PAY_KIND" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "hid_SALARY_TYPE", "hid_PAY_KIND" }; //設定GridView Key
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
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "hid_SALARY_TYPE", "hid_PAY_KIND" }; //設定GridView Key
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

    protected void WFB2SC5B00Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SALARY_YM desc,SALARY_TYPE,SALARY_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("SALARY_YM desc,SALARY_TYPE,SALARY_DT", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2SC5B00Search.Visible = true;
                WFB2SC5B0Excel.Visible = true;

            }
            else
            {
                WFB2SC5B00Search.Visible = true;
                WFB2SC5B0Excel.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2SC5B0Excel_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> salary_data = new List<Tuple<string, string>>();
            string SALARY_TYPE = "", SALARY_YM = "", SALARY_DT = "", PAY_KIND = "", hid_SALARY_TYPE = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    salary_data.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["hid_SALARY_TYPE"].ToString(), gv_result.DataKeys[i].Values["hid_PAY_KIND"].ToString()));
                    SALARY_TYPE = gv_result.Rows[i].Cells[2].Text.ToString();
                    SALARY_YM = gv_result.Rows[i].Cells[3].Text.ToString();
                    SALARY_DT = gv_result.Rows[i].Cells[4].Text.ToString();
                    PAY_KIND = gv_result.DataKeys[i].Values["hid_PAY_KIND"].ToString();
                    hid_SALARY_TYPE = gv_result.DataKeys[i].Values["hid_SALARY_TYPE"].ToString();
                }
            }
            if (salary_data.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                string emp_id = "";
                CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
                string deleteData = service.deleteData();
                string ins3day = service.ins3DayTmp();

                if (salary_data[0].Item1 == "A")
                {
                    DataTable selectEmp = service.selectEmp(SALARY_YM);
                    for (int i = 0; i < selectEmp.Rows.Count; i++)
                    {
                        emp_id = selectEmp.Rows[i]["EMP_ID"].ToString();

                        if (int.Parse(selectEmp.Rows[i]["LACK_HOUR"].ToString()) >= 480) //=480 分表示1天
                        {
                            string insertEmp = service.insertEmp(emp_id);
                        }
                    }
                    DataTable getData1 = fb2sc.getData1(SALARY_YM);
                    if (getData1.Rows.Count > 0)
                    {
                        IWorkbook workbook = service.CreatExcel(SALARY_TYPE, SALARY_YM, SALARY_DT, salary_data, "xlsx");
                        Session["workbook_fb2sc5b0"] = workbook;
                        dwnframe.Attributes["src"] = "WFB2SC5B00_Qry.aspx?FileType=fb2sc5b00";
                        Session["FileType_fb2sc5b0"] = "fb2sc5b00";
                        if (workbook != null)
                        {
                            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nodata", "alert('" + Resources.Resource.wfd2ia_nodata + "');", true);
                    }
                }
                if (salary_data[0].Item1 != "A")
                {
                    DataTable selectEmpTmp = service.selectEmpTmp(SALARY_DT, hid_SALARY_TYPE, PAY_KIND);
                    for (int i = 0; i < selectEmpTmp.Rows.Count; i++)
                    {
                        emp_id = selectEmpTmp.Rows[i]["EMP_ID"].ToString();
                        //DataTable CheckEmp = service.CheckEmp(emp_id);
                        //if (CheckEmp.Rows.Count > 0)
                        //{
                            //if (int.Parse(CheckEmp.Rows[0]["LACK_HOUR"].ToString()) >= 16)
                        if (int.Parse(selectEmpTmp.Rows[i]["LACK_HOUR"].ToString()) >= 16)
                            {
                                string insertEmp = service.insertEmp(emp_id);
                            }
                        //}
                    }
                    DataTable getData2 = fb2sc.getData2(SALARY_DT, salary_data[0].Item1, salary_data[0].Item2);
                    if (getData2.Rows.Count > 0)
                    {
                        IWorkbook workbook = service.CreatExcel(SALARY_TYPE, SALARY_YM, SALARY_DT, salary_data, "xlsx");
                        Session["workbook_fb2sc5b0"] = workbook;
                        dwnframe.Attributes["src"] = "WFB2SC5B00_Qry.aspx?FileType = fb2sc5b00";
                        Session["FileType_fb2sc5b0"] = "fb2sc5b00";
                        if (workbook != null)
                        {
                            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nodata", "alert('" + Resources.Resource.wfd2ia_nodata + "');", true);
                    }
                }
                //service.CreatExcel(SALARY_TYPE,SALARY_YM,SALARY_DT, salary_data, "xlsx");

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_fb2sc5b0"] != null && Session["FileType_fb2sc5b0"].ToString() != "")
            {
                string fileType = Session["FileType_fb2sc5b0"].ToString();

                IWorkbook workBook = (IWorkbook)Session["workbook_fb2sc5b0"];
                Session["FileType_fb2sc5b0"] = "";
                Session["workbook_fb2sc5b0"] = null;

                if (fileType == "fb2sc5b00")
                    ExcelHandle.exportExcel(workBook, "FB2SC5B0_1.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}