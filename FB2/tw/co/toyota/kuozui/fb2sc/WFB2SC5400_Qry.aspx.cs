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
public partial class WebContent_fb2sc_WFB2SC5400_Qry : BasePage
{
    //Service 物件
    private CFB2SC5400BO bo = new CFB2SC5400BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportPDF();
            txt_SALARY_YM.Text = DateTime.Now.ToString("yyyy/MM");
            ViewState["NewPageIndex"] = 0;

        }


    }




    protected void WFB2SC5400ExcelExport_Click(object sender, EventArgs e)
    {
        int day = DateTime.DaysInMonth(Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "").Substring(0, 4)), Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "").Substring(4, 2)));
        string SALARY_DT_S = string.Format("{0}/01", txt_SALARY_YM.Text);
        string SALARY_DT_E = string.Format("{0}/{1}", txt_SALARY_YM.Text, day);

        DataTable dt = new DataTable();
        string salary_ym = txt_SALARY_YM.Text.Replace("/", "");
        dt = bo.get_PDF_Data(salary_ym, txt_DEPT_NO.Text, txt_EMP_ID.Text, SALARY_DT_S, SALARY_DT_E);
        if(dt.Rows.Count ==0)
        {
            showMessage("noDownDataMessage");
                return;
        }
        else
        {

            Int32 STD;
            STD = Convert.ToInt32(salary_ym) - 191100;
            // 建立報表參數陣列變數
            ReportParameter[] para = new ReportParameter[4];
            para[0] = new ReportParameter("datatime", DateTime.Now.ToString("yyyy/MM/dd HH:mm "), true);
            para[1] = new ReportParameter("SALARY_DT_Y", STD.ToString().Substring(0, 3), true);
            para[2] = new ReportParameter("SALARY_DT_M", STD.ToString().Substring(3), true);
            para[3] = new ReportParameter("count_EMP_ID", dt.Rows.Count.ToString(), true);
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
            if (salary_ym.Substring(4, 2) == "12")
            {
                reportviewer1.LocalReport.ReportPath = "report/WFB2SC5400Excel_TAX.rdlc";
            }
            else
            {
                reportviewer1.LocalReport.ReportPath = "report/WFB2SC5400Excel.rdlc";

            }


            // 設定 ReportViewer1 的參數, 把值傳過去
            reportviewer1.LocalReport.SetParameters(para);
            // 設定 ReportViewer1 的 DataSources
            reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsSC5400", dt));

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            byte[] fileByte = reportviewer1.LocalReport.Render(
                                      "Excel", null, out mimeType, out encoding, out filenameExtension,
                                      out streamids, out warnings);
            
            Session["SC5400_fileByte"] = fileByte;
            dwnframe.Attributes["src"] = "WFB2SC5400_Qry.aspx?SC5400_FileType = pdfDefault";
            Session["SC5400_FileType"] = "pdfDefault";
            Session["SC5400_mimeType"] = mimeType;
            Session["SC5400_filenameExtension"] = filenameExtension;
            if (fileByte != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
            ////將Byte內容寫到Client
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={1}.{0}", filenameExtension, HttpUtility.UrlEncode("FB2SC540", System.Text.Encoding.UTF8)));        //Response.BinaryWrite(bytes);
            //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            //Response.Flush(); // send it to the client to download  
            //Response.End();
        }
    }
    public void exportPDF()
    {
        try
        {
            if (Session["SC5400_FileType"] != null && Session["SC5400_FileType"].ToString() != "")
            {
                string fileType = Session["SC5400_FileType"].ToString();
                if (fileType == "pdfDefault")
                {
                    byte[] fileByte = (byte[])Session["SC5400_fileByte"];
                    Session["SC5400_FileType"] = "";
                    Session["SC5400_fileByte"] = null;
                    string mimeType = Session["SC5400_mimeType"].ToString();
                    string filenameExtension = Session["SC5400_filenameExtension"].ToString();
                    //將Byte內容寫到Client
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={1}.{0}", filenameExtension, HttpUtility.UrlEncode("FB2SC540_1", System.Text.Encoding.UTF8)));        //Response.BinaryWrite(bytes);
                    Response.OutputStream.Write(fileByte, 0, fileByte.Length); // create the file  
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
}


