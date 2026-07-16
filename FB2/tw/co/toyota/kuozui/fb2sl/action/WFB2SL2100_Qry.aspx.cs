using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sl_WFB2SL2100_Qry : BasePage
{
    CFB2SL2100BO service = new CFB2SL2100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            this.exportZIP();
        }
    }
    protected void WFB2SL2100Generate_Click(object sender, EventArgs e)
    {
        try
        {
            string year = txt_DATA_YM_search.Text;
            string salary_dt_s = year+"/01/01";
            string salary_dt_e = year+"/12/31";
            string func_id = "FB2SL210"; //2014-09-25 fixed by Stanley Chen
            string msg = "";
            msg = service.executeGenerate(year, salary_dt_s, salary_dt_e, SessionHandle.Current.emp_id, func_id); //2014-09-25 fixed by Stanley Chen, add 2 parameters: login.id、function.id
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("executeFailMessage", "//n" + msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                showMessage("executeSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SL2100Export_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SL2100DAO fb2sl = new CFB2SL2100DAO();
            string year = txt_DATA_YM_search.Text;
            MemoryStream fileStream = service.Action(year);
            Session["SL2100_DATA_YM"] = txt_DATA_YM_search.Text;
            Session["SL2100_fileStream"] = fileStream;
            dwnframe.Attributes["src"] = "WFB2SL2100_Qry.aspx?SL2100_FileType = zipDefault";
            Session["SL2100_FileType"] = "zipDefault";
            if (fileStream != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    public void exportZIP()
    {
        try
        {
            if (Session["SL2100_FileType"] != null && Session["SL2100_FileType"].ToString() != "")
            {
                string fileType = Session["SL2100_FileType"].ToString();
                if (fileType == "zipDefault")
                {
                    MemoryStream fileStream = (MemoryStream)Session["SL2100_fileStream"];
                    Session["SL2100_FileType"] = "";
                    Session["SL2100_fileStream"] = null;

                    string chineseYear = (Convert.ToInt32(Session["SL2100_DATA_YM"]) - 1911).ToString();

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "filename=" + HttpUtility.UrlEncode(chineseYear + "年度綜合所得稅電子媒體申報檔.zip"));

                    System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
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