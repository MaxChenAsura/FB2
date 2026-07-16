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

public partial class WebContent_fb2di_WFB2DI1000_Qry : BasePage
{
  

    //Service 物件
    private CFB2DI1000BO di100BO = new CFB2DI1000BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);        

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();

            //txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            //txt_DATA_YM_S.Text="201908";
            //txt_DATA_YM_E.Text = "201909";
           

            //取得 職種/考核類別 資料
            getQryItem();

            

        }        

    }

      #region DB資料取得
     //取得查詢條件的資料
     private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
  

    //匯出EXCEL
    protected void WFB2DI1000EXCEL_Click(object sender, EventArgs e)
    { 
        try{
            CFB2DI1000DAO di100DAO = new CFB2DI1000DAO();


            di100DAO.YM_S = txt_DATA_YM_S.Text.Replace("/", "");
            di100DAO.YM_E = txt_DATA_YM_E.Text.Replace("/", "");
            di100DAO.EMP_ID = txt_EMP_ID.Text;
            di100DAO.DEPT_NO = txt_DEPT_NO.Text;
            di100DAO.WS_CD = ddl_WS_CD.SelectedValue;

            DataTable dt = di100DAO.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI100_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = di100BO.excelDownload(Server.MapPath("~/ExcelTemplate/WFB2DI100.xlsx"), di100DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DI100_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            dwnframe.Attributes["src"] = "WFB2DI1000_Qry.aspx?FileType_DI1000=excel";
            Session["FileType_DI1000"] = "excel";
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
            if (Session["FileType_DI1000"] != null && Session["FileType_DI1000"].ToString() != "")
            {
                Session["FileType_DI1000"] = "";
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI100_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DI100Excel.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


}