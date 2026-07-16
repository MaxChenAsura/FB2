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

public partial class WebContent_fb2se_WFB2SE0100_Upload : BasePage
{
    //Service 物件
    private CFB2SE0100BO se010BO = new CFB2SE0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {            
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
            if (Session["FileType_SE0100"] != null && Session["FileType_SE0100"].ToString() != "")
            {
                string FileType_SE0100 = Session["FileType_SE0100"].ToString();
                if (FileType_SE0100 == "excel")
                {
                    Session["FileType_SE0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
                if (FileType_SE0100 == "download")
                {
                    Session["FileType_SE010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE010_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SE010_LEVEL.xlsx");
                }
                
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    //上傳
    protected void WFB2SE0100Upload_Click(object sender, EventArgs e)
    {
        string msg = "";
        string release_dt = "";
        try
        {
            //檢核,是否已核可
            CFB2SE0100DAO se010DAO = new CFB2SE0100DAO();
            se010DAO.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            
            DataTable dt = se010DAO.Get_H_RELEASE_DT();
            if (dt.Rows.Count > 0)
            {
                release_dt = Convert.ToString(dt.Rows[0]["RELEASE_DT"]);
            }
            if (release_dt != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + txt_EFFECT_YM.Text + "資料,已提出核可申請,不允修改。');doUnBlock();", true);
                return;
            }
            
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

                workbook = se010BO.uploadExcel1(FileUpload1.FileContent, Path.GetExtension(FileUpload1.PostedFile.FileName), se010DAO);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE010_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2SE0100_Upload.aspx?FileType_SE0100=excel";
                    Session["FileType_SE0100"] = "excel";
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

    //範例下載
    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SE0100_upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SE0100_upload.xlsx"), "WFB2SE0100_upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
            }
        }
    }
 


    //資格下載 //生成EXCEL
    protected void WFB2SE0100LoadAdd_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SE0100DAO se010DAO = new CFB2SE0100DAO();

            //取得下載資料
            DataTable dt = se010DAO.getExcelResultData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE010_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = se010BO.createExcelResult(Server.MapPath("~/ExcelTemplate/WFB2SE0100_upload.xlsx"), se010DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SE010_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //
            dwnframe.Attributes["src"] = "WFB2SE0100_Upload.aspx?FileType_SE0100=download";
            Session["FileType_SE0100"] = "download";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

        }
    }
    //返回
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SE0100_Is_Search"] = "Y";
        Response.Redirect("WFB2SE0100_Qry.aspx");
    }

    #endregion
}