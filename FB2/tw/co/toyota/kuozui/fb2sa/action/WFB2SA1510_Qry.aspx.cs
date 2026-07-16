using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA1510_Qry : BasePage
{
    //Service 物件
    private CFB2SA1510BO service = new CFB2SA1510BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value
            txt_CONTACT_TO.Text = "XXG長(分機:XXX)或擔當XXX(分機:XXX)聯絡。";
        }
        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;


            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    protected void WFB2SA1510Execute_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            dao.DATA_YEAR = txt_DATA_YEAR.Text;
            dao.SEND_DT = txt_SEND_DT.Text;
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.MAIL_TITLE = txt_MAIL_TITLE.Text;
            dao.MAIL_DESC = txt_MAIL_DESC.Text;
            dao.CONTACT_TO = txt_CONTACT_TO.Text;
            dao.CC_EMAIL = txt_CC_EMAIL.Text;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "WFB2SA1510";
            string msg = service.excute(dao);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("addFailMessage", msg);
            }
            else
            {
                showMessage("addSuccessMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SA1510Search_Click(object sender, EventArgs e)
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
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SA1510Print.Visible = true;               
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
                getSortDirection("EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                //WFB2DD0100Detail.Visible = false;
                //WFB2DD0100Edit.Visible = false;

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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
    protected void WFB2SA1510Print_Click(object sender, EventArgs e)
    {
        CFB2SA1510DAO dao = new CFB2SA1510DAO();
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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dao.EMP_ID = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    if ( gv_result.DataKeys[i].Values["SEND_DT"] != null)
                    {
                        dao.SEND_DT = gv_result.DataKeys[i].Values["SEND_DT"].ToString();
                    }
                    else
                    {
                        dao.SEND_DT = "";
                    }
                    
                }
            }

            dao.DATA_YEAR = txt_DATA_YEAR.Text;

            DataTable salaryDT = service.get_PDF_Data(dao);
            DataTable salaryYM3 = service.getSalaryYM();//薪資年月
            DataTable vaa = service.getvaaData();
            DataTable vbb = service.getvbbData();
            DataTable before_Pay = service.before_Pay(dao);//調整前資格俸
            DataTable after_Pay = service.after_Pay(dao);//調整後資格俸

            string vdata_year = "";
            string vY07 = "";
            string vY0701 = "";            
            string CONTACT_TO = "";
            string salaryYM = "";
            string vDatePeriod = "";
            string before_sum = "";//調整前合計
            string after_sum = "";//調整後合計

            if (salaryDT.Rows.Count > 0)
            {
                if (vdata_year != salaryDT.Rows[0]["DATA_YEAR"].ToString())
                {
                    vY07 = salaryDT.Rows[0]["DATA_YEAR"].ToString() + "年" +
                        vbb.Rows[0]["CODE_VAL1"].ToString().Substring(0, 2) + "月" +
                        vbb.Rows[0]["CODE_VAL1"].ToString().Substring(2) + "日";
                    vY0701 = salaryDT.Rows[0]["DATA_YEAR"].ToString() + "年" +
                        vbb.Rows[0]["CODE_VAL1"].ToString().Substring(0, 2) + "月";
                    vdata_year = salaryDT.Rows[0]["DATA_YEAR"].ToString();
                    vDatePeriod = salaryDT.Rows[0]["DATA_YEAR"].ToString() + "/" +
                        vbb.Rows[0]["CODE_VAL1"].ToString().Substring(0, 2) + " ~ " +
                        Convert.ToString(Convert.ToInt32(salaryDT.Rows[0]["DATA_YEAR"]) + 1) + "/06";

                }
                               
                salaryYM = salaryYM3.Rows[0]["salaryYM"].ToString();//薪資計算年月
                if (dao.SEND_DT == "")
                {
                    CONTACT_TO = txt_CONTACT_TO.Text;
                }
                else
                {
                    CONTACT_TO = salaryDT.Rows[0]["CONTACT_TO"].ToString();
                }
                string st1 = before_Pay.Rows[0]["AMOUNT"].ToString();
                string st2 = after_Pay.Rows[0]["AMOUNT"].ToString();
                //合計值
                before_sum = Convert.ToString(Convert.ToInt32(salaryDT.Rows[0]["ABILITY_PAY_B"].ToString()) + Convert.ToInt32(before_Pay.Rows[0]["AMOUNT"].ToString()));
                after_sum = Convert.ToString(Convert.ToInt32(salaryDT.Rows[0]["ABILITY_PAY_A"].ToString()) + Convert.ToInt32(after_Pay.Rows[0]["AMOUNT"].ToString()));

                // 建立報表參數陣列變數
                ReportParameter[] parameters = new ReportParameter[25];
                parameters[0] = new ReportParameter("vY07", vY07);
                parameters[1] = new ReportParameter("vY0701", vY0701);
                parameters[2] = new ReportParameter("vaa", vaa.Rows[0]["CODE_VAL1"].ToString().Substring(0, 2) + "月" +
                                                            vaa.Rows[0]["CODE_VAL1"].ToString().Substring(2) + "日");
                parameters[3] = new ReportParameter("vbb", vbb.Rows[0]["CODE_VAL1"].ToString().Substring(0, 2) + "月" +
                                                            vbb.Rows[0]["CODE_VAL1"].ToString().Substring(2) + "日");
                parameters[4] = new ReportParameter("EMP_NAME", salaryDT.Rows[0]["EMP_NAME"].ToString());
                parameters[5] = new ReportParameter("DEPT_NO", salaryDT.Rows[0]["DEPT_NO"].ToString());
                parameters[6] = new ReportParameter("EMP_ID", salaryDT.Rows[0]["EMP_ID"].ToString());
                parameters[7] = new ReportParameter("DEPT_NAME", salaryDT.Rows[0]["DEPT_NAME"].ToString());                
                parameters[8] = new ReportParameter("ABILITY_PAY_A", Convert.ToInt32( salaryDT.Rows[0]["ABILITY_PAY_A"].ToString()).ToString("N0"));
                parameters[9] = new ReportParameter("DATA_YEAR", salaryDT.Rows[0]["DATA_YEAR"].ToString());
                parameters[10] = new ReportParameter("CONTACT_TO", CONTACT_TO);
               // parameters[11] = new ReportParameter("LOGO", "");
                parameters[11] = new ReportParameter("SALARY_YM", salaryYM);
                parameters[12] = new ReportParameter("ABILITY_REPAY", Convert.ToInt32(salaryDT.Rows[0]["ABILITY_REPAY"].ToString()).ToString("N0"));
                parameters[13] = new ReportParameter("NO_TAX_OVERTIME_REPAY", Convert.ToInt32(salaryDT.Rows[0]["NO_TAX_OVERTIME_REPAY"].ToString()).ToString("N0"));
                parameters[14] = new ReportParameter("TAX_OVERTIME_REPAY", Convert.ToInt32(salaryDT.Rows[0]["TAX_OVERTIME_REPAY"].ToString()).ToString("N0"));
                parameters[15] = new ReportParameter("LEAVE_REPAY", Convert.ToInt32(salaryDT.Rows[0]["LEAVE_REPAY"].ToString()).ToString("N0"));
                parameters[16] = new ReportParameter("WORK_SHIFT_REPAY", Convert.ToInt32(salaryDT.Rows[0]["WORK_SHIFT_REPAY"].ToString()).ToString("N0"));
                parameters[17] = new ReportParameter("LEVEL_CD", salaryDT.Rows[0]["LEVEL_CD"].ToString());
                parameters[18] = new ReportParameter("GRADE_CD", salaryDT.Rows[0]["GRADE_CD"].ToString());
                parameters[19] = new ReportParameter("vDatePeriod", vDatePeriod);
                parameters[20] = new ReportParameter("LEVEL_PAY_OLD", Convert.ToInt32(before_Pay.Rows[0]["AMOUNT"].ToString()).ToString("N0"));
                parameters[21] = new ReportParameter("LEVEL_PAY_NEW", Convert.ToInt32(after_Pay.Rows[0]["AMOUNT"].ToString()).ToString("N0"));
                parameters[22] = new ReportParameter("BEFORE_PAY", Convert.ToInt32(before_sum).ToString("N0"));
                parameters[23] = new ReportParameter("AFTER_PAY", Convert.ToInt32(after_sum).ToString("N0"));
                parameters[24] = new ReportParameter("ABILITY_PAY_B", Convert.ToInt32(salaryDT.Rows[0]["ABILITY_PAY_B"].ToString()).ToString("N0"));
                
                ReportViewer reportviewer1 = new ReportViewer();
                //將ReportViewer1的DataSources集合清除
                reportviewer1.LocalReport.DataSources.Clear();
                //將ReportViewer1重置為初始狀態           
                reportviewer1.Reset();
                // 設定 ReportViewer1 的 DataSources
                reportviewer1.LocalReport.Refresh();
                // 給 ReportViewer1 新的設定
                reportviewer1.LocalReport.ReportPath = Server.MapPath("~/report/WFB2SA151PDF.rdlc");
                // 設定 ReportViewer1 的參數, 把值傳過去
                reportviewer1.LocalReport.SetParameters(parameters);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                byte[] bytes = reportviewer1.LocalReport.Render(
                                          "PDF", null, out mimeType, out encoding, out filenameExtension,
                                          out streamids, out warnings);

                //將Byte內容寫到Client
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=WFB2SA151PDF.{0}", filenameExtension));
                //Response.BinaryWrite(bytes);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
            }         

            

        }  
        
    }
}