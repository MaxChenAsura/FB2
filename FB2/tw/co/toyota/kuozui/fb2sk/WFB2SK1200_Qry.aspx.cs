using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sk_WFB2SK1200_Qry : BasePage
{
    private CFB2SK1200BO service = new CFB2SK1200BO();
    public static string year = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
           
            //查詢條件的預設值-工號,姓名
            txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            txt_EMP_DESC.Text = SessionHandle.Current.emp_name;
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        string dept_name = "";
        string dept_no = txt_DEPT_NO.Text;
        if (!string.IsNullOrEmpty(dept_no))
        {
            CFB2DL0300DAO dao = new CFB2DL0300DAO();
            DataTable dt = dao.getDept_name(dept_no);
            if (dt.Rows.Count == 1)
            {
                dept_name = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                txt_DEPT_NAME.Text = dept_name;
            }
            else
            {
                txt_DEPT_NAME.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "txt_DEPT_NO_error", "alert('部門代號輸入錯誤或不完整');", true);
            }
        }
        else
        {
            txt_DEPT_NAME.Text = "";
        }
    }
  
    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2SK1200BO service = new CFB2SK1200BO();
            dt = service.getEmpName(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_DESC.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_EMP_DESC.Text = "";
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("YEAR,LICENSE_ID");
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "YEAR", "EMP_ID", "LICENSE_ID", "DEPT_NO", "SALARY_EMAIL" }; //設定GridView Key
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK120Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "YEAR", "EMP_ID", "LICENSE_ID", "DEPT_NO", "SALARY_EMAIL" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";
        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
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
        gv_result.DataKeyNames = new string[] { "YEAR", "EMP_ID", "LICENSE_ID", "DEPT_NO", "SALARY_EMAIL" }; //設定GridView Key
    }


    protected void WFB2SK120Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("YEAR,LICENSE_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("YEAR,LICENSE_ID", 0, 10);
            //end
            CFB2SK1200DAO dao = new CFB2SK1200DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DATA_YM_search.Text
                                         , txt_DEPT_NO.Text, txt_EMP_ID.Text, txt_LICENSE_ID_search.Text);
            if (dataCount == 0)
            {
                WFB2SK120Search.Visible = true;
                btn_clear.Visible = true;
                WFB2SK120SendEmail.Visible = false;
                WFB2SK120Down.Visible = false;
                gv_result.Visible = false;
                OnePage.Visible = false;
                gv_result.ShowFooter = false;
                showMessage("QryNotFoundMessage");
            }
            else
            {
                WFB2SK120Search.Visible = true;
                btn_clear.Visible = true;
                WFB2SK120SendEmail.Visible = true;
                gv_result.Visible = true;
                OnePage.Visible = true;
                gv_result.ShowFooter = false;
                WFB2SK120Down.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SK120SendEmail_Click(object sender, EventArgs e)
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
            if (editindex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            string mail_content = "";
            List<Tuple<string, string, string, string, string>> emp_data = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    if (gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該員工無Email可寄送'); $.unblockUI();", true);
                        return;
                    }
                    emp_data.Add(new Tuple<string, string, string, string, string>(gv_result.DataKeys[i].Values["YEAR"].ToString(),
                gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["LICENSE_ID"].ToString(),
                gv_result.DataKeys[i].Values["DEPT_NO"].ToString(), gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString()));
                }
            }
            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料'); $.unblockUI();", true);
                return;
            }
            else
            {
                DataTable dtSendTo = service.getSendToEmail();
                string sendToEmail = "";
                string mailTitle = "";
                if (dtSendTo.Rows.Count > 0)
                    sendToEmail = dtSendTo.Rows[0]["CODE_VAL1"].ToString();//寄件者

                year = txt_DATA_YM_search.Text;
                DataTable dt_Content = service.getEmailContent();
                if (dt_Content.Rows.Count > 0)
                {
                    mail_content = dt_Content.Rows[0]["REMARK"].ToString();//內文
                    mailTitle = dt_Content.Rows[0]["CODE_VAL1"].ToString();//TITLE
                }

                string COMPANY_ID = "";
                string TAX_ORG_ID = "";
                string CATEGORY_INCOME  = "";
                string OTHER_ITEM = "";
                string UNIT_NAME = "";
                string UNIT_ADDR = "";
                string UNIT_MEN = "";
                string E183 = "";

                year = txt_DATA_YM_search.Text;
                //取得福利會資料
                DataTable dt_data = service.get_MUTUAL_Data();
                if (dt_data.Rows.Count > 0)
	            {
		            COMPANY_ID = dt_data.Rows[0]["COMPANY_ID"].ToString();
                    TAX_ORG_ID = dt_data.Rows[0]["TAX_ORG_ID"].ToString();
                    CATEGORY_INCOME = dt_data.Rows[0]["CATEGORY_INCOME"].ToString();
                    OTHER_ITEM = dt_data.Rows[0]["OTHER_ITEM"].ToString();
                    UNIT_NAME = dt_data.Rows[0]["UNIT_NAME"].ToString();
                    UNIT_ADDR = dt_data.Rows[0]["UNIT_ADDR"].ToString();
                    UNIT_MEN = dt_data.Rows[0]["UNIT_MEN"].ToString();                 
	            }

                for (int i = 0; i < emp_data.Count; i++)
                {
                    if (emp_data[i].Item5 != "")
                    {
                        E183 = "";
                        DataTable dt = new DataTable();
                        dt = service.get_PDF_Data(txt_DATA_YM_search.Text, emp_data);
                        dt.TableName = "DataTable1";

                        //是外籍人士 且 超過183天為Y的
                        if (dt.Rows[0]["JPN_CD"].ToString() != "" || dt.Rows[0]["JPN_CD"].ToString() != null)
                        {
                            if (dt.Rows[0]["EXCEED_183"].ToString() == "Y")
                            {
                                E183 = "Ｖ";
                            }
                        }

                        // 建立報表參數陣列變數
                        ReportParameter[] para = new ReportParameter[18];
                        para[0] = new ReportParameter("DEPT_NO", emp_data[i].Item4, true);
                        para[1] = new ReportParameter("EMP_ID", emp_data[i].Item2, true);
                        para[2] = new ReportParameter("EMP_NAME", dt.Rows[0]["EMP_NAME"].ToString(), true);
                        para[3] = new ReportParameter("DATA_YM", txt_DATA_YM_search.Text, true);
                        para[4] = new ReportParameter("COMPANY_ID", COMPANY_ID, true);
                        para[5] = new ReportParameter("TAX_ORG_ID", TAX_ORG_ID, true);
                        para[6] = new ReportParameter("CATEGORY_INCOME", CATEGORY_INCOME, true);
                        para[7] = new ReportParameter("OTHER_ITEM", OTHER_ITEM, true);
                        para[8] = new ReportParameter("UNIT_NAME", UNIT_NAME, true);
                        para[9] = new ReportParameter("UNIT_ADDR", UNIT_ADDR, true);
                        para[10] = new ReportParameter("UNIT_MEN", UNIT_MEN, true);
                        para[11] = new ReportParameter("YEAR_MONTH", "自" + txt_DATA_YM_search.Text + "年1月至" + txt_DATA_YM_search.Text + "年12月", true);
                        para[12] = new ReportParameter("REGISTER_ADDR", dt.Rows[0]["REGISTER_ADDR"].ToString(), true);
                        para[13] = new ReportParameter("PAYMENT_AMT", dt.Rows[0]["PAYMENT_AMT"].ToString(), true);
                        para[14] = new ReportParameter("E183", E183, true);
                        para[15] = new ReportParameter("YEAR", txt_DATA_YM_search.Text, true);
                        para[16] = new ReportParameter("LICENSE_ID", emp_data[i].Item3, true);
                        para[17] = new ReportParameter("MUTUAL_SEQ", dt.Rows[0]["MUTUAL_SEQ"].ToString(), true);

                        ReportViewer reportviewer1 = new ReportViewer();
                        //將ReportViewer1的DataSources集合清除
                        reportviewer1.LocalReport.DataSources.Clear();
                        //將ReportViewer1重置為初始狀態           
                        reportviewer1.Reset();
                        // 設定 ReportViewer1 的 DataSources
                        reportviewer1.LocalReport.Refresh();
                        // 給 ReportViewer1 新的設定
                        reportviewer1.LocalReport.ReportPath = Server.MapPath("~/report/WFB2SK120.rdlc");
                        // 設定 ReportViewer1 的參數, 把值傳過去
                        reportviewer1.LocalReport.SetParameters(para);

                        reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

                        Warning[] warnings;
                        string[] streamids;
                        string mimeType;
                        string encoding;
                        string filenameExtension;
                        byte[] bytes = reportviewer1.LocalReport.Render(
                                                  "PDF", null, out mimeType, out encoding, out filenameExtension,
                                                  out streamids, out warnings);


                        List<string> mailto = new List<string>();
                        //sendToEmail = "10067er@mail.kuozui.com.tw";
                        //mailto.Add("wenbin3456@gmail.com");
                        //string tt = emp_data[i].Item5;
                        mailto.Add(emp_data[i].Item5);
                        MemoryStream sendStream = new MemoryStream(bytes);
                        PdfReader reader2 = new PdfReader(sendStream);
                        MemoryStream outputsendStream = new MemoryStream();
                        PdfEncryptor.Encrypt(reader2, outputsendStream, true, emp_data[i].Item3, emp_data[i].Item3, PdfWriter.ALLOW_PRINTING);
                        using (MemoryStream dolly = new MemoryStream(outputsendStream.ToArray()))
                        {
                            //utilities.SendMail2("您所申請的文件", "", sendToEmail, mailto, file_name: DateTime.Now.ToString("yyyyMMdd") + "_" + emp_data[i].Item3 + "_" + txt_DATA_YM_search.Text + "_" + ".pdf", attch: dolly);
                            utilities.SendMail2(year + mailTitle, mail_content, sendToEmail, mailto, file_name: DateTime.Now.ToString("yyyyMMdd") + "_" + emp_data[i].Item3 + "_" + txt_DATA_YM_search.Text + "_" + ".pdf", attch: dolly);

                        }
                        showMessage("executeSuccessMessage");
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblock", "$.unblockUI();", true); 
                    }                          

                }

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
    protected void WFB2SK120Down_Click(object sender, EventArgs e)
    {

        List<Tuple<string, string, string, string, string>> emp_data = new List<Tuple<string, string, string, string, string>>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {                
                emp_data.Add(new Tuple<string, string, string, string, string>(gv_result.DataKeys[i].Values["YEAR"].ToString(),
                gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["LICENSE_ID"].ToString(),
                gv_result.DataKeys[i].Values["DEPT_NO"].ToString(),gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString()));
            }
        }
        if (emp_data.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料'); $.unblockUI();", true);
            return;
        }
        else
        {
            string COMPANY_ID = "";
            string TAX_ORG_ID = "";
            string CATEGORY_INCOME  = "";
            string OTHER_ITEM = "";
            string UNIT_NAME = "";
            string UNIT_ADDR = "";
            string UNIT_MEN = "";
            string E183 = "";

            year = txt_DATA_YM_search.Text;
            //取得福利會資料
            DataTable dt_data = service.get_MUTUAL_Data();
            if (dt_data.Rows.Count > 0)
	        {
		        COMPANY_ID = dt_data.Rows[0]["COMPANY_ID"].ToString();
                TAX_ORG_ID = dt_data.Rows[0]["TAX_ORG_ID"].ToString();
                CATEGORY_INCOME = dt_data.Rows[0]["CATEGORY_INCOME"].ToString();
                OTHER_ITEM = dt_data.Rows[0]["OTHER_ITEM"].ToString();
                UNIT_NAME = dt_data.Rows[0]["UNIT_NAME"].ToString();
                UNIT_ADDR = dt_data.Rows[0]["UNIT_ADDR"].ToString();
                UNIT_MEN = dt_data.Rows[0]["UNIT_MEN"].ToString();                 
	        }

            for (int i = 0; i < emp_data.Count; i++)
            {
                E183 = "";
                DataTable dt = new DataTable();                
                dt = service.get_PDF_Data(txt_DATA_YM_search.Text, emp_data);
                dt.TableName = "DataTable1";                

                //是外籍人士 且 超過183天為Y的
                if (dt.Rows[0]["JPN_CD"].ToString() != "" ||  dt.Rows[0]["JPN_CD"].ToString()!= null)
	            {
		            if (dt.Rows[0]["EXCEED_183"].ToString() == "Y")
	                {
		                E183 = "Ｖ";
	                }
	            }
                
                // 建立報表參數陣列變數
                ReportParameter[] para = new ReportParameter[18];
                para[0] = new ReportParameter("DEPT_NO",emp_data[i].Item4 , true);
                para[1] = new ReportParameter("EMP_ID", emp_data[i].Item2, true);
                para[2] = new ReportParameter("EMP_NAME", dt.Rows[0]["EMP_NAME"].ToString(), true);
                para[3] = new ReportParameter("DATA_YM", txt_DATA_YM_search.Text, true);
                para[4] = new ReportParameter("COMPANY_ID", COMPANY_ID, true);
                para[5] = new ReportParameter("TAX_ORG_ID", TAX_ORG_ID, true);
                para[6] = new ReportParameter("CATEGORY_INCOME", CATEGORY_INCOME, true);
                para[7] = new ReportParameter("OTHER_ITEM", OTHER_ITEM, true);
                para[8] = new ReportParameter("UNIT_NAME", UNIT_NAME, true);
                para[9] = new ReportParameter("UNIT_ADDR", UNIT_ADDR, true);
                para[10] = new ReportParameter("UNIT_MEN", UNIT_MEN, true);
                para[11] = new ReportParameter("YEAR_MONTH", "自"+txt_DATA_YM_search.Text+"年1月至" + txt_DATA_YM_search.Text+ "年12月", true);
                para[12] = new ReportParameter("REGISTER_ADDR", dt.Rows[0]["REGISTER_ADDR"].ToString(), true);
                para[13] = new ReportParameter("PAYMENT_AMT", dt.Rows[0]["PAYMENT_AMT"].ToString(), true);
                para[14] = new ReportParameter("E183", E183, true);
                para[15] = new ReportParameter("YEAR", txt_DATA_YM_search.Text, true);
                para[16] = new ReportParameter("LICENSE_ID", emp_data[i].Item3, true);
                para[17] = new ReportParameter("MUTUAL_SEQ", dt.Rows[0]["MUTUAL_SEQ"].ToString(), true);

                ReportViewer reportviewer1 = new ReportViewer();
                //將ReportViewer1的DataSources集合清除
                reportviewer1.LocalReport.DataSources.Clear();
                //將ReportViewer1重置為初始狀態           
                reportviewer1.Reset();
                // 設定 ReportViewer1 的 DataSources
                reportviewer1.LocalReport.Refresh();
                // 給 ReportViewer1 新的設定
                reportviewer1.LocalReport.ReportPath = Server.MapPath("~/report/WFB2SK120.rdlc");
                // 設定 ReportViewer1 的參數, 把值傳過去
                reportviewer1.LocalReport.SetParameters(para);

                reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

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
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=SK1200.{0}", filenameExtension));
                //Response.BinaryWrite(bytes);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
            }

        }
    }
              
}