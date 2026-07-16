using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sk_WFB2SK0100_Qry : BasePage
{
    CFB2SK0100BO service = new CFB2SK0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.exportTXT();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    protected void WFB2SK0100TxtDown_Click(object sender, EventArgs e)
    {
        CFB2SK0100DAO fb2sk = new CFB2SK0100DAO();
        string DATA_YM = txt_DATA_YM_S.Text.Replace("/", "");
        DataTable dt2 = service.Action(fb2sk, DATA_YM);
        if (dt2.Rows.Count == 0)
        {
            showMessage("noDownDataMessage");
            return;
        }
        else if (dt2.Rows.Count > 0)
        {
            MemoryStream fileStream = service.Download(dt2);
            Session["fileStream_SK010"] = fileStream;
            dwnframe.Attributes["src"] = "WFB2SK0100_Qry.aspx?FileType_SK010 = sk010txt";
            Session["FileType_SK010"] = "sk010txt";
            if (fileStream != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
            }
        }
        
        else if (dt2 ==null)
        {
            
            showMessage("downFailMessage");
            return;
        }

    }

    public void exportTXT()
    {
        try
        {
            if (Session["FileType_SK010"] != null && Session["FileType_SK010"].ToString() != "")
            {
                string FileType_SK010 = Session["FileType_SK010"].ToString();
                if (FileType_SK010 == "sk010txt")
                {
                    MemoryStream fileStream = (MemoryStream)Session["fileStream_SK010"];
                    Session["FileType_SK010"] = "";
                    Session["fileStream_SK010"] = null;

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("DB3KFUMN.txt"));
                    System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    fileStream.Close();
                    fileStream.Dispose();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}