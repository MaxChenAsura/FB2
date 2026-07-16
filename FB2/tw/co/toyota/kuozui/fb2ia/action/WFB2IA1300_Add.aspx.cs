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
using NPOI.SS.UserModel;

public partial class WebContent_WFB2IA1300_Add : BasePage
{
    private CFB2IA1300BO bo = new CFB2IA1300BO();
    string year = string.Empty;
    string month = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportExcel();
            ViewState["NewPageIndex"] = 0;
            initSet();           
        }


    }
    private void getOrder_By()
    {
        try
        {
            ddl_ORDER_BY.Items.Add(new ListItem("身分證", "LICENSE_ID"));
            ddl_ORDER_BY.Items.Add(new ListItem("工號", "EMP_ID"));  
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void initSet()
    {
        string maxYM = bo.getLast_SALARY_YM();
        if (maxYM.Length == 6)
        {
            string maxY = maxYM.Substring(0, 4);
            string maxM = maxYM.Substring(4, 2);
            txt_DEF_EYM.Text = maxY + "/" + maxM;
            txt_DEF_SYM.Text = ((Convert.ToInt16(maxM) > 2) ? maxY : (Convert.ToInt16(maxY) - 1).ToString())
                                + "/" + ((Convert.ToInt16(maxM) > 2) ? (Convert.ToInt16(maxM) - 2).ToString("00") : (Convert.ToInt16(maxM) + 10).ToString("00"));
        }

        getOrder_By();
    }

    //產生薪調資料
    protected void WFB2IA1300Process_Click(object sender, EventArgs e)
    {
        bool successed = false;
        try
        {
            if (validateDate())
            {
                //檢核是否有已生效資料
                if (bo.get_mon3avgsalry_count(txt_DEF_SYM.Text.Replace("/", ""), txt_DEF_EYM.Text.Replace("/", "")) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('指定之試算月份資料已生效,不允重新計算。')", true);
                }
                else
                {
                    bo.Delete_TB_I_M_LEVEL_CHG_ALL(txt_DEF_SYM.Text.Replace("/", ""));

                    DataTable dt = new DataTable();
                    dt = bo.get_mon3avgsalry_Data(txt_DEF_SYM.Text + "/01", txt_DEF_EYM.Text + "/01");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        successed = bo.Calculate_SALARY_ADJUSTMENT(dt, txt_DEF_SYM.Text.Replace("/", ""), txt_DEF_EYM.Text.Replace("/", ""));

                        if (successed)
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調試算完畢');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!')", true);
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
    private bool validateDate()
    {
        bool isvalid = true;
        string[] s = txt_DEF_SYM.Text.Split('/');
        string[] e = txt_DEF_EYM.Text.Split('/');
        if (s.Length == 2 && e.Length == 2)
        {
            int sYear = Convert.ToInt16(s[0]); //開始年
            int sMonth = Convert.ToInt16(s[1]); //開始月
            int eYear = Convert.ToInt16(e[0]); //結束年
            int eMonth = Convert.ToInt16(e[1]); //結束月

            if (eMonth + (eYear - sYear) * 12 - sMonth != 2)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調月份區間為三個月！');", true);
                isvalid = false;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調月份區間格式有誤！');", true);
            isvalid = false;
        }
        return isvalid;
    }

    //Excel匯出事件
    protected void WFB2IA1300Excel_Click(object sender, EventArgs e)
    {
        try
        {
            if (validateDate())
            {
                CFB2IA1300DAO wfb2ia = new CFB2IA1300DAO();
                wfb2ia.COMPANY_CD = txt_COMPANY_CD.Text;
                wfb2ia.SALARY_SYM = txt_DEF_SYM.Text.Replace("/", "");
                wfb2ia.SALARY_EYM = txt_DEF_EYM.Text.Replace("/", "");
                string excelPath = Server.MapPath("~/ExcelTemplate/FB2IA130_薪調.xlsx");
                IWorkbook workbook = bo.createExcel(wfb2ia, excelPath, "xlsx");
                Session["IA1300_workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2IA1300_Add.aspx?IA1300_FileType = excelDefault";
                Session["IA1300_FileType"] = "excelDefault";
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
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IA1300Excel, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["IA1300_FileType"] != null && Session["IA1300_FileType"].ToString() != "")
            {
                string fileType = Session["IA1300_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["IA1300_workbook"];
                    Session["IA1300_FileType"] = "";
                    Session["IA1300_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2IA1300_1.xlsx");
                }
                else if (fileType == "pdf")
                {
                    string mimeType = Session["IA1300_mimeType"].ToString();
                    string filenameExtension = Session["IA1300_filenameExtension"].ToString();
                    Session["IA1300_mimeType"] = null;
                    Session["IA1300_filenameExtension"] = null;
                    byte[] bytes = (byte[])Session["IA1300_byte"];
                    //將Byte內容寫到Client
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=Salary_Report.{0}", filenameExtension));
                    //Response.BinaryWrite(bytes);
                    Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                    Response.Flush(); // send it to the client to download  
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //PDF 匯出事件
    protected void WFB2IA1300PDF_Click(object sender, EventArgs e)
    {
        byte[] bytes = null;
        string st = "";
        string st1 = "";
        string st2 = "";
        if (validateDate())
        {
            DataTable dt = new DataTable();
            dt = bo.get_PDF_Data(txt_DEF_SYM.Text.Replace("/", ""), txt_DEF_EYM.Text.Replace("/", ""), txt_CLASSQTY.Text, txt_COMPANY_CD.Text, ddl_ORDER_BY.SelectedValue);
            year = txt_DEF_EYM.Text.Substring(0, 4);
            month = txt_DEF_EYM.Text.Substring(5, 2);

            if (Convert.ToInt32(month) > 10)
            {
                st1 = Convert.ToString(Convert.ToInt32(year) + 1);
                st2 = Convert.ToString(Convert.ToInt32(month) - 10);
            }
            else
            {
                st1 = year;
                st2 = Convert.ToString(Convert.ToInt32(month) + 2);
            }
            //string st1 = (Convert.ToInt32(month) > 10) ? (Convert.ToInt32(year) + 1).ToString("yyyy") : year;


            //string st2 = (Convert.ToInt32(month) > 10) ? (Convert.ToInt32(month) + 2).ToString("00") : (Convert.ToInt32(month) + 2).ToString("00");

            //string ym = (Convert.ToInt16(month) > 10) ? (Convert.ToInt32(year)+1).ToString() :year
            //                    + "/" + ((Convert.ToInt16(month) > 10) ? (Convert.ToInt16(month) +2).ToString("00") : (Convert.ToInt16(month) + 2).ToString("00"));

            string ym = st1 + st2;
            DataTable dt1 = bo.get_Company(txt_COMPANY_CD.Text);
            if (dt1.Rows.Count > 0)
            {
                st = dt1.Rows[0]["COMPANY_NAME"].ToString();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                // 建立報表參數陣列變數
                ReportParameter[] para = new ReportParameter[7];
                para[0] = new ReportParameter("COMPANY_CD", txt_COMPANY_CD.Text, true);
                para[1] = new ReportParameter("COMPANY_SNAME", st, true);
                para[2] = new ReportParameter("DEF_SYM", txt_DEF_SYM.Text.Replace("/", ""), true);
                para[3] = new ReportParameter("DEF_EYM", txt_DEF_EYM.Text.Replace("/", ""), true);
                para[4] = new ReportParameter("HEALTH_ORG_ID", txt_HEALTH_ORG_ID.Text, true);
                para[5] = new ReportParameter("CLASSQTY", txt_CLASSQTY.Text, true);
                para[6] = new ReportParameter("DEF_EYM_ADD2", ym, true);


                ReportViewer reportviewer1 = new ReportViewer();
                //將ReportViewer1的DataSources集合清除
                reportviewer1.LocalReport.DataSources.Clear();
                //將ReportViewer1重置為初始狀態           
                reportviewer1.Reset();
                reportviewer1.LocalReport.Refresh();
                // 給 ReportViewer1 新的設定
                reportviewer1.LocalReport.ReportPath = "report/WFB2IA1300PDF.rdlc";
                // 設定 ReportViewer1 的參數, 把值傳過去
                reportviewer1.LocalReport.SetParameters(para);
                // 設定 ReportViewer1 的 DataSources
                reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsIA1300", dt));

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension = "PDF";
                bytes = reportviewer1.LocalReport.Render(
                                          "PDF", null, out mimeType, out encoding, out filenameExtension,
                                          out streamids, out warnings);

                Session["IA1300_mimeType"] = mimeType;
                Session["IA1300_filenameExtension"] = filenameExtension;

                Session["IA1300_byte"] = bytes;
                dwnframe.Attributes["src"] = "WFB2IA1300_Add.aspx?IA1300_FileType = pdf";
                Session["IA1300_FileType"] = "pdf";
                if (bytes != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }

                ////將Byte內容寫到Client
                //Response.Buffer = true;
                //Response.Clear();
                //Response.ContentType = mimeType;
                //Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=Salary_Report.{0}", filenameExtension));
                ////Response.BinaryWrite(bytes);
                //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                //Response.Flush(); // send it to the client to download  
                //Response.End();
            }
            else
                ScriptManager.RegisterClientScriptBlock(WFB2IA1300PDF, this.GetType(), "error", "alert('查無資料!');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(WFB2IA1300PDF, this.GetType(), "error", "alert('查無資料!');", true);
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["IA1300_Is_Search"] = "Y";
        Response.Redirect("WFB2IA1300_Qry.aspx");
    }
}