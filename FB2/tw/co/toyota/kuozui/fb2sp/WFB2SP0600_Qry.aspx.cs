
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SP0600_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SP0600BO bo = new CFB2SP0600BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            txt_REMARK.Text = "按中華民國XX年XX月XX日財政部台財稅字第YYYYYY號公告。";
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
       
    }


    #region button 事件

    //資料下載
    protected void WFB2SP0600ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SP0600DAO dao = new CFB2SP0600DAO();
            dao.RETIRE_YM = txt_RETIRE_YM.Text;
            dao.REMARK = txt_REMARK.Text;
            DataTable dt = new DataTable();
            //取得下載資料
            dt = dao.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SP060_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = bo.createExcelFromTemplateDefault(Server.MapPath("~/ExcelTemplate/WFB2SP060.xlsx"), dao);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SP060_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            //Session["workbook_SP0600"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SP0600_Qry.aspx?FileType_SP0600 = excel";
            Session["FileType_SP0600"] = "excel";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SP0600"] != null && Session["FileType_SP0600"].ToString() != "")
            {
                string FileType_SP0600 = Session["FileType_SP0600"].ToString();
                if (FileType_SP0600 == "excel")
                {
                    Session["FileType_SP0600"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SP060_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SP060.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #endregion


  
}
