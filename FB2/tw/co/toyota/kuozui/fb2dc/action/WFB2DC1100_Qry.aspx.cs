using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC1100_Qry : BasePage
{
    //Service 物件
    private CFB2DC1100BO service = new CFB2DC1100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //取得統計類型
            getBORROW_REASON_CD();

            //將Session 的workbook 匯出Excel
            this.exportExcel();

        }
    }

    private void getBORROW_REASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DC", "ABNORMAL_REASON_CD", "", "");
            cb_DUTY_CHECK_RESULT.Items.Add(new ListItem("借卡", "X"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_DUTY_CHECK_RESULT.Items.Add(new ListItem((dt.Rows[i]["sub_desc"].ToString().Split('-'))[1], dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC1100Export_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1100DAO dao = new CFB2DC1100DAO();
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.CALENDAR_DT_S = txt_CALENDAR_DT_S.Text;
            dao.CALENDAR_DT_E = txt_CALENDAR_DT_E.Text;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;
            dao.COUNT = txt_COUNT.Text;
            if (cb_DUTY_CHECK_RESULT.Items[0].Selected)
                dao.TYPE1 = "Y";
            else
                dao.TYPE1 = "N";



            dao.OTHER_TYPE = new Dictionary<string, string>();
            for (int i = 1; i < cb_DUTY_CHECK_RESULT.Items.Count; i++)
            {
                if (cb_DUTY_CHECK_RESULT.Items[i].Selected)
                    dao.OTHER_TYPE.Add(cb_DUTY_CHECK_RESULT.Items[i].Value, cb_DUTY_CHECK_RESULT.Items[i].Text);
            }

            //if (dao.TYPE1 == "N" && dao.OTHER_TYPE.Count == 0)
            //{
            //    //都沒勾選視同都勾選
            //    dao.TYPE1 = "Y";
            //    for (int i = 1; i < cb_DUTY_CHECK_RESULT.Items.Count; i++)
            //    {
            //        dao.OTHER_TYPE.Add(cb_DUTY_CHECK_RESULT.Items[i].Value, cb_DUTY_CHECK_RESULT.Items[i].Text);
            //    }
            //}

            //string msg = service.createExcel(dao, "xlsx");
            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "jQuery(document).ready(function () { alert('" + msg + "'); });", true);
            //    return;
            //}
            //else { }

            IWorkbook result = service.createExcel(dao, "xlsx");
            if (result == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                Session["workbook_DC1100"] = result;
                dwnframe.Attributes["src"] = "WFB2DC1100_Qry.aspx?";
                Session["FileType_DC1100"] = "excel1";
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
            if (Session["FileType_DC1100"] != null && Session["FileType_DC1100"].ToString() != "")
            {
                string fileType = Session["FileType_DC1100"].ToString();
                if (fileType == "excel1")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DC1100"];
                    Session["FileType_DC1100"] = "";
                    Session["workbook_DC1100"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2DC110_1.xlsx");
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
        CFB2DC1100DAO dao = new CFB2DC1100DAO();
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
        else
        {
            txt_EMP_NAME.Text = "";
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC1100DAO dao = new CFB2DC1100DAO();
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
        else
        {
            txt_DEPT_NAME.Text = "";
        }
    }
}