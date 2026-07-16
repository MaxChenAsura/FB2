using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2SJ0170_Upload : BasePage
{
    //Service 物件
    private CFB2SJ0170BO sj017BO = new CFB2SJ0170BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //將Session 的workbook 匯出Excel
            createASSESS_TYPE();
            this.exportExcel();
        }
    }


    private void createASSESS_TYPE()
    {
        try
        {
            DataTable dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ASSESS_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            /**
            if (Session["FileType_SJ0170"] != null && Session["FileType_SJ0170"].ToString() != "")
            {
                string FileType_SJ0170 = Session["FileType_SJ0170"].ToString();
                if (FileType_SJ0170 == "excel")
                {
                    Session["FileType_SJ0170"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ023_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                }
            }**/
            if (Session["FileType_SJ0170"] != null && Session["FileType_SJ0170"].ToString() != "")
            {
                string FileType_SJ0170 = Session["FileType_SJ0170"].ToString();
                if (FileType_SJ0170 == "excel")
                {
                    //Session["FileType_SJ0130"] = "";
                    //Session["workbook_SJ0130"] = null;
                    //ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ013_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");

                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ0170"];
                    Session["FileType_SJ0170"] = "";
                    Session["workbook_SJ0170"] = null;

                    ExcelHandle.exportExcel(workBook, "error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //上傳
    protected void WFB2SJ0170Upload_Click(object sender, EventArgs e)
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
                IWorkbook workbook = sj017BO.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), 
                    txt_ASSESS_YEAR.Text, ddl_ASSESS_TYPE.SelectedValue);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ170_error_" + SessionHandle.Current.emp_id + ".xlsx");
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
                    Session["workbook_SJ0170"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SJ0170_Upload.aspx?FileType_SJ0170=excel";
                    Session["FileType_SJ0170"] = "excel";
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

    
    //EXCEL匯出
    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SJ017_Upload.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SJ017_Upload.xlsx"), "Upload.xlsx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }

        }
    }
    //返回
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SJ0170_Is_Search"] = "Y";
        Response.Redirect("WFB2SJ0170_Qry.aspx");
    }
}