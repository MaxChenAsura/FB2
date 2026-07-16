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

public partial class WebContent_fb2pa_WFB2PA0100_Upload : BasePage
{
    //Service 物件
    private CFB2PA0100BO pa010BO = new CFB2PA0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {            
            getQryItem();
          
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }

    //取得查詢條件的資料
    private void getQryItem()
    {
       

        try
        {
            DataTable dt = new DataTable();
            CFB2PA0100DAO pa0100DAO=new CFB2PA0100DAO();
            dt = pa0100DAO.getLastCloseYm();
            if (dt.Rows.Count > 0)
            {
                string sLastYm = dt.Rows[0]["YM"].ToString();
                DateTime originalDate = new DateTime(Convert.ToInt32(sLastYm.Substring(0,4)),Convert.ToInt32(sLastYm.Substring(4,2)), 01);
                DateTime newDate = originalDate.AddMonths(1);
                txt_YM.Text = newDate.ToString("yyyyMM");
            }
            else
            {
                DateTime newDate = DateTime.Now;
                txt_YM.Text = newDate.ToString("yyyyMM");

            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    


    #region EXCEL上傳相關
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_PA0100"] != null && Session["FileType_PA0100"].ToString() != "")
            {
                string FileType_PA0100 = Session["FileType_PA0100"].ToString();
                if (FileType_PA0100 == "excel")
                {
                    Session["FileType_PA0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2PA010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
                if (FileType_PA0100 == "download")
                {
                    Session["FileType_PA0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2PA010_2_" + SessionHandle.Current.emp_id + ".xlsx"), "WFB2SJ_Result.xlsx");
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
    protected void WFB2PA0100Upload_Click(object sender, EventArgs e)
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

                CFB2PA0100DAO pa0100Dao = new CFB2PA0100DAO();
                pa0100Dao.YM = txt_YM.Text.Replace("/","");


                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");



                workbook = pa010BO.uploadExcel1(FileUpload1.FileContent, Path.GetExtension(FileUpload1.PostedFile.FileName), pa0100Dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2PA010_error_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                if (workbook == null)
                {
                    DataTable dt = pa0100Dao.getLastLog("FB2PA010", SessionHandle.Current.emp_id);
                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('" + dt.Rows[0]["CHANGE_DESC"] + "');</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                    }
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
                    dwnframe.Attributes["src"] = "WFB2PA0100_Upload.aspx?FileType_PA0100=excel";
                    Session["FileType_PA0100"] = "excel";
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
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2PA010_Upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2PA010_Upload.xlsx"), "WFB2PA010_Upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

            }
        }
    }
    #endregion

   

 
}