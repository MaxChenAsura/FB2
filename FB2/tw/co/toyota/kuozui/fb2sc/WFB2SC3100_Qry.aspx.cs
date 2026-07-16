using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//IWorkbook需要
using System.IO;
using NPOI.SS.UserModel;

public partial class WebContent_fb2sc_WFB2SC3100_Qry : BasePage
{
    private CFB2SC3100BO service = new CFB2SC3100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            txt_SALARY_YM.Text = DateTime.Today.AddMonths(-1).ToString("yyyy/MM");
            hid_SALARY_YM.Value = DateTime.Today.AddMonths(-1).ToString("yyyy/MM");
            ViewState["NewPageIndex"] = 0;
            getSALARY_TYPE();//發薪類別
            getPROCESS_STATUS();//薪資年月
            getCOMPANY_CD();//公司別

        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }


    private void getCOMPANY_CD()
    {
        try
        {
            CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
            DataTable dt = new DataTable();
            dt = fb2sc.getCOMPANY_CD();
            ddl_COMPANY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["COMPANY_CD"].ToString() + '-' + dt.Rows[i]["COMPANY_SNAME"].ToString(), dt.Rows[i]["COMPANY_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPROCESS_STATUS()
    {
        try
        {
            CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
            DataTable dt = new DataTable();
            dt = fb2sc.getPROCESS_STATUS("SC", "PROCESS_STATUS", "Y");
            ddl_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString() + '-' + dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getSALARY_TYPE()
    {
        try
        {
            CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
            DataTable dt = new DataTable();
            dt = fb2sc.getSALARY_TYPE("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString() + '-' + dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("SALARY_YM desc,SALARY_TYPE,SALARY_DT");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "hid_PROCESS_STATUS", "hid_SALARY_TYPE", "SALARY_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "hid_PROCESS_STATUS", "hid_SALARY_TYPE", "SALARY_DT" }; //設定GridView Key
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
            e.Row.Cells[4].Text = SALARY_YM.Substring(0, 4) + "/" + SALARY_YM.Substring(4, 2);
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
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "hid_PROCESS_STATUS", "hid_SALARY_TYPE", "SALARY_DT" }; //設定GridView Key
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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


    protected void WFB2SC3100Search_Click(object sender, EventArgs e)
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
                WFB2SC3100Search.Visible = true;
                WFB2SC3100Print.Visible = true;
                WFB2SC3100Print2.Visible = true;
                lb_COMPANY_CD.Visible = true;
                ddl_COMPANY_CD.Visible = true;
                gv_result.ShowFooter = false;
            }
            else
            {
                WFB2SC3100Search.Visible = true;
                WFB2SC3100Print.Visible = false;
                WFB2SC3100Print2.Visible = false;
                lb_COMPANY_CD.Visible = false;
                ddl_COMPANY_CD.Visible = false;
                gv_result.ShowFooter = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無相關資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //薪資彙計表
    protected void WFB2SC3100Print_Click(object sender, EventArgs e)
    {
        int checkedCount = 0; //checkedCount:勾選筆數
        int jj = 0;
        int kk = 0;
        int gridRowIndex = 0; //jj=key的數目 kk = 選取筆數
        try
        {
            string[] count = new string[this.gv_result.Rows.Count];
            List<Tuple<string, string>> salary_data = new List<Tuple<string, string>>();
            for (gridRowIndex = 0; gridRowIndex < this.gv_result.Rows.Count; gridRowIndex++)
            {
                if (((CheckBox)gv_result.Rows[gridRowIndex].FindControl("cb_check")).Checked)
                {
                    salary_data.Add(new Tuple<string, string>(gv_result.DataKeys[gridRowIndex].Values["hid_PROCESS_STATUS"].ToString(), gv_result.DataKeys[gridRowIndex].Values["hid_SALARY_TYPE"].ToString()));
                    bool isChecked = ((CheckBox)gv_result.Rows[gridRowIndex].FindControl("cb_check")).Checked;
                    if (isChecked)
                    {
                        count[checkedCount] = gv_result.Rows[gridRowIndex].Cells[4].Text;
                        checkedCount++;
                    }
                }
            }
            for (int counteach = 0; counteach < checkedCount; counteach++)
            {
                //若 資料列.發薪狀態<>'2'(薪資計算)或 '3'(關帳)或 '4'(月結),則顯示錯誤訊息"該月薪資未計算,無法提供薪資彙計表!!"
                if ((salary_data[counteach].Item1 != "2") && (salary_data[counteach].Item1 != "3") && (salary_data[counteach].Item1 != "4"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該月薪資未計算,無法提供薪資彙計表!!')", true);
                    return;
                }

                if (checkedCount > 1)
                {
                    for (counteach = 0; counteach < checkedCount; counteach++)
                    {
                        if ((salary_data[counteach].Item2 != "A") || (count[checkedCount - 2] != count[checkedCount - 1]))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('只限月薪資且薪資年月相同者才可複選!')", true);
                            return;
                        }
                    }
                }
            }
            if (salary_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請點選一筆資料!')", true);
                return;
            }
            else
            {
                CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
                for (gridRowIndex = 0; gridRowIndex < this.gv_result.Rows.Count; gridRowIndex++)
                {
                    if (((CheckBox)gv_result.Rows[gridRowIndex].FindControl("cb_check")).Checked)
                    {
                        if (jj < checkedCount)
                        {
                            fb2sc.PAY_ID = gv_result.Rows[gridRowIndex].Cells[10].Text;
                            fb2sc.SALARY_DT = gv_result.Rows[gridRowIndex].Cells[5].Text;
                            fb2sc.SALARY_YM = gv_result.Rows[gridRowIndex].Cells[4].Text.Replace("/", "");
                            fb2sc.PAY_KIND = ((HiddenField)gv_result.Rows[gridRowIndex].FindControl("hid_PAY_KIND")).Value;
                            fb2sc.PROCESS_STATUS = ((HiddenField)gv_result.Rows[gridRowIndex].FindControl("hid_PROCESS_STATUS")).Value;
                            fb2sc.SALARY_TYPE = ((HiddenField)gv_result.Rows[gridRowIndex].FindControl("hid_SALARY_TYPE")).Value;
                            fb2sc.RunSP_S_WFB2SC310();
                            DataTable dtSPresult = fb2sc.checkSP("SP_S_WFB2SC310");
                            string msg = "";
                            if (dtSPresult.Rows.Count > 0)
                            {
                                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                                {
                                    msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                                }
                            }
                        }
                        fb2sc.PAY_ID = gv_result.Rows[gridRowIndex].Cells[10].Text;
                        fb2sc.PAY_KIND_DESC = gv_result.Rows[gridRowIndex].Cells[6].Text;
                        fb2sc.SALARY_DT = gv_result.Rows[gridRowIndex].Cells[5].Text;
                        fb2sc.SALARY_YM = gv_result.Rows[gridRowIndex].Cells[4].Text.Replace("/", "");
                        fb2sc.SALARY_TYPE = gv_result.Rows[gridRowIndex].Cells[2].Text;
                        fb2sc.PROCESS_STATUS = gv_result.Rows[gridRowIndex].Cells[8].Text;
                        fb2sc.PAY_KIND = ((HiddenField)gv_result.Rows[gridRowIndex].FindControl("hid_PAY_KIND")).Value;
                        jj++;
                    }
                }
                fb2sc.COMPANY_CD = ddl_COMPANY_CD.SelectedValue.ToString();

                //先刪除原始的檔案
                File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SC310_1_" + SessionHandle.Current.emp_id + ".xlsx"));

                IWorkbook workbook = service.createExcel1(checkedCount, salary_data[0].Item2, fb2sc, "xlsx");  //kk

                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }

                dwnframe.Attributes["src"] = "WFB2SC3100_Qry.aspx?FileType_SC310 = print1";
                Session["FileType_SC310"] = "print1";
                
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SC310_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SC3100Print2_Click(object sender, EventArgs e)
    {
        int j = 0, i = 0;

        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            string[] count = new string[this.gv_result.Rows.Count];
            List<Tuple<string, string, string>> salary_data = new List<Tuple<string, string, string>>();
            for (i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    salary_data.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["hid_PROCESS_STATUS"].ToString(), gv_result.DataKeys[i].Values["hid_SALARY_TYPE"].ToString(), gv_result.DataKeys[i].Values["SALARY_DT"].ToString()));
                    bool isChecked = ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked;
                    if (isChecked)
                    {
                        count[j] = gv_result.Rows[i].Cells[4].Text;
                        j++;
                    }
                }
            }
            for (int counteach = 0; counteach < j; counteach++)
            {
                //若 資料列.發薪狀態<>'2'(薪資計算)或 '3'(關帳)或 '4'(月結),則顯示錯誤訊息"該月薪資未計算,無法提供薪資解析!!"
                if ((salary_data[counteach].Item1 != "2") && (salary_data[counteach].Item1 != "3") && (salary_data[counteach].Item1 != "4"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該月薪資未計算,無法提供薪資解析!!')", true);
                    return;
                }

                //if (salary_data[counteach].Item1 == "4")
                //{
                //列印時若多筆 檢核 月薪資類+可勾薪資年月相同 才可列印,否則顯示錯誤訊息"只限月薪資且薪資年月相同者才可複選!"
                if (j > 1)
                {
                    for (counteach = 0; counteach < j; counteach++)
                    {
                        if ((salary_data[counteach].Item2 != "A") || (count[j - 2] != count[j - 1]))
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('只限月薪資且薪資年月相同者才可複選!')", true);
                            return;
                        }
                    }
                }
                //}
            }
            if (salary_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請點選一筆資料!')", true);
                return;
            }
            else
            {
                for (i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        fb2sc.SALARY_DT = gv_result.Rows[i].Cells[5].Text;
                        fb2sc.SALARY_YM = gv_result.Rows[i].Cells[4].Text.Replace("/", "");
                        fb2sc.PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
                        fb2sc.PROCESS_STATUS = ((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value;
                        fb2sc.SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                        fb2sc.COMPANY_CD = ddl_COMPANY_CD.SelectedValue.ToString();

                        //fb2sc.PAY_KIND = gv_result.Rows[i].Cells[6].Text;
                        //fb2sc.SALARY_DT = gv_result.Rows[i].Cells[5].Text;
                        //fb2sc.SALARY_YM = gv_result.Rows[i].Cells[4].Text.Replace("/", "");
                        //fb2sc.SALARY_TYPE = gv_result.Rows[i].Cells[2].Text;
                        //fb2sc.PROCESS_STATUS = gv_result.Rows[i].Cells[7].Text;
                        //fb2sc.COMPANY_CD = ddl_COMPANY_CD.SelectedValue.ToString();

                        fb2sc.RunSP_S_SALARY_REPORT_WFB2SC310();
                        DataTable dtSPresult = fb2sc.checkSP("SP_S_SALARY_REPORT_WFB2SC310");
                        string msg = "";
                        if (dtSPresult.Rows.Count > 0)
                        {
                            //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                            if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                            {
                                msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                            }
                        }
                    }
                }
                fb2sc.COMPANY_CD = ddl_COMPANY_CD.SelectedValue.ToString();
                //先刪除原始的檔案
                File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SC310_2_" + SessionHandle.Current.emp_id + ".xlsx"));

                IWorkbook workbook = service.createExcel2(j, salary_data, fb2sc, "xlsx");
                
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無匯出資料'); $.unblockUI();", true);
                }

                dwnframe.Attributes["src"] = "WFB2SC3100_Qry.aspx?FileType_SC310 = print2";
                Session["FileType_SC310"] = "print2";

                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SC310_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
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
            if (Session["FileType_SC310"] != null && Session["FileType_SC310"].ToString() != "")
            {
                string FileType_SC310 = Session["FileType_SC310"].ToString();
                if (FileType_SC310 == "print1")
                {
                    Session["FileType_SC310"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SC310_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SC310_1.xlsx");
                }
                if (FileType_SC310 == "print2")
                {
                    Session["FileType_SC310"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SC310_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SC310_2.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}