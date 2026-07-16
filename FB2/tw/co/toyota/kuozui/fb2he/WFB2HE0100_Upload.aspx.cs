using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HE0100_Upload : BasePage
{
    //Service 物件
    private CFB2HE0100BO service = new CFB2HE0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
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
            if (Session["FileType_HE0100"] != null && Session["FileType_HE0100"].ToString() != "")
            {
                string FileType_SG0201 = Session["FileType_HE0100"].ToString();
                if (FileType_SG0201 == "excel")
                {
                    Session["FileType_HE0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HE010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2HE0100Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                if (filename != ".xlsx" && filename != ".xls")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                IWorkbook workbook = service.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HE010_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2HE0100_Upload.aspx?FileType_HE0100=excel";
                    Session["FileType_HE0100"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }

            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2HE010_Upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2HE010_Upload.xlsx"), "WFB2HE010_Upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

            }
        }
    }
    #endregion

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["HE0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HE0100_Qry.aspx");
    }
}