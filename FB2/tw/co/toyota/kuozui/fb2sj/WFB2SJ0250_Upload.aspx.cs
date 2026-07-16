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

public partial class WebContent_fb2sj_WFB2SJ0250_Upload : BasePage
{
    //Service 物件
    private CFB2SJ0250BO sj025BO = new CFB2SJ0250BO();
    private CFB2SJ0200BO sj020BO = new CFB2SJ0200BO();

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
        WFB2SJ0250DAO dao = new WFB2SJ0250DAO();
        dao.getAssessData();
        txt_YEAR.Text=dao.ASSESS_YEAR ;

        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", ""));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_ASSESS_TYPE.SelectedValue = dao.ASSESS_TYPE;

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
            if (Session["FileType_SJ0250"] != null && Session["FileType_SJ0250"].ToString() != "")
            {
                string FileType_SJ0250 = Session["FileType_SJ0250"].ToString();
                if (FileType_SJ0250 == "excel")
                {
                    Session["FileType_SJ0250"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ025_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
                if (FileType_SJ0250 == "download")
                {
                    Session["FileType_SJ0250"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx"), "WFB2SJ_Result.xlsx");
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
    protected void WFB2SJ0250Upload_Click(object sender, EventArgs e)
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

                WFB2SJ0250DAO sj025dao = new WFB2SJ0250DAO();
                sj025dao.ASSESS_YEAR = txt_YEAR.Text;
                sj025dao.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
                string freeze_flag = sj025dao.getFreeze_flag();
                if (freeze_flag == "E")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無該年度及類型考核資料');doUnBlock();", true);
                    return;
                }
                if (freeze_flag == "Y") {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無法處理(簽核中/已發佈)');doUnBlock();", true);
                    return;
                }


                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");



                workbook = sj025BO.uploadExcel1(FileUpload1.FileContent, Path.GetExtension(FileUpload1.PostedFile.FileName), sj025dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ025_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    dwnframe.Attributes["src"] = "WFB2SJ0250_Upload.aspx?FileType_SJ0250=excel";
                    Session["FileType_SJ0250"] = "excel";
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
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SJ_Result.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SJ_Result.xlsx"), "WFB2SJ025_Upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

            }
        }
    }
    #endregion

    //考核結果下載
    protected void WFB2SJ0250Result_Click(object sender, EventArgs e)
    {
        try
        {
            //生成EXCEL
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_YEAR.Text;
            sj020DAO.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;

            //取得下載資料
            DataTable dt = sj020DAO.getExcelResultData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = sj020BO.createExcelResult(Server.MapPath("~/ExcelTemplate/WFB2SJ_Result.xlsx"), sj020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //
            dwnframe.Attributes["src"] = "WFB2SJ0250_Upload.aspx?FileType_SJ0250=download";
            Session["FileType_SJ0250"] = "download";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");

            /*
            //下載EXCEL
            string downloadPath = "~/ExcelTemplate/DownloadFile/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx";
            if (File.Exists(Server.MapPath(downloadPath)))
            {
                try
                {                
                    ExcelHandle.excel_Down(Server.MapPath(downloadPath), "WFB2SJ025_result.xlsx");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

                }
            }
            */
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);

        }

    }

 
}