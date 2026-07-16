using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sk_WFB2SK0200_Qry : BasePage
{
    CFB2SK0200BO service = new CFB2SK0200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.exportTXT();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    protected void WFB2SK0200TxtDown_Click(object sender, EventArgs e)
    {
        CFB2SK0200DAO fb2sk = new CFB2SK0200DAO();
        //從View新增資料至福利會用人事主檔
        //service.insertData(fb2sk); //舊寫法(效率差)
        
        service.insertData();//新寫法

        DataTable dt2 = service.getData(fb2sk); 
        if (dt2.Rows.Count == 0)
        {
            showMessage("noDownDataMessage");
            return;
        }
        else if (dt2.Rows.Count > 0)
        {
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SK020_" + SessionHandle.Current.emp_id + ".txt"));
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SK020_" + SessionHandle.Current.emp_id + ".txt");
            service.Download(dt2, toPath);
            //Session["fileStream_SK020"] = fileStream;
            dwnframe.Attributes["src"] = "WFB2SK0200_Qry.aspx?FileType_SK020 = sk020txt";
            Session["FileType_SK020"] = "sk020txt";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
        }

        else if (dt2 == null)
        {

            showMessage("downFailMessage");
            return;
        }
    }
    public void exportTXTtoDownloadFile(MemoryStream fileStream)
    {
        try
        {
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SK020_" + SessionHandle.Current.emp_id + ".txt"));

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("DB3KFUPR.txt"));
            System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
            System.Web.HttpContext.Current.Response.Buffer = false;
            fileStream.Close();
            fileStream.Dispose();
            System.Web.HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }


    public void exportTXT()
    {
        try
        {
            if (Session["FileType_SK020"] != null && Session["FileType_SK020"].ToString() != "")
            {
                string fileType = Session["FileType_SK020"].ToString();
                if (fileType == "sk020txt")
                {
                    //MemoryStream fileStream = (MemoryStream)Session["fileStream_SK020"];
                    //Session["fileStream_SK020"] = null;

                    Session["FileType_SK020"] = "";
                    ExcelHandle.txt_DownBIG5(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SK020_" + SessionHandle.Current.emp_id + ".txt"), "DB3KFUPR.txt");

                    //System.Web.HttpContext.Current.Response.Clear();
                    //System.Web.HttpContext.Current.Response.ClearHeaders();
                    //System.Web.HttpContext.Current.Response.ClearContent();
                    //System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("DB3KFUPR.txt"));
                    //System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    //System.Web.HttpContext.Current.Response.Buffer = false;
                    //fileStream.Close();
                    //fileStream.Dispose();
                    //System.Web.HttpContext.Current.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }



}