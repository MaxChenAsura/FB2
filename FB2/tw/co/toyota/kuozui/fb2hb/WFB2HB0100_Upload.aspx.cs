using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2hb_WFB2HB0100_Upload : BasePage
{
    //Service 物件
    private CFB2HB0100BO service = new CFB2HB0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.exportExcel();
        }
    }
    protected void WFB2HB0101ExcelImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {

                IWorkbook workbook = service.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));
                //Session["HB0100_workbook"] = workbook;
                //dwnframe.Attributes["src"] = "WFB2HB0100_Upload.aspx?HB0100_FileType=excel";
                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB010_error_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                if (workbook == null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                }
                else
                {
                    #region 存在SERVER取代SESSION

                    FileStream file = new FileStream(@toPath, FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion
                    //Session["workbook_SI010"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2HB0100_Upload.aspx?HB0100_FileType=excel";
                    Session["HB0100_FileType"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }

                /*
                if (workbook != null)
                {

                    Session["HB0100_FileType"] = "excel";
                    //exportExcel("考核查詢資料.xlsx");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳失敗');</script>");
                }
                else
                {
                    Session["FileType_HB0100"] = "";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳成功');</script>");
                }
                */
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
            if (Session["HB0100_FileType"] != null && Session["HB0100_FileType"].ToString() != "")
            {
                string fileType = Session["HB0100_FileType"].ToString();
                if (fileType == "excel")
                {
                    Session["HB0100_FileType"] = "";
                    //ExcelHandle.exportExcel(workBook, "FB2HB0100_error.xlsx");
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0101ExcelDown_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2HB010.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2HB010.xlsx"), "WFB2HB010.xlsx");
                /*
                FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2HB010.xlsx"));  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2HB010.xlsx", System.Text.Encoding.UTF8));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();
                */
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

            }

        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["HB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0100_Qry.aspx");
    }
}