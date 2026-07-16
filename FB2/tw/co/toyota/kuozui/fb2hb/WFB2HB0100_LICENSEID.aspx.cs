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

public partial class WebContent_WFB2HB0100_LICENSEID : BasePage
{
    CFB2HB0100BO hb010BO = new CFB2HB0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //initialValue();

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
            /*
            ddl_INCENTIVE_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INCENTIVE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            */
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    
    //上傳計算
    protected void WFBHB0100Upload_Click(object sender, EventArgs e)
    {
        try
        {
            
            string msg = "0";
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
                IWorkbook workbook = hb010BO.uploadLICENSEID(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));

                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB010_error_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                if (workbook == null)
                {                    
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
                    dwnframe.Attributes["src"] = "WFB2HB0100_LICENSEID.aspx?FileType_HB010=LID";
                    Session["FileType_HB010"] = "LID";
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
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2HB010_LICENSEID.xlsx")))
        {
            try
            {
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/WFB2HB010_LICENSEID.xlsx"), "外籍技術員.xlsx");
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
            if (Session["FileType_HB010"] != null && Session["FileType_HB010"].ToString() != "")
            {
                string FileType_HB010 = Session["FileType_HB010"].ToString();
                if (FileType_HB010 == "LID")
                {
                    Session["FileType_HB010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB010_error_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HB010error.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //返回
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["HB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0100_Qry.aspx");
    }
}