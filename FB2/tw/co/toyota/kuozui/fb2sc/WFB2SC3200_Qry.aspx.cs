using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2sc_WFB2SC3200_Qry : BasePage
{
    CFB2SC3200BO service = new CFB2SC3200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            txt_SALARY_YM.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM");
            hid_YM.Value = DateTime.Now.AddMonths(-1).ToString("yyyy/MM");
            getprocess_status();
            hid_WKduthwker.Value = "0";
            hid_WKmember.Value = "0"; //正社員伙食費人數
            hid_WKpaytotal.Value = "0"; //薪資發放人數文字
            hid_WKmemofstu.Value = "0"; 
            ViewState["NewPageIndex"] = 0;
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SALARY_DT", "SALARY_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            string SALARY_YM = Convert.ToString(DataRow["SALARY_YM"]);
            e.Row.Cells[3].Text = SALARY_YM.Substring(0, 4) + "/" + SALARY_YM.Substring(4, 2);
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
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "SALARY_DT", "SALARY_TYPE" }; //設定GridView Key
    }
    protected void WFB2SC3200Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SALARY_YM desc", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("SALARY_YM desc", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                gv_result.Visible = true;
                WFB2SC3200Print.Visible = true;
                gv_result.ShowFooter = false;
            }
            else
            {
                gv_result.Visible = false;
                WFB2SC3200Print.Visible = false;
                showMessage("QryNotFoundMessage");
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
                getSortDirection("SALARY_YM ", "desc");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_DT", "SALARY_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void getprocess_status()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getprocess_status();
            ddl_PROCESS_STATUS.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + '-' + dt.Rows[i]["PROCESS_STATUS"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC3200Print_Click(object sender, EventArgs e)
    {
        CFB2SC3200DAO dao = new CFB2SC3200DAO();
        string Message = "";
        string SALARY_YM = "";
        string status = "";
        //try //"SALARY_DT", "SALARY_TYPE"
        //{
            List<Tuple<string, string>> SALARY_DT = new List<Tuple<string, string>>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    SALARY_DT.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["SALARY_DT"].ToString(), gv_result.DataKeys[i].Values["SALARY_TYPE"].ToString()));
                    SALARY_YM = gv_result.Rows[i].Cells[3].Text.ToString().Replace("/", "");
                    status = gv_result.Rows[i].Cells[9].Text.ToString();
                }
            }
            if (SALARY_DT.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請點選一筆資料!')", true);
                return;
            }
            if (SALARY_DT.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請點選一筆資料!')", true);
                return;
            }
            else
            {
                DataTable dt, dt2 = new DataTable();
                //if (Message == "")
                //{
                //    dt = service.getSALARY_TYPE(SALARY_DT);
                //    SALARY_DT[0].Item2 = dt.Rows[0]["SUB_CD"].ToString();
                //}
                //dt = service.tryPROCESS_STATUS(SALARY_DT, SALARY_DT[0].Item2);
                if (Message == "")
                {
                    if (status.IndexOf("3") == -1 && status.IndexOf("4") == -1)
                        Message = "該月薪資未關帳,無法提供薪資解析!!";
                }
                if (Message != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Message + "');", true);
                }
                else
                {
                    //X為暫存累加用
                    int x = Convert.ToInt32(hid_WKmember.Value);
                    
                    if (status.IndexOf("3") != -1)
                    {
                        service.deleteSIS(SALARY_DT);
                        dt = service.checkRESULTcnt(SALARY_DT, SALARY_DT[0].Item2);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["EMP_CD"].ToString() == "2")
                            {
                                hid_WKduthwker.Value = dt.Rows[i]["cnt"].ToString();
                            }
                            else
                            {
                                hid_WKmember.Value = dt.Rows[i]["cnt"].ToString();
                            }
                        }

                        //Y為暫存累加用
                        int y = Convert.ToInt32(hid_WKpaytotal.Value);
                        dt = service.checkRESULTcnt_equal(SALARY_DT, SALARY_DT[0].Item2); //建教生伙食費
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            hid_WKmemofstu.Value = dt.Rows[i]["cnt"].ToString();
                            y = y + Convert.ToInt32(dt.Rows[i]["cnt"]);
                           // hid_WKpaytotal.Value = hid_WKpaytotal+y.ToString(); BY EVA MARK 2015/6/23
                        }

                        dt = service.checkRESULTcnt_total(SALARY_DT, SALARY_DT[0].Item2); //薪資發放人數
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            hid_WKpaytotal.Value = dt.Rows[i]["cnt"].ToString();
                        }


                        dt = service.check_SA_GR_H();
                        string level = "";//xx用來存LEVEL
                        string[,] AMT = new string[dt.Rows.Count, 2];
                        if (dt.Rows.Count != 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AMT[i, 1] = dt.Rows[i]["GROUP_ID"].ToString();
                                level = dt.Rows[i]["LEVEL"].ToString();
                                dt2 = service.checkPAY(SALARY_DT, dt.Rows[i]["GROUP_ID"].ToString(), SALARY_DT[0].Item2, level);
                                if (dt2.Rows[0]["AMT"].ToString() != "")
                                {
                                    AMT[i, 0] = dt2.Rows[0]["AMT"].ToString();
                                }
                                else { AMT[i, 0] = "0"; }

                            }
                        }
                        //開始新增作業
                        CFB2SC3200DAO cfb2sc3200 = new CFB2SC3200DAO();
                        cfb2sc3200.WKduthwker = hid_WKduthwker.Value;
                        cfb2sc3200.WKmember = hid_WKmember.Value;
                        cfb2sc3200.WKmemofstu = hid_WKmemofstu.Value;
                        cfb2sc3200.WKpaytotal = hid_WKpaytotal.Value;
                        cfb2sc3200.SALARY_DT = SALARY_DT[0].Item1;
                        cfb2sc3200.SALARY_TYPE = SALARY_DT[0].Item2;
                        cfb2sc3200.SALARY_YM = SALARY_YM.ToString();

                        cfb2sc3200.CREATED_BY = SessionHandle.Current.emp_id;
                        cfb2sc3200.UPDATED_BY = SessionHandle.Current.emp_id;
                        cfb2sc3200.FUNC_ID = "WFB2C320";

                        service.addSALARY_ANALYSIS(cfb2sc3200, AMT, dt.Rows.Count);
                    }
                    IWorkbook workbook = service.createExcel(SALARY_YM, "xlsx", SALARY_DT, status);
                    Session["SC3200_workbook"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SC3200_Qry.aspx?SC3200_FileType = excelDefault";
                    Session["SC3200_FileType"] = "excelDefault";
                    if (workbook != null)
                    {
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                    }
                }
            }
        //}
        //catch (Exception ex)
        //{
        //    logger.Error(ex.Message);
        //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        //}
    }
    public void exportExcel()
    {
        try
        {
            if (Session["SC3200_FileType"] != null && Session["SC3200_FileType"].ToString() != "")
            {
                string fileType = Session["SC3200_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["SC3200_workbook"];
                    Session["SC3200_FileType"] = "";
                    Session["SC3200_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2SC3200_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}