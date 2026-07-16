using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SS0100_Qry : BasePage
{
    CFB2SS0100BO ss010BO = new CFB2SS0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            
            initialValue();

            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //類別
            dt = utilities.getCommCode("SS", "FIRED_TYPE", "", "", "Y");
            ddl_FIRED_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_FIRED_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion

    //上傳計算
    protected void WFBSS0100Upload_Click(object sender, EventArgs e)
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

                CFB2SS0100DAO dao = new CFB2SS0100DAO();
                dao.SALARY_DT = txt_SALARY_DT.Text;
                dao.FIRED_TYPE = ddl_FIRED_TYPE.SelectedValue;

                string msg = "0";
                //是否已有轉薪資,或節金檔是否有相同資料
                msg = ss010BO.chkIS_SEND(dao);
                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                    return;
                }

                IWorkbook workbook = ss010BO.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS010_error_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                if (workbook == null)
                {
                    /* 做資遣費的計算 */
                    msg = "0";
                    msg = ss010BO.doExec(dao);
                    
                    if (msg != "0")
                    {
                        showMessage("executeFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("executeSuccessMessage");
                    }
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
                    dwnframe.Attributes["src"] = "WFB2SS0100_Qry.aspx?FileType_SS0100=excel";
                    Session["FileType_SS0100"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('" + "上傳失敗，請下載檔案檢查!" + "');doUnBlock();</script>");
                }

            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);

        }
    }

    protected void btn_Excel_Down_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SS010_Import.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SS010_Import.xlsx"), "FB2SS010_upload.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);

            }
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SS0100"] != null && Session["FileType_SS0100"].ToString() != "")
            {
                string FileType_SS0100 = Session["FileType_SS0100"].ToString();
                if (FileType_SS0100 == "excel")
                {
                    Session["FileType_SS0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SS010error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
}