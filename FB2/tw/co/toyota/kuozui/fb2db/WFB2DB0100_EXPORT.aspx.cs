using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0100_EXPORT : BasePage
{
    //Service 物件
    private WFB2DB0100BO service = new WFB2DB0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //取得輪值表下拉清單  
            getWORK_SHIFT_CD();
            //匯出EXCEL檔
            this.exportExcel();
        }
    }

    private void getWORK_SHIFT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getTB_D_M_WORK_SHIFT_H();
            ddl_WORK_SHIFT_CD.Items.Add(new ListItem("", "-1"));
            ddl_WORK_SHIFT_CD.Items.Add(new ListItem("ALL-全部", "ALL"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_SHIFT_CD.Items.Add(
                        new ListItem(dt.Rows[i]["WORK_SHIFT_DESC"].ToString(),
                            dt.Rows[i]["WORK_SHIFT_CD"].ToString()));
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DB0100"] != null && Session["FileType_DB0100"].ToString() != "")
            {
                string FileType_DB0100 = Session["FileType_DB0100"].ToString();
                if (FileType_DB0100 == "downReport")
                {
                    Session["FileType_DB0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DB010_DOWNLOAD_" + SessionHandle.Current.emp_id + ".xlsx"), "FF2DB010_DOWNLOAD.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void WFB2DB0100EXCEL_Click(object sender, EventArgs e)
    {
        string msg = "";
        WFB2DB0100DAO dao = new WFB2DB0100DAO();
        dao.WORK_SHIFT_CD = ddl_WORK_SHIFT_CD.SelectedValue;
        dao.START_DT2 = txt_START_DATE.Text;
        dao.END_DT2 = txt_END_DATE.Text;
        try
        {
            #region 檢核
            if (dao.START_DT2.Substring(0, 4) != dao.END_DT2.Substring(0, 4))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('日期不可跨年度');", true);
                return;
            }

            #endregion

            // 取得差異分析資料，並供使用者下載EXCEL報表
            DataTable dt = service.getTB_D_M_WORK_SHIFT_D_t(dao);
            if (dt.Rows.Count > 0)
            {
                //提供EXCEL下載實績檢核異常報表
                IWorkbook workbook = service.createDownloadData(Server.MapPath("~/ExcelTemplate/WFB2DB010_Upload.xlsx"), dao, dt);
                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DB010_DOWNLOAD_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);

                if (workbook != null)
                {
                    #region 存在SERVER取代SESSION

                    FileStream file = new FileStream(@toPath, FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion
                    //Session["workbook_DB0100"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2DB0100_EXPORT.aspx?FileType_DB0100=downReport";
                    Session["FileType_DB0100"] = "downReport";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "downReport", "doUnBlock();", true);
                }

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "errorMessage", "<script>doUnBlock();</script>");

            }
            else
            {
                showMessage("executeFailMessage", msg);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DB0100_Qry.aspx");
    }

}