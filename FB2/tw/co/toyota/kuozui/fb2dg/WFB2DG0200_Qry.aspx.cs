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
public partial class WebContent_fb2dg_WFB2DG0200_Qry : BasePage
{
    //Service 物件
    private CFB2DG020BO bo = new CFB2DG020BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportExcel();
            ViewState["NewPageIndex"] = 0;

        }
        if (ddl_PAG_TYPE.SelectedValue == "4")
        {
            Tie.Visible = true;

        }
        else
        {
            Tie.Visible = false;
        }


    }




    protected void WFB2DG020ExcelExport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        bool IsExport = false;
        byte[] bytes = null;

        switch (ddl_PAG_TYPE.SelectedValue)//拿什麼東西來做判斷
        {
            case "1":
                CFB2DG020DAO wfb2dg = new CFB2DG020DAO();
                bytes = bo.createExcel(wfb2dg, "xlsx");
                if (bytes == null)
                    showMessage("QryNotFoundMessage");
                else
                    IsExport = true;
                break;
            case "2":
                dt = bo.get_PDF_Data2();
                if (dt.Rows.Count <= 0)
                    showMessage("QryNotFoundMessage");
                else
                    IsExport = true;
                break;
            case "3":
                dt = bo.get_PDF_Data3();
                if (dt.Rows.Count <= 0)
                    showMessage("QryNotFoundMessage");
                else
                    IsExport = true;
                break;
            case "4":
                dt = bo.get_PDF_Data4(txt_UPDATED_DT_S.Text, txt_UPDATED_DT_E.Text);
                if (dt.Rows.Count <= 0)
                    showMessage("QryNotFoundMessage");
                else
                    IsExport = true;
                break;
        }
        if (IsExport)
        {

            // 建立報表參數陣列變數
            ReportParameter[] para = new ReportParameter[0];
            //para[0] = new ReportParameter("datatime", DateTime.Now.ToString("yyyy/MM/dd hh:mm"), true);
            //para[1] = new ReportParameter("SALARY_DT_Y", STD.ToString().Substring(0, 3), true);
            //para[2] = new ReportParameter("SALARY_DT_M", STD.ToString().Substring(3), true);
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
            switch (ddl_PAG_TYPE.SelectedValue)//拿什麼東西來做判斷
            {
                case "1":
                    break;
                case "2":
                    reportviewer1.LocalReport.ReportPath = "report/WFB2DG020Excel_2.rdlc";
                    reportviewer1.LocalReport.SetParameters(para);
                    reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsDG020_2", dt));
                    break;
                case "3":
                    reportviewer1.LocalReport.ReportPath = "report/WFB2DG020Excel_3.rdlc";
                    reportviewer1.LocalReport.SetParameters(para);
                    reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsDG020_3", dt));
                    break;
                case "4":
                    reportviewer1.LocalReport.ReportPath = "report/WFB2DG020Excel_4.rdlc";
                    reportviewer1.LocalReport.SetParameters(para);
                    reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsDG020_4", dt));
                    break;
            }
            // 給 ReportViewer1 新的設定
            if (ddl_PAG_TYPE.SelectedValue != "1")
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                bytes = reportviewer1.LocalReport.Render(
                                          "Excel", null, out mimeType, out encoding, out filenameExtension,
                                          out streamids, out warnings);

                Session["DG0200_mimeType"] = mimeType;
                Session["DG0200_filenameExtension"] = filenameExtension;
            }
            Session["DG0200_byte"] = bytes;
            dwnframe.Attributes["src"] = "WFB2DG0200_Qry.aspx?DG0200_FileType = excelDefault";
            Session["DG0200_FileType"] = "excelDefault";
            Session["DG0200_ddl_PAG_TYPE"] = ddl_PAG_TYPE.SelectedValue;
            if (bytes != null)
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
    public void exportExcel()
    {
        try
        {
            if (Session["DG0200_FileType"] != null && Session["DG0200_FileType"].ToString() != "")
            {
                string pag_type = Session["DG0200_ddl_PAG_TYPE"].ToString(); //報表類型
                string fileType = Session["DG0200_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    byte[] bytes = (byte[])Session["DG0200_byte"];
                    Session["DG0200_FileType"] = "";
                    Session["DG0200_byte"] = null;
                    Session["DG0200_ddl_PAG_TYPE"] = null;
                    if (pag_type == "1")
                    {
                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.ClearHeaders();
                        System.Web.HttpContext.Current.Response.ClearContent();
                        System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("FB2DG020_" + pag_type + ".xlsx"));
                        System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                        System.Web.HttpContext.Current.Response.Buffer = false;
                        System.Web.HttpContext.Current.Response.End();
                    }
                    else
                    {
                        string mimeType = Session["DG0200_mimeType"].ToString();
                        string filenameExtension = Session["DG0200_filenameExtension"].ToString();
                        Session["DG0200_mimeType"] = null;
                        Session["DG0200_filenameExtension"] = null;
                        //將Byte內容寫到Client
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={1}.{0}", filenameExtension, HttpUtility.UrlEncode("FB2DG0200_" + pag_type, System.Text.Encoding.UTF8)));        //Response.BinaryWrite(bytes);
                        Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                        Response.Flush(); // send it to the client to download  
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}


