using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
/// <summary>
/// ExcelHandle 的摘要描述
/// </summary>
public static class ExcelHandle
{
    

    //匯出Excel
    public static void exportExcel(IWorkbook workBook,string fileName)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            workBook.Write(ms);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
            //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
            //君彥的寫法(IE10無法執行)
            //System.Web.HttpContext.Current.Response.ContentType = "application/application/octet-stream"; 
            //改寫
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            System.Web.HttpContext.Current.Response.Buffer = false;
            workBook = null;
            ms.Close();
            ms.Dispose();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            System.Web.HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            
            throw;
        }finally{
             if (workBook != null)
            {
                workBook=null;
            }
        }
    }

    //EXCEL下載(可以直接將伺服器的檔案直接下載,如EXCEL,PDF,jpg,xlsx,docx,mp3,mp4,zip,rar等)
    public static void excel_Down(string filepath, string filename)
    {

        if (File.Exists(filepath))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(filepath);  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8));
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
                throw;
            }

        }
    }
    //資料下載[檔名為UTF8](可以直接將伺服器的檔案直接下載,如EXCEL,PDF,jpg,xlsx,docx,mp3,mp4,zip,rar等)
    public static void file_Download(string filepath, string filename)
    {

        if (File.Exists(filepath))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(filepath);  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8));
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
                throw;
            }

        }
    }
    //EXCEL下載
    public static void txt_DownBIG5(string filepath, string filename)
    {

        if (File.Exists(filepath))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(filepath);  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                //System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}