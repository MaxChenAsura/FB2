using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC1500_Qry : BasePage
{
    CFB2DC1500BO service = new CFB2DC1500BO();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //取得工廠區分
            getPLANT_CD();
            //取得職務
            getWS_CD();
            //取得工數區分
            getWORK_CD();

            //將Session 的workbook 匯出Excel
            this.exportExcel();

        }

    }

    private void getWORK_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWS_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
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

    private void getPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC1500_ExportXLS1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1500DAO dao = new CFB2DC1500DAO();
            dao.DUTY_YM = txt_DUTY_YM.Text;
            dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;
            dao.WS_CD = ddl_WS_CD.SelectedValue;
            dao.WORK_CD = ddl_WORK_CD.SelectedValue;

            //DataTable tmp = dao.searchResult1();
            //if (tmp.Rows.Count == 0) {
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無匯出資料');", true);
            //    return;
            //}

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC150_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook = service.createExcel1(dao, "xlsx");
             if (workbook == null)
            {
                showMessage("noDownDataMessage");
                return;
            }
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DC150_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

           
            //Session["workbook_DC1500"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DC1500_Qry.aspx?FileType_DC1500=excel1";
            Session["FileType_DC1500"] = "excel1";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //請假統計
    protected void WFB2DC1500_ExportXLS2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1500DAO dao = new CFB2DC1500DAO();
            dao.DUTY_YM = txt_DUTY_YM.Text;
            dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;
            dao.WS_CD = ddl_WS_CD.SelectedValue;
            dao.WORK_CD = ddl_WORK_CD.SelectedValue;

            //string msg = service.createExcel2(dao, "xlsx");
            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "jQuery(document).ready(function () { alert('" + msg + "'); });", true);
            //    return;
            //}
            //else { }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC152_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook = service.createExcel2(dao, "xlsx");
            if (workbook == null)
            {
                showMessage("noDownDataMessage");
                return;
            }
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DC152_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            dwnframe.Attributes["src"] = "WFB2DC1500_Qry.aspx?FileType_DC1500=excel2";
            Session["FileType_DC1500"] = "excel2";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //加班明細
    protected void WFB2DC1500_ExportXLS3_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1500DAO dao = new CFB2DC1500DAO();
            dao.DUTY_YM = txt_DUTY_YM.Text;
            dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;
            dao.WS_CD = ddl_WS_CD.SelectedValue;
            dao.WORK_CD = ddl_WORK_CD.SelectedValue;

            //string msg=service.createExcel3(dao, "xlsx");
            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "jQuery(document).ready(function () { alert('" + msg + "'); });", true);
            //    return;
            //}
            //else { }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC153_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook = service.createExcel3(dao, "xlsx");
            if (workbook == null)
            {
                showMessage("noDownDataMessage");
                return;
            }
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DC153_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            dwnframe.Attributes["src"] = "WFB2DC1500_Qry.aspx?FileType_DC1500=excel3";
            Session["FileType_DC1500"] = "excel3";

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
            if (Session["FileType_DC1500"] != null && Session["FileType_DC1500"].ToString() != "")
            {
                string fileType = Session["FileType_DC1500"].ToString();
                if (fileType == "excel1")
                {
                    Session["FileType_DC1500"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC150_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DC150_1.xlsx");
                }
                else if (fileType == "excel2")
                {
                    Session["FileType_DC1500"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC152_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DC150_2.xlsx");
                }
                else if (fileType == "excel3")
                {
                    Session["FileType_DC1500"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC153_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DC150_3.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC1500DAO dao = new CFB2DC1500DAO();
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