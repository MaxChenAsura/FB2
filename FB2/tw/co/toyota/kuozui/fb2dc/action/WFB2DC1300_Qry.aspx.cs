using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//DataTable要用
using System.Data;
using NPOI.SS.UserModel;



public partial class WebContent_fb2dc_WFB2DC1300_Qry : BasePage
{
    //Service 物件
    private CFB2DC1300BO service = new CFB2DC1300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

        }

    }
    protected void WFB2DC1300Export_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1300DAO dao = new CFB2DC1300DAO();
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.CALENDAR_DT = txt_CALENDAR_DT_S.Text;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;

            //string msg = service.createExcel(dao, "xlsx");
            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "jQuery(document).ready(function () { alert('" + msg + "'); });", true);
            //    return;
            //}
            //else
            //{ }

            IWorkbook result = service.createExcel(dao, "xlsx");
            if (result == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                Session["workbook_DC1300"] = result;
                dwnframe.Attributes["src"] = "WFB2DC1300_Qry.aspx?";
                Session["FileType_DC1300"] = "excel1";
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
            if (Session["FileType_DC1300"] != null && Session["FileType_DC1300"].ToString() != "")
            {
                string fileType = Session["FileType_DC1300"].ToString();
                if (fileType == "excel1")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DC1300"];
                    Session["FileType_DC1300"] = "";
                    Session["workbook_DC1300"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2DC130_1.xlsx");
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
        CFB2DC1300DAO dao = new CFB2DC1300DAO();
        string emp_id = txt_EMP_ID.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            DataTable dt = dao.getEmp_Name(emp_id);
            if (dt.Rows.Count == 1)
            {
                txt_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
            }
            else
            {
                txt_EMP_ID.Text = "";
                txt_EMP_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC1300DAO dao = new CFB2DC1300DAO();
        string dept_no = txt_DEPT_NO.Text;
        if (!string.IsNullOrEmpty(dept_no))
        {
            DataTable dt = dao.getDEPT_NAME(dept_no);
            if (dt.Rows.Count == 1)
            {
                txt_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
            }
            else
            {
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DEPT_NOerror", "alert('部門代號輸入錯誤');", true);
            }
        }
    }
}