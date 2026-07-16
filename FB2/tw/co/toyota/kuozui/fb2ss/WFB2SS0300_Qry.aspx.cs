using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SS0300_Qry : BasePage
{
  
    //Service 物件
    private CFB2SS0300BO dl040BO = new CFB2SS0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);        

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            /* 測試初始值
            txt_JOIN_SDT.Text="2019/01/01";
            txt_JOIN_EDT.Text="2019/12/31";
            txt_LEAVE_SDT.Text = "2019/01/01";
            txt_LEAVE_EDT.Text = "2019/01/01";
            */

            //匯出EXCEL檔
            this.exportExcel();

            //取得 資料
            initialValue();

        }        

    }

      #region DB資料取得
     //取得查詢條件的資料
    private void initialValue()
    {
        try
        {
            ddl_IS_BE_EMP.Items.Add(new ListItem("", "-1"));
            ddl_IS_BE_EMP.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_BE_EMP.Items.Add(new ListItem("N-否", "N"));

            ddl_IS_LEAVE.Items.Add(new ListItem("", "-1"));
            ddl_IS_LEAVE.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_LEAVE.Items.Add(new ListItem("N-否", "N"));


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion
  

    //匯出EXCEL
    protected void WFB2SS0300EXCEL_Click(object sender, EventArgs e)
    { 
        try{
            CFB2SS0300DAO ss030DAO = new CFB2SS0300DAO();


            ss030DAO.JOIN_SDT = txt_JOIN_SDT.Text;
            ss030DAO.JOIN_EDT = txt_JOIN_EDT.Text;
            ss030DAO.BE_EMP_SDT = txt_BE_EMP_SDT.Text;
            ss030DAO.BE_EMP_EDT = txt_BE_EMP_EDT.Text;
            ss030DAO.LEAVE_SDT = txt_LEAVE_SDT.Text;
            ss030DAO.LEAVE_EDT = txt_LEAVE_EDT.Text;
            ss030DAO.IS_LEAVE = ddl_IS_LEAVE.SelectedValue;
            ss030DAO.IS_BE_EMP = ddl_IS_BE_EMP.SelectedValue;

            DataTable dt = ss030DAO.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS030" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = dl040BO.excelDownload(Server.MapPath("~/ExcelTemplate/WFB2SS030.xlsx"), ss030DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SS030_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            dwnframe.Attributes["src"] = "WFB2SS0300_Qry.aspx?FileType_SS0300=excel";
            Session["FileType_SS0300"] = "excel";
            if (workbook != null)
            {
                //exportExcel("考核查詢資料.xlsx");
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
            }
            else
            {
                showMessage("noDownDataMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SS0300"] != null && Session["FileType_SS0300"].ToString() != "")
            {
                Session["FileType_SS0300"] = "";
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS030_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SS030Excel.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


}