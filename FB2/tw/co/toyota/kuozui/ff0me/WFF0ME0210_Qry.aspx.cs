using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFF0ME0210_Qry : BasePage
{
    //宣告BO 物件
    private CFF0ME0110BO me010BO = new CFF0ME0110BO();
    private CFF0ME0210BO ME020BO = new CFF0ME0210BO();

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
    protected void WFF0ME0210EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0210DAO ME020DAO = new CFF0ME0210DAO();
            ME020DAO.BILL_YM = txt_BILL_YM.Text;
            //ME020DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME020DAO.VENDOR_ID = txt_VENDOR_ID.Text;
            ME020DAO.VENDOR_AREA = txt_VENDOR_AREA.Text;
            ME020DAO.SAP_INV_FLAG = txt_SAP_INV_FLAG.Text;
            ME020DAO.FUNC_ID = "FF0ME021";
            ME020DAO.CREATED_BY = SessionHandle.Current.emp_id;

            string msg = ME020BO.exec_SP_D2C_TRANS(ME020DAO);

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
    protected void WFF0ME0210DOWNLOAD_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0110DAO ME010DAO = new CFF0ME0110DAO();
            ME010DAO.BILL_YM = txt_BILL_YM.Text;
            //ME010DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME010DAO.VENDOR_ID = txt_VENDOR_ID.Text;
            ME010DAO.VENDOR_AREA = txt_VENDOR_AREA.Text;
            ME010DAO.SAP_INV_FLAG = txt_SAP_INV_FLAG.Text;
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
            string toPathTmpFile = Server.MapPath("~/ExcelTemplate/WFF0ME011_Download.xlsx");  //範本用ME010
            //先刪除原始的檔案
            File.Delete(toPathDownFile);
            IWorkbook workbook = me010BO.create_T060_INVOICE_EXCEL(toPathTmpFile, ME010DAO);
            FileStream file = new FileStream(toPathDownFile, FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            dwnframe.Attributes["src"] = "WFF0ME0210_Qry.aspx?FileType_ME0210=invoice";
            Session["FileType_ME0210"] = "invoice";
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
            if (Session["FileType_ME0210"] != null && Session["FileType_ME0210"].ToString() != "")
            {
                string FileType_ME0210 = Session["FileType_ME0210"].ToString();
                if (FileType_ME0210 == "excel")
                {
                    Session["FileType_ME0210"] = "";
                    ExcelHandle.file_Download(Server.MapPath("~/ExcelTemplate/DownloadFile/FF0ME020_error_" + SessionHandle.Current.emp_id + ".xlsx"), "FF0ME020_Error.xlsx");
                }

                if (FileType_ME0210 == "invoice")
                {
                    Session["FileType_ME0210"] = "";
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

