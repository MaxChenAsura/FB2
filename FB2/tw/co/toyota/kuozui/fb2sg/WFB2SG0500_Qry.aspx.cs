using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using Ionic.Zip;
using System.Web.UI.HtmlControls;


public partial class WebContent_WFB2SG0500_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SG0500BO sg050BO = new CFB2SG0500BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
         //第一次進入頁面執行
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            string lastYear = Convert.ToString(Convert.ToInt32(DateTime.Now.ToString("yyyy"))-1);
            txt_START_DT.Text= lastYear+"/01/01";
            txt_END_DT.Text = lastYear + "/12/31";
            getQryItem();
            //Session["FileType_SG0500"] = "";
            //Session["workbook_SG050"] = null;
        }


    }

    //取得查詢條件-員工區分、支付狀態、在職區分
    private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
           
            //在職區分
            dt = utilities.getCommCode("HB", "EMP_STATUS", "", "");
            ddl_EMP_STATUS.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
                ddl_EMP_STATUS.SelectedValue="01";//預設為在職
            }

            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region button 事件
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SG0500"] != null && Session["FileType_SG0500"].ToString() != "")
            {
                string FileType_SG0500 = Session["FileType_SG0500"].ToString();
                if (FileType_SG0500 == "excel")
                {
                    Session["FileType_SG0500"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG050_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SG050_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }
        finally {
            Session["FileType_SG0500"] = "";
            Session["workbook_SG0500"] = null;
        }

    }

    //一時金對象下載
    protected void WFB2SG0500ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SG0500DAO sg050DAO = new CFB2SG0500DAO();
            sg050DAO.START_DT = txt_START_DT.Text;
            sg050DAO.END_DT = txt_END_DT.Text;

            sg050DAO.EMP_ID = txt_EMP_ID.Text;
            sg050DAO.EMP_NAME = txt_EMP_NAME.Text;
            sg050DAO.EMP_STATUS = ddl_EMP_STATUS.SelectedValue;
            sg050DAO.EMP_CD = ddl_EMP_CD.SelectedValue;
            


            DataTable dt = sg050DAO.getMaintainData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG050_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook =  sg050BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_OneTime.xlsx"), sg050DAO);

            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SG050_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            dwnframe.Attributes["src"] = "WFB2SG0500_Qry.aspx?FileType_SG0500 = excel";
            Session["FileType_SG0500"] = "excel";
            if (workbook != null)
            {
                //exportExcel("考核查詢資料.xlsx");
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion


  
}
