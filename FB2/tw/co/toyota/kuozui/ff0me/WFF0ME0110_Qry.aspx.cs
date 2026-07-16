using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFF0ME0110_Qry : BasePage
{
    //宣告BO 物件
    private CFF0ME0110BO me010BO = new CFF0ME0110BO();

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
    protected void WFF0ME0110EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0110DAO ME010DAO = new CFF0ME0110DAO();
            ME010DAO.BILL_YM  = txt_BILL_YM.Text ;
            //ME010DAO.ACCOUNT_TRM = txt_ACCOUNT_TRM.Text;
            //ME010DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME010DAO.FUNC_ID = "FF0ME011";
            ME010DAO.CREATED_BY = SessionHandle.Current.emp_id;

            //string msg = "0";
            string msg = me010BO.exec_SP_DC2_TRANS(ME010DAO);
            
            if (msg != "0")
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('執行失敗:" + msg + "');</script>");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行失敗:" + msg.Replace("\r\n", "").Replace("'", "\"") + "');iniForm();", true);
            }
            else {
                int resultCount = ME010DAO.getresultCount();
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

    //發票資料下載
    protected void WFF0ME0110DOWNLOAD_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0110DAO ME010DAO = new CFF0ME0110DAO();
            ME010DAO.BILL_YM = txt_BILL_YM.Text.Replace("/", "");
            ME010DAO.TRANS_FLAG = ddl_TRANS_FLAG.SelectedValue;
            //ME010DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");  
            ME010DAO.FUNC_ID = "FF0ME011";
            ME010DAO.CREATED_BY = SessionHandle.Current.emp_id;
            ME010DAO.VENDOR_ID = "";
            ME010DAO.VENDOR_AREA = "";
            ME010DAO.SAP_INV_FLAG = "";
            DataTable dt = new DataTable();
            //取得下載資料
            dt = ME010DAO.getT060_INVOICE();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
			
			logger.Error("enter WFF0ME0110DOWNLOAD_Click-取完資料");
            //相關路徑
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            string toPathDownFile = @toPath + "/FF0ME011_" + SessionHandle.Current.emp_id + ".xlsx";
            string toPathTmpFile = Server.MapPath("~/ExcelTemplate/WFF0ME011_Download.xlsx");
            //先刪除原始的檔案
            File.Delete(toPathDownFile);
			
			logger.Error("enter WFF0ME0110DOWNLOAD_Click-處理完檔案");
            IWorkbook workbook = me010BO.create_T060_INVOICE_EXCEL(toPathTmpFile, ME010DAO);
            FileStream file = new FileStream(toPathDownFile, FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            dwnframe.Attributes["src"] = "WFF0ME0110_Qry.aspx?FileType_ME0110=invoice";
            Session["FileType_ME0110"] = "invoice";
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
            if (Session["FileType_ME0110"] != null && Session["FileType_ME0110"].ToString() != "")
            {
                string FileType_ME0100 = Session["FileType_ME0110"].ToString();
                if (FileType_ME0100 == "excel")
                {
                    Session["FileType_ME0110"] = "";
                    ExcelHandle.file_Download(Server.MapPath("~/ExcelTemplate/DownloadFile/FF0ME011_" + SessionHandle.Current.emp_id + ".xlsx"), "FF0ME010_Error.xlsx");
                }
         
                if (FileType_ME0100 == "invoice")
                {
                    Session["FileType_ME0110"] = "";
                    ExcelHandle.file_Download(Server.MapPath("~/ExcelTemplate/DownloadFile/FF0ME011_" + SessionHandle.Current.emp_id + ".xlsx"), "FF0ME010_INVOICE.xlsx");
                }
               

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


  
}

