using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;


public partial class WebContent_fb2hb_WFB2SG0200_Upload : BasePage
{
    //Service 物件
    private CFB2HA0200BO ha020BO = new CFB2HA0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }

    //返回
    protected void btn_back_Click(object sender, EventArgs e)
    {
        string parentFuncID = hid_parentFuncID.Value;
        Session["HA0200_Is_Search"] = "Y";
        Response.Redirect("WFB2HA0200_Qry.aspx?parentFuncId=" + parentFuncID);
    }


    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_HA0201"] != null && Session["FileType_HA0201"].ToString() != "")
            {
                string FileType_HA0201 = Session["FileType_HA0201"].ToString();
                if (FileType_HA0201 == "excel")
                {
                    Session["FileType_HA0201"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HA020_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
               
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //匯入
    protected void WFB2HA0201ExcelImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                Stream fs = FileUpload1.FileContent;
                string FileType_HA0201 = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                IWorkbook workbook = ha020BO.uploadExcel(fs, FileType_HA0201);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HA020_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2HA0200_Upload.aspx?FileType_HA0201=excel";
                    Session["FileType_HA0201"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);

        }
    }


    //EXCEL匯出(範例下載)
    protected void WFB2HA0201ExcelDown_Click(object sender, EventArgs e)
    {
        FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2HA020_Upload.xlsx"));

        if (xpath_file.Exists)
        {
            try
            {
                // FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2SG_OneTime.xlsx"));  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2HA020_Upload.xlsx", System.Text.Encoding.UTF8));
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
   
}