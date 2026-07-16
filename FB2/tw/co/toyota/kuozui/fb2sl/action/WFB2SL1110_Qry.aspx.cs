using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sl_WFB2SL1110_Qry : BasePage
{
    CFB2SL1110BO service = new CFB2SL1110BO();
    public static string type = "";
    public static string key1 = "";
    public static string key2 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);        
       

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //initial value           
            txt_DATA_YEAR.Text = DateTime.Now.ToString("yyyy");
        }
        //else
        //{
        //    if (FileUpload.HasFile)
        //    {
        //        string fileextension = Path.GetExtension(FileUpload.PostedFile.FileName);
        //        switch (fileextension.ToUpper())
        //        {
        //            case ".TXT":
        //                ViewState["UploadFileContent"] = service.getTxtData(FileUpload.FileContent);
        //                break;
        //            case ".XLSX":
        //                ViewState["UploadFileContent"] = service.getExcelData(FileUpload.FileContent, fileextension);
        //                break;
        //        }
        //    }
        //}        
        
    }
    /* 勞保txt匯入 */
    protected void WFB2SL1111Upload_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SL1110DAO dao = new CFB2SL1110DAO();
            
            //開始上傳作業
            if (FileUpload.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload.PostedFile.FileName);
                if (filename != ".txt")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }

                string fileextension = Path.GetExtension(FileUpload.PostedFile.FileName);
                ViewState["UploadFileContent"] = service.getTxtData(FileUpload.FileContent);

                dao.DATA_YEAR = txt_DATA_YEAR.Text;
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2SL111";
                                
                string result = service.updateTxtData((ArrayList)ViewState["UploadFileContent"], dao);
                if (result != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + result.ToString() + "');", true);
                }
                else
                {
                    showMessage("importSuccessMessage");
                }
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /* 健保匯入 */
    protected void WFB2SL1112Upload_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SL1110DAO dao = new CFB2SL1110DAO();
            
            //開始上傳作業
            if (FileUpload.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload.PostedFile.FileName);
                if (filename != ".xlsx" && filename != ".xls")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                dao.DATA_YEAR = txt_DATA_YEAR.Text;
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2SL111";
                IWorkbook workbook = service.uploadExcel(FileUpload.FileContent, System.IO.Path.GetExtension(FileUpload.PostedFile.FileName), dao);


                if (workbook == null)
                {                    
                    //WFB2SN0100Search_Click(null, null);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                }
                else
                {
                    #region 存在SERVER取代SESSION
                    /*
                    //刪除檔案
                    File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx"));

                    string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                    FileStream file = new FileStream(@toPath + "/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    
                    //Session["workbook_SH0200"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SN0100_Qry.aspx";
                    Session["FileType_SN010"] = "excelERR";

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                    */
                    #endregion
                }

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");

            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SL111_upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SL111_upload.xlsx"), "WFB2SL111_upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);

            }
        }
    }
    
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SN010"] != null && Session["FileType_SN010"].ToString() != "")
            {
                string fileType = Session["FileType_SN010"].ToString();
                if (fileType == "excel")
                {
                    Session["FileType_SN010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SN010_SAMPLE.xlsx");


                }
                if (fileType == "excelERR")
                {
                    Session["FileType_SN010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx"), "檢核錯誤說明.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    
}