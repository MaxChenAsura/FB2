using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using System.IO;

public partial class WebContent_fb2dl_WFB2DL0103_LabelPrint : BasePage
{
    //Service 物件
    private CFB2DL0100BO service = new CFB2DL0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            //產生員工區分下拉式選單
            createddl_EMP_CD_seaarch();
        }
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
    }
    //產生員工區分下拉式選單
    private void createddl_EMP_CD_seaarch()
    {
        try
        {
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("HB", "EMP_CD", "Y");
            ddl_EMP_CD_seaarch.Items.Clear();
            ddl_EMP_CD_seaarch.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD_seaarch.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD_seaarch, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //Excel匯出按鈕事件
    protected void WFB2DL0103ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            string base_year = txt_Year.Text;
            string dept_no = txt_DEPT_NO.Text;
            string emp_cd = ddl_EMP_CD_seaarch.SelectedValue;
            string emp_id = txt_EMP_ID_search.Text;
            string join_sdt = txt_JOIN_SDT.Text;
            string join_edt = txt_JOIN_EDT.Text;
            DataTable dtExcelData = dao.getExcelData(base_year, dept_no, emp_cd, emp_id, join_sdt, join_edt);
            if (dtExcelData.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            IWorkbook workbook = service.createExcelFromTemplate( base_year, dept_no, emp_cd, emp_id, dtExcelData);
            #region 存在SERVER取代SESSION
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DL0103_" + SessionHandle.Current.emp_id + ".xlsx"));
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DL0103_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["DL0103_workbook"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DL0103_LabelPrint.aspx?DL0103_FileType = excelDefault";
            Session["DL0103_FileType"] = "excelDefault";
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["DL0103_FileType"] != null && Session["DL0103_FileType"].ToString() != "")
            {
                string fileType = Session["DL0103_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    //IWorkbook workBook = (IWorkbook)Session["DL0103_workbook"];
                    Session["DL0103_FileType"] = "";
                    Session["DL0103_workbook"] = null;
                    //ExcelHandle.exportExcel(workBook, "WFB2DL0103_1.xlsx");
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DL0103_" + SessionHandle.Current.emp_id + ".xlsx"), "WFB2DL0103_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0100_Qry.aspx");
    }
}