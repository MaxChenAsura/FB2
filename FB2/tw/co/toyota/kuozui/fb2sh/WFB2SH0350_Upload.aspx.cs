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

public partial class WebContent_fb2sh_WFB2SH0350_Upload : BasePage
{
    //Service 物件
    private CFB2SH0350BO sh035BO = new CFB2SH0350BO();

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
        //取得最大的考核年度及類型
        WFB2SH0350DAO dao = new WFB2SH0350DAO();
        dao.getAwardData();
        txt_YEAR.Text=dao.AWARD_YEAR ;
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SH", "AWARD_ROUND", "", "");
            ddl_AWARD_ROUND.Items.Add(new ListItem("", ""));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_AWARD_ROUND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_AWARD_ROUND.SelectedValue = dao.AWARD_ROUND;

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
            if (Session["FileType_SH0350"] != null && Session["FileType_SH0350"].ToString() != "")
            {
                string FileType_SH0350 = Session["FileType_SH0350"].ToString();
                if (FileType_SH0350 == "excel")
                {
                    Session["FileType_SH0350"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH035_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
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
    protected void WFB2SH0350Upload_Click(object sender, EventArgs e)
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

                WFB2SH0350DAO sh035dao = new WFB2SH0350DAO();
                sh035dao.AWARD_YEAR = txt_YEAR.Text;
                sh035dao.AWARD_ROUND = ddl_AWARD_ROUND.SelectedValue;
                string freeze_flag = sh035dao.getFreeze_flag();
                if (freeze_flag == "E")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無該年度及回數的年獎資料');doUnBlock();", true);
                    return;
                }
                if (freeze_flag == "Y") {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無法處理(簽核中/已發佈)');doUnBlock();", true);
                    return;
                }


                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");



                workbook = sh035BO.uploadExcel1(FileUpload1.FileContent, Path.GetExtension(FileUpload1.PostedFile.FileName), sh035dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH035_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2SH0350_Upload.aspx?FileType_SH0350=excel";
                    Session["FileType_SH0350"] = "excel";
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
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SH0350_upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SH0350_upload.xlsx"), "WFB2SH035_Upload.xlsx");
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