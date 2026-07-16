using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Preview_File : System.Web.UI.Page
{
    CFB2SJ0410BO sj0410BO = new CFB2SJ0410BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["FILE_EMP_ID"] != null && Request.QueryString["ASSESS_YEAR"] != null && Request.QueryString["ASSESS_TYPE"] != null)
        {
            string serverFilePath = sj0410BO.getFilePath() + "\\" + Request.QueryString["ASSESS_YEAR"] + Request.QueryString["ASSESS_TYPE"] + "\\" + Request.QueryString["ASSESS_YEAR"] + Request.QueryString["ASSESS_TYPE"] + Request.QueryString["FILE_EMP_ID"] + ".pdf";
           
            WebClient User = new WebClient();
           
            string filePath = "";
            string fileType = "";
            if (fileType.ToUpper() == "PDF")
            {
                Byte[] FileBuffer = null;
                try
                {
                    FileBuffer = User.DownloadData(serverFilePath);
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('實體檔案不存在!!');close();", true);
                }
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/" + fileType;
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileData.Rows[0]["OLD_FILE_NAME"].ToString(), System.Text.Encoding.UTF8));
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
        }
    }
}