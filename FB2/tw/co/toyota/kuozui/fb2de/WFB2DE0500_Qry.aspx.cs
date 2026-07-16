using ACESLib;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0500_Qry : BasePage
{
    CFB2DE0500BO service = new CFB2DE0500BO();
    private string emp_id = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = SessionHandle.Current.emp_id;          //取得使用者ID

        if (!IsPostBack)
        {
            createPLANT_CD();
            //將Session 的workbook 匯出Excel
            this.exportExcel();            
        }

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
    }
    private void createPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DE0300Cancel_Click(object sender, EventArgs e)
    {
        //InitialView();
    }   

    protected void WFB2DE0500ExcelDown_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            CFB2DE0500DAO dao = new CFB2DE0500DAO();
            dao.MANAGER_YM = txt_MANAGER_YM.Text.Replace("/", "");
            IWorkbook workbook = service.createExcelDateMonth(dao, "xlsx");
            #region 存在SERVER取代SESSION
            //刪除檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DE050_" + SessionHandle.Current.emp_id + ".xlsx"));

            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DE050_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_DE050"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DE0500_Qry.aspx?FileType_DE050 = excel";
            Session["FileType_DE050"] = "excel";

            if (workbook != null)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DE0500ExcelDown, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DE050"] != null && Session["FileType_DE050"].ToString() != "")
            {
                string fileType = Session["FileType_DE050"].ToString();
                if (fileType == "excel")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_DE050"];
                    //Session["FileType_DE050"] = "";
                    //Session["workbook_DE050"] = null;

                    //ExcelHandle.exportExcel(workBook, "FB2DE050_SAMPLE.xlsx");
                    Session["FileType_DE050"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DE050_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DE050_SAMPLE.xlsx");
                    

                }
                if (fileType == "excelERR")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_DE050"];
                    //Session["FileType_DE050"] = "";
                    //Session["workbook_DE050"] = null;

                    //ExcelHandle.exportExcel(workBook, "FB2DE050_SAMPLE.xlsx");
                    Session["FileType_DE050"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DE050_ERR_" + SessionHandle.Current.emp_id + ".xlsx"), "檢核錯誤說明.xlsx");
                   
                }  

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void WFB2DE0500Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload.PostedFile.FileName);
                if (filename != ".xlsx")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                CFB2DE0500DAO dao = new CFB2DE0500DAO();
                dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
                dao.MANAGER_YM = txt_MANAGER_YM.Text.Replace("/","");
                //string msg = service.uploadExcel(FileUpload.FileContent, System.IO.Path.GetExtension(FileUpload.PostedFile.FileName), dao);
                IWorkbook workbook = service.uploadExcel(FileUpload.FileContent, System.IO.Path.GetExtension(FileUpload.PostedFile.FileName),dao);

               
                if (workbook == null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                }
                else
                {
                    #region 存在SERVER取代SESSION
                    //刪除檔案
                    File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DE050_ERR_" + SessionHandle.Current.emp_id + ".xlsx"));

                    string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                    FileStream file = new FileStream(@toPath + "/FB2DE050_ERR_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion
                    //Session["workbook_SH0200"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2DE0500_Qry.aspx";
                    Session["FileType_DE050"] = "excelERR";
               
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                }

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
               
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}