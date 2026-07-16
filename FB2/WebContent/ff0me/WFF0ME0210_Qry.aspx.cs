using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFF0ME0200_Qry : BasePage
{
    //宣告BO 物件
    private CFF0ME0100BO me010BO = new CFF0ME0100BO();
    private CFF0ME0200BO ME020BO = new CFF0ME0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

        }
    }


    #region button 事件
    //執行
    protected void WFF0ME0200EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0200DAO ME020DAO = new CFF0ME0200DAO();
            ME020DAO.YM = txt_ACCOUNT_YM.Text;
            ME020DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME020DAO.T06FAC = txt_FAC.Text;
            ME020DAO.T06ARE = txt_ARE.Text;
            ME020DAO.T06FLG = txt_FLG.Text;
            ME020DAO.FUNC_ID = "FF0ME020";
            ME020DAO.CREATED_BY = SessionHandle.Current.emp_id;

            string msg = ME020BO.exec_SP_D5CT060_TRANS(ME020DAO);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行失敗:" + msg.Replace("\r\n", "").Replace("'", "\"") + "');iniForm();", true);
                return;                                                                                             
            }
            else
            {
                int resultCount = ME020DAO.getresultCount();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('執行成功,共" + resultCount + "筆發票');</script>");
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message.Replace("\r\n", "").Replace("'", "\""));
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');doUnBlock();", true);
        }
    }

  

    //未轉出發票資料下載(借ME010的功能)
    protected void WFF0ME0200DOWNLOAD_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0100DAO ME010DAO = new CFF0ME0100DAO();
            ME010DAO.ACCOUNT_YM = txt_ACCOUNT_YM.Text;
            ME010DAO.YM = txt_ACCOUNT_YM.Text;
            ME010DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME010DAO.T06FAC = txt_FAC.Text;
            ME010DAO.T06ARE = txt_ARE.Text;
            ME010DAO.T06FLG = txt_FLG.Text;
            ME010DAO.FUNC_ID = "FF0ME020";
            ME010DAO.TRANS_FLAG = "N";
            ME010DAO.CREATED_BY = SessionHandle.Current.emp_id;

            DataTable dt = new DataTable();
            //取得下載資料
            dt = ME010DAO.getT060_INVOICE();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //相關路徑
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            string toPathDownFile = @toPath + "/FF0ME020_" + SessionHandle.Current.emp_id + ".xlsx";
            string toPathTmpFile = Server.MapPath("~/ExcelTemplate/WFF0ME010_Download.xlsx");  //範本用ME010
            //先刪除原始的檔案
            File.Delete(toPathDownFile);
            IWorkbook workbook = me010BO.create_T060_INVOICE_EXCEL(toPathTmpFile, ME010DAO);
            FileStream file = new FileStream(toPathDownFile, FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            dwnframe.Attributes["src"] = "WFF0ME0200_Qry.aspx?FileType_ME0200=invoice";
            Session["FileType_ME0200"] = "invoice";
            if (workbook != null)
            {
                //exportExcel("考核查詢資料.xlsx");
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message.Replace("\r\n", "").Replace("'", "\""));
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }



    #endregion

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_ME0200"] != null && Session["FileType_ME0200"].ToString() != "")
            {
                string FileType_ME0200 = Session["FileType_ME0200"].ToString();
                if (FileType_ME0200 == "excel")
                {
                    Session["FileType_ME0200"] = "";
                    ExcelHandle.file_Download(Server.MapPath("~/ExcelTemplate/DownloadFile/FF0ME020_error_" + SessionHandle.Current.emp_id + ".xlsx"), "FF0ME020_Error.xlsx");
                }

                if (FileType_ME0200 == "invoice")
                {
                    Session["FileType_ME0200"] = "";
                    ExcelHandle.file_Download(Server.MapPath("~/ExcelTemplate/DownloadFile/FF0ME020_" + SessionHandle.Current.emp_id + ".xlsx"), "FF0ME020_INVOICE.xlsx");
                }
               
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

  
}

