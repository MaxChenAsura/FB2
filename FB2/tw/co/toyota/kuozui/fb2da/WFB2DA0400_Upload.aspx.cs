using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

public partial class WebContent_fb2da_WFB2DA0400_Upload : BasePage
{
    //Service 物件
    private WFB2DA0400BO service = new WFB2DA0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            txt_YEAR.Text = DateTime.Now.Year.ToString();
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }

    #region EXCEL上傳相關
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DA0400"] != null && Session["FileType_DA0400"].ToString() != "")
            {
                string FileType_DA0400 = Session["FileType_DA0400"].ToString();
                if (FileType_DA0400 == "excel")
                {
                    Session["FileType_DA0400"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FF2DA040_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    protected void WFB2DA0400Upload_Click(object sender, EventArgs e)
    {
        string msg = "";
        try
        {
            if (FileUpload1.HasFile)
            {
                IWorkbook workbook;
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                if (filename != ".xlsx" && filename != ".xls")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }

                WFB2DA0400DAO dao = new WFB2DA0400DAO();
                dao.YEAR = txt_YEAR.Text;
                //年度需>=系統年
                int nowyear = DateTime.Now.Year;
                if (Convert.ToInt32(dao.YEAR) < nowyear)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('年度需大於或等於系統年');", true);
                    return;
                }

                workbook = service.uploadExcel1(FileUpload1.FileContent, Path.GetExtension(FileUpload1.PostedFile.FileName), dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FF2DA040_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2DA0400_Upload.aspx?FileType_DA0400=excel";
                    Session["FileType_DA0400"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

        }
    }
    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2DA040_Upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2DA040_Upload.xlsx"), "WFB2DA040_Upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

            }
        }
    }
    #endregion

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DA0400_Is_Search"] = "Y";
        Response.Redirect("WFB2DA0400_Qry.aspx");
    }

}