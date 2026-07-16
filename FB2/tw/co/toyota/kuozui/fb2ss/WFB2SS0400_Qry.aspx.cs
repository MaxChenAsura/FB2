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

public partial class WebContent_WFB2SS0400_Qry : BasePage
{
    CFB2SS0400BO ss040BO = new CFB2SS0400BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //取得 資料
            initialValue();

        }
    }

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //類別
            dt = utilities.getCommCode("SS", "INCENTIVE_TYPE", "", "", "Y");
            ddl_INCENTIVE_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INCENTIVE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    
    //上傳計算
    protected void WFBSS0400Upload_Click(object sender, EventArgs e)
    {
        try
        {
            //後端檢核-已轉前工程不可刪除(1.已轉薪資,2.節金檔已有相同節金類型及發放日期)
            CFB2SS0400DAO dao = new CFB2SS0400DAO();
            dao.SALARY_DT = txt_SALARY_DT.Text;
            dao.INCENTIVE_TYPE = ddl_INCENTIVE_TYPE.SelectedValue;

            string msg = "0";
            msg = ss040BO.chkIS_SEND(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }

            if (FileUpload1.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                if (filename != ".xlsx" && filename != ".xls")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                IWorkbook workbook = ss040BO.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), dao);

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS040_error_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                if (workbook == null)
                {
                    /* 做資遣費激勵金的計算 */
                    msg = ss040BO.exec_SP_S_SS040(dao);
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
                    dwnframe.Attributes["src"] = "WFB2SS0400_Qry.aspx?FileType_SS0400=excel";
                    Session["FileType_SS0400"] = "excel";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
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
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SS040_Import.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2SS040_Import.xlsx"), "FB2SS040_upload.xlsx");
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
            if (Session["FileType_SS0400"] != null && Session["FileType_SS0400"].ToString() != "")
            {
                string FileType_SS0400 = Session["FileType_SS0400"].ToString();
                if (FileType_SS0400 == "excel")
                {
                    Session["FileType_SS0400"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS040_error_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SS040error.xlsx");
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