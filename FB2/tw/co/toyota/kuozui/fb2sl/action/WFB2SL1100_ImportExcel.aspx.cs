using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2sl_WFB2SL1100_ImportExcel : BasePage
{
    //Service 物件
    private CFB2SL1100BO service = new CFB2SL1100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportExcel();

            ViewState["NewPageIndex"] = 0;
        }

    }
    #region "Control Event"
    protected void txt_COMPANY_CD_search_TextChanged(object sender, EventArgs e)
    {
        if (txt_COMPANY_CD_search.Text != "")
        {
            CFB2SL1100DAO dao = new CFB2SL1100DAO();
            DataTable dtCompany_cd = dao.getCompany_cd(txt_COMPANY_CD_search.Text);
            if (dtCompany_cd.Rows.Count == 1)
            {
                txt_COMPANY_NAME_search.Text = Convert.ToString(dtCompany_cd.Rows[0]["COMPANY_SNAME"]);
            }
            else
            {
                txt_COMPANY_CD_search.Text = "";
                txt_COMPANY_NAME_search.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2sl_COMPANY_CD_isNotExist + "');", true);
            }
        }
        else
            txt_COMPANY_NAME_search.Text = "";
    }
    #endregion

    #region "Button Event"
    protected void WFB2SL1100Download_Click(object sender, EventArgs e)
    {
        try
        {
            string data_format = ddl_DATA_FORMAT.SelectedValue;
            if (data_format == "A" || data_format == "D")
                service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SL110_Import_1.xlsx"), data_format);
            else if (data_format == "V")
                service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SL110_Import_2.xlsx"), data_format);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SL1100Download, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }
    protected void WFB2SL110Process_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                string company_cd = txt_COMPANY_CD_search.Text.ToUpper();
                string data_ym = txt_DATA_YM_search.Text;
                string data_format = ddl_DATA_FORMAT.SelectedValue;
                string msg = string.Empty;
                IWorkbook workbook;
                CFB2SL1100DAO dao = new CFB2SL1100DAO();
                if (!dao.checkExistIsRepeat(company_cd, data_ym, data_format))
                {
                    workbook = service.importExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), company_cd, data_ym, data_format);
                    Session["SL1100_workbook"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SL1100_ImportExcel.aspx?SL1100_FileType=excel";
                    if (workbook != null)
                    {

                        Session["SL1100_FileType"] = "excel";
                        //exportExcel("考核查詢資料.xlsx");
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('上傳失敗!');$.unblockUI();", true);
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳失敗');</script>");
                    }
                    else
                    {
                        Session["SL1100_FileType"] = "";
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('上傳成功!');$.unblockUI();", true);
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳成功');</script>");
                    }
                }
                else
                {
                    string path = System.IO.Path.GetTempPath();
                    //string fileName = "C:\\Users\\Administrator\\Desktop\\import\\WFB2SL110_Import_Example.xlsx";
                    string fileName = FileUpload1.PostedFile.FileName.ToString();
                    string NEW = fileName.Replace("\\", "/");
                    int index = NEW.LastIndexOf('/');
                    if (index != -1)
                        NEW = NEW.Substring(index + 1, NEW.Length - index - 1);
                    FileUpload1.SaveAs(path + NEW);
                    hid_file_path.Value = path + NEW;
                    //FileUpload1.SaveAs(FileUpload1.PostedFile.FileName);
                    //hid_file_path.Value = FileUpload1.PostedFile.FileName;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmImport", "confirmImportAfter();", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SL110Process, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //所得年度已存在，繼續進行
    protected void btn_confirmImport_Click(object sender, EventArgs e)
    {
        string company_cd = txt_COMPANY_CD_search.Text;
        string data_ym = txt_DATA_YM_search.Text;
        string data_format = ddl_DATA_FORMAT.SelectedValue;
        //刪除所得年度和資料格式相同的所有資料，再匯入
        service.deleteData(company_cd, data_ym, data_format);
        System.IO.FileStream filestream = new System.IO.FileStream(hid_file_path.Value, System.IO.FileMode.Open);
        IWorkbook workbook = service.importExcel(filestream, System.IO.Path.GetExtension(hid_file_path.Value), company_cd, data_ym, data_format);
        Session["SL1100_workbook"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SL1100_ImportExcel.aspx?SL1100_FileType=excel";
        if (workbook != null)
        {
            Session["SL1100_FileType"] = "excel";
            //exportExcel("考核查詢資料.xlsx");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳失敗');</script>");
        }
        else
        {
            Session["SL1100_FileType"] = "";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳成功');</script>");
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["SL1100_FileType"] != null && Session["SL1100_FileType"].ToString() != "")
            {
                string fileType = Session["SL1100_FileType"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["SL1100_workbook"];
                    Session["SL1100_FileType"] = "";
                    Session["SL1100_workbook"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SL1100_error.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SL0100_Is_Search"] = "Y";
        Response.Redirect("WFB2SL1100_Qry.aspx");
    }
    #endregion

    
}

