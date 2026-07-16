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

public partial class WebContent_WFB2SC5500_Qry : BasePage
{
    private CFB2SC5500BO bo = new CFB2SC5500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
            txt_SALARY_DT.Text = DateTime.Now.ToString("yyyy/MM") ;
        }


    }




    protected void WFB2SC5500PDF_Click(object sender, EventArgs e)
    {
        
            DataTable dt = new DataTable();

            //int day = DateTime.DaysInMonth(Convert.ToInt32(txt_SALARY_DT.Text.Replace("/", "").Substring(0, 4)), Convert.ToInt32(txt_SALARY_DT.Text.Replace("/", "").Substring(4, 2)));
            //string SALARY_DT_S = string.Format("{0}/01", txt_SALARY_DT.Text);
            //string SALARY_DT_E = string.Format("{0}/{1}", txt_SALARY_DT.Text, day);
            dt = bo.get_PDF_Data(txt_SALARY_DT.Text.Replace("/", ""));
            if (dt.Rows.Count > 0)
            {
                Int32 STD;
                STD = Convert.ToInt32(txt_SALARY_DT.Text.Replace("/", "")) - 191100;
                // 建立報表參數陣列變數
                ReportParameter[] para = new ReportParameter[3];
                para[0] = new ReportParameter("datatime", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), true);
                para[1] = new ReportParameter("SALARY_DT_Y", STD.ToString().Substring(0, 3), true);
                para[2] = new ReportParameter("SALARY_DT_M", STD.ToString().Substring(3), true);
                //para[2] = new ReportParameter("DEF_SYM", txt_DEF_SYM.Text.Replace("/", ""), true);
                //para[3] = new ReportParameter("DEF_EYM", txt_DEF_EYM.Text.Replace("/", ""), true);
                //para[4] = new ReportParameter("HEALTH_ORG_ID", txt_HEALTH_ORG_ID.Text, true);
                //para[5] = new ReportParameter("CLASSQTY", txt_CLASSQTY.Text, true);

                ReportViewer reportviewer1 = new ReportViewer();
                //將ReportViewer1的DataSources集合清除
                reportviewer1.LocalReport.DataSources.Clear();
                //將ReportViewer1重置為初始狀態           
                reportviewer1.Reset();
                reportviewer1.LocalReport.Refresh();
                // 給 ReportViewer1 新的設定
                reportviewer1.LocalReport.ReportPath = "report/WFB2SC5500PDF.rdlc";
                // 設定 ReportViewer1 的參數, 把值傳過去
                reportviewer1.LocalReport.SetParameters(para);
                // 設定 ReportViewer1 的 DataSources
                reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dssc550", dt));

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
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=SC5500.{0}", filenameExtension));
                //Response.BinaryWrite(bytes);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
            }
            else
            {
                showMessage("QryNotFoundMessage");
            }
    }
}