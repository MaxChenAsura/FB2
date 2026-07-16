using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HB0700_Upload : BasePage
{
    //Service 物件
    private CFB2HB0700BO service = new CFB2HB0700BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_HB0700"] != null && Session["FileType_HB0700"].ToString() != "")
            {
                string FileType_SG0201 = Session["FileType_HB0700"].ToString();
                if (FileType_SG0201 == "excel")
                {
                    Session["FileType_HB0700"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB070_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2HB0701Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                if (filename != ".xlsx" && filename != ".xls")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                IWorkbook workbook = service.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB070_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2HB0700_Upload.aspx?FileType_HB0700=excel";
                    Session["FileType_HB0700"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }

            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2HB070_Upload.xlsx")))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2HB070_Upload.xlsx"));  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2HB070_Upload.xlsx", System.Text.Encoding.UTF8));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();

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
        Session["HB0700_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0700_Qry.aspx");
    }
}