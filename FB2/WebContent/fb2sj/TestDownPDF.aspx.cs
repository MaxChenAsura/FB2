using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class WebContent_fb2sj_TestDownPDF : System.Web.UI.Page
{
    CFB2SJ0410BO sj0410BO = new CFB2SJ0410BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["EMP_ID"] != null && Request.QueryString["ASSESS_YEAR"] != null && Request.QueryString["ASSESS_TYPE"] != null)
        {
            try
            {
           
            String fileName=Request.QueryString["EMP_ID"]+".pdf";
            if (Request.QueryString["FILE_NAME"] != null)
            {
                fileName = Request.QueryString["FILE_NAME"] + ".pdf";
            }
          
                string serverFilePath = sj0410BO.getFilePath() + "\\" + Request.QueryString["ASSESS_YEAR"] + Request.QueryString["ASSESS_TYPE"] + "\\" + Request.QueryString["ASSESS_YEAR"] + Request.QueryString["ASSESS_TYPE"] + Request.QueryString["EMP_ID"] + ".pdf";
                //string serverFilePath = "D:\\FB2_ASSESS\\20211\\2021128751.pdf";
                Stream FileStream;
                FileStream = File.OpenRead(serverFilePath);

                Byte[] Buf = new byte[FileStream.Length];
                FileStream.Read(Buf, 0, int.Parse(FileStream.Length.ToString()));
                FileStream.Close();

                //準備下載檔案 
                Response.ClearHeaders();
                Response.Clear();
                Response.Expires = 0;
                Response.Buffer = false;
                Response.ContentType = "Application/pdf";
                Response.Charset = "utf-8";
                //透過Header設定檔名 
                Response.AddHeader("Content-Disposition", "Attachment; filename="+fileName);
                Response.BinaryWrite(Buf);
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }
        }
    }
    protected void PDFDown_Click(object sender, EventArgs e)
    {
        try
        {
            string serverFilePath = "D:\\FB2_ASSESS\\20211\\2021128751.pdf";
            Stream FileStream;
            FileStream = File.OpenRead(serverFilePath);

            Byte[] Buf = new byte[FileStream.Length];
            FileStream.Read(Buf, 0, int.Parse(FileStream.Length.ToString()));
            FileStream.Close();

            //準備下載檔案 
            Response.ClearHeaders();
            Response.Clear();
            Response.Expires = 0;
            Response.Buffer = false;
            Response.ContentType = "Application/pdf";
            Response.Charset = "utf-8";
            //透過Header設定檔名 
            Response.AddHeader("Content-Disposition", "Attachment; filename=123.pdf");
            Response.BinaryWrite(Buf);
            //Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}