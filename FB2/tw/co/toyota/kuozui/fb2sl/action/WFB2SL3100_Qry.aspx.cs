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

public partial class WebContent_fb2sl_WFB2SL3100_Qry : BasePage
{
    private CFB2SL3100BO service = new CFB2SL3100BO();
    public static string year = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
            getWS_CD();//職種
            getEMP_STATUS();//狀態

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
    private void getEMP_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("EMP_STATUS", "", "");
            ddl_EMP_STATUS.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2SL3100BO service = new CFB2SL3100BO();
            //dt = service.getEmpName(txt_EMP_ID.Text, rbl_BORROW_TYPE.SelectedValue);
            dt = service.getEmpName(txt_EMP_ID.Text, "1");
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
    private void getWS_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("PJOB_CD,EMP_ID");
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "DEPT_NO", "EMP_ID", "EMP_NAME", "SALARY_EMAIL", "TAX_FORMAT" }; //設定GridView Key
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SL310Generate, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "DEPT_NO", "EMP_ID", "EMP_NAME", "SALARY_EMAIL" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "DEPT_NO", "EMP_ID", "EMP_NAME", "SALARY_EMAIL" }; //設定GridView Key
    }


    protected void WFB2SL310Generate_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PJOB_CD,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PJOB_CD,EMP_ID", 0, 10);
            //end
            CFB2SL3100DAO dao = new CFB2SL3100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DATA_YM_search.Text
                                         , txt_DEPT_NO.Text, ddl_WS_CD_search.SelectedValue, txt_EMP_ID.Text, txt_LICENSE_ID_search.Text, ddl_EMP_STATUS.SelectedValue);
            if (dataCount == 0)
            {
                WFB2SL310Generate.Visible = true;
                btn_clear.Visible = true;
                WFB2SL310Go.Visible = false;
                WFB2SL310Down.Visible = false;
                gv_result.Visible = false;
                OnePage.Visible = false;
                gv_result.ShowFooter = false;
                showMessage("QryNotFoundMessage");
            }
            else
            {
                WFB2SL310Generate.Visible = true;
                btn_clear.Visible = true;
                WFB2SL310Go.Visible = true;
                gv_result.Visible = true;
                OnePage.Visible = true;
                gv_result.ShowFooter = false;
                WFB2SL310Down.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //寄送電子郵件
    protected void WFB2SL310Go_Click(object sender, EventArgs e)
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
            List<Tuple<string, string, string, string, string, string>> emp_data = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    if (gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該員工無Email可寄送'); $.unblockUI();", true);
                        return;
                    }
                    emp_data.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["LICENSE_ID"].ToString(),
                        gv_result.DataKeys[i].Values["DEPT_NO"].ToString(), gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        gv_result.DataKeys[i].Values["EMP_NAME"].ToString(), gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString()
                        , gv_result.DataKeys[i].Values["TAX_FORMAT"].ToString()
                        ));
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
                    sendToEmail = dtSendTo.Rows[0]["CODE_VAL1"].ToString();
                
                year = txt_DATA_YM_search.Text;
                DataTable dt_Content = service.getEmailContent();
                if (dt_Content.Rows.Count > 0)
                {
                    mail_content = dt_Content.Rows[0]["REMARK"].ToString();
                    mailTitle = dt_Content.Rows[0]["CODE_VAL1"].ToString();
                }
                for (int i = 0; i < emp_data.Count; i++)
                {
                    if (emp_data[i].Item5 != "")
                    {

                        DataTable dt = new DataTable();
                        DataTable dt2 = new DataTable();
                        dt = service.get_PDF_Data(txt_DATA_YM_search.Text, emp_data);
                        dt.TableName = "DataTable3";
                        dt2 = service.get_PDF_Data2(txt_DATA_YM_search.Text, txt_DATA_YM_search.Text + "/01/01", txt_DATA_YM_search.Text + "/12/31", emp_data, txt_DATA_YM_search.Text + "01", txt_DATA_YM_search.Text + "12");

                        //dt2 = service.get_PDF_Data2(txt_DATA_YM_search.Text, txt_DATA_YM_search.Text + "/01/01", txt_DATA_YM_search.Text + "/12/31", emp_data);
                        dt2.TableName = "DataTable4";

                        //dt = service.get_PDF_Data(txt_DATA_YM_search.Text, txt_LICENSE_ID_search.Text);

                        // 建立報表參數陣列變數
                        ReportParameter[] para = new ReportParameter[4];
                        para[0] = new ReportParameter("DEPT_NO", emp_data[i].Item2, true);
                        para[1] = new ReportParameter("EMP_ID", emp_data[i].Item3, true);
                        para[2] = new ReportParameter("EMP_NAME", emp_data[i].Item4, true);
                        para[3] = new ReportParameter("DATA_YM", txt_DATA_YM_search.Text, true);

                        ReportViewer reportviewer1 = new ReportViewer();
                        //將ReportViewer1的DataSources集合清除
                        reportviewer1.LocalReport.DataSources.Clear();
                        //將ReportViewer1重置為初始狀態           
                        reportviewer1.Reset();
                        // 設定 ReportViewer1 的 DataSources
                        reportviewer1.LocalReport.Refresh();
                        // 給 ReportViewer1 新的設定
                        reportviewer1.LocalReport.ReportPath = Server.MapPath("~/report/WFB2SL310.rdlc");
                        // 設定 ReportViewer1 的參數, 把值傳過去
                        reportviewer1.LocalReport.SetParameters(para);
                        
                        reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dt));
                        reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet4", dt2));

                        Warning[] warnings;
                        string[] streamids;
                        string mimeType;
                        string encoding;
                        string filenameExtension;
                        byte[] bytes = reportviewer1.LocalReport.Render(
                                                  "PDF", null, out mimeType, out encoding, out filenameExtension,
                                                  out streamids, out warnings);

                        List<string> mailto = new List<string>();
                        //sendToEmail = "14939kevinliu@mail.kuozui.com.tw";
                        //mailto.Add("jenercatlin@yahoo.com.tw");
                        //string tt = emp_data[i].Item5;
                        mailto.Add(emp_data[i].Item5);
                        MemoryStream sendStream = new MemoryStream(bytes);
                        PdfReader reader2 = new PdfReader(sendStream);
                        MemoryStream outputsendStream = new MemoryStream();
                        PdfEncryptor.Encrypt(reader2, outputsendStream, true, emp_data[i].Item1, emp_data[i].Item1, PdfWriter.ALLOW_PRINTING);
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

    //下載PDF
    protected void WFB2SL310Down_Click(object sender, EventArgs e)
    {

        List<Tuple<string, string, string, string, string, string>> emp_data = new List<Tuple<string, string, string, string, string, string>>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                emp_data.Add(new Tuple<string, string, string, string, string, string>(
                gv_result.DataKeys[i].Values["LICENSE_ID"].ToString(),
                gv_result.DataKeys[i].Values["DEPT_NO"].ToString(), gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                gv_result.DataKeys[i].Values["EMP_NAME"].ToString(), gv_result.DataKeys[i].Values["SALARY_EMAIL"].ToString()
                , gv_result.DataKeys[i].Values["TAX_FORMAT"].ToString()
                ));
            }
        }
      
        if (emp_data.Count() != 1)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料'); $.unblockUI();", true);
            return;
        }
        else
        {
            year = txt_DATA_YM_search.Text;
            for (int i = 0; i < emp_data.Count; i++)
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = service.get_PDF_Data(txt_DATA_YM_search.Text, emp_data);
                dt.TableName = "DataTable3";
                dt2 = service.get_PDF_Data2(txt_DATA_YM_search.Text, txt_DATA_YM_search.Text + "/01/01", txt_DATA_YM_search.Text + "/12/31", emp_data, txt_DATA_YM_search.Text + "01", txt_DATA_YM_search.Text + "12");
                dt2.TableName = "DataTable4";

                //dt = service.get_PDF_Data(txt_DATA_YM_search.Text, txt_LICENSE_ID_search.Text);

                // 建立報表參數陣列變數
                ReportParameter[] para = new ReportParameter[4];
                para[0] = new ReportParameter("DEPT_NO", emp_data[i].Item2, true);
                para[1] = new ReportParameter("EMP_ID", emp_data[i].Item3, true);
                para[2] = new ReportParameter("EMP_NAME", emp_data[i].Item4, true);
                para[3] = new ReportParameter("DATA_YM", txt_DATA_YM_search.Text, true);

                ReportViewer reportviewer1 = new ReportViewer();
                //將ReportViewer1的DataSources集合清除
                reportviewer1.LocalReport.DataSources.Clear();
                //將ReportViewer1重置為初始狀態           
                reportviewer1.Reset();
                // 設定 ReportViewer1 的 DataSources
                reportviewer1.LocalReport.Refresh();
                // 給 ReportViewer1 新的設定
                reportviewer1.LocalReport.ReportPath = Server.MapPath("~/report/WFB2SL310.rdlc");
                // 設定 ReportViewer1 的參數, 把值傳過去
                reportviewer1.LocalReport.SetParameters(para);

                reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dt));
                reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet4", dt2));

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
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=SL3100.{0}", filenameExtension));
                //Response.BinaryWrite(bytes);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();                
            }

        }       
    }

    
              
}