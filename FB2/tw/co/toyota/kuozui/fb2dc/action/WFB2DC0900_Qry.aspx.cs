using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0900_Qry : BasePage
{
    //Service 物件
    private CFB2DC0900BO service = new CFB2DC0900BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();
            //查詢條件的預設值-工號,姓名
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;
            txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            txt_EMP_NAME.Text = SessionHandle.Current.emp_name;
        }

    }
    protected void WFB2DC0900Export_Click(object sender, EventArgs e)
    {
        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID.Text.Trim()) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }

            CFB2DC0900DAO dao = new CFB2DC0900DAO();
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.CALENDAR_DT_S = txt_CALENDAR_DT_S.Text;
            dao.CALENDAR_DT_E = txt_CALENDAR_DT_E.Text;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;


            IWorkbook workbook = service.createExcel(dao, "xlsx");
            if (workbook == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                #region 存在SERVER取代SESSION
                //先刪除原始的檔案
                File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC090_1_" + SessionHandle.Current.emp_id + ".xlsx"));
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2DC090_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                //Session["workbook_DC0900"] = workbook;
                dwnframe.Attributes["src"] = "WFB2DC0900_Qry.aspx?";
                Session["FileType_DC0900"] = "excel1";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DC0900"] != null && Session["FileType_DC0900"].ToString() != "")
            {
                string fileType = Session["FileType_DC0900"].ToString();
                if (fileType == "excel1")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_DC0900"];
                    //Session["workbook_DC0900"] = null;
                    //ExcelHandle.exportExcel(workBook, "FB2DC090_1.xlsx");
                    Session["FileType_DC0900"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC090_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DC090_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string emp_id = txt_EMP_ID.Text;

            DataTable dt = service.getEMP_NAME(emp_id);

            if (dt.Rows.Count > 0)
            {

                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();

            }
            else
            {
                txt_EMP_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_DEPT_NO.Text;

            DataTable dt = service.getDEPT_NAME(dept_no);

            if (dt.Rows.Count > 0)
            {

                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();

            }
            else
            {
                txt_DEPT_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}