using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using NPOI.SS.UserModel;
public partial class WebContent_fb2sc_WFB2SC5300_Qry : BasePage
{
    //Service 物件
    private CFB2SC5300BO service = new CFB2SC5300BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            createddl_SALARY_TYPE_search();
            //下拉式選單ddl_JPN_CD
            getJPN_CD();
            ViewState["NewPageIndex"] = 0;

            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
        }
    }
    private void createddl_SALARY_TYPE_search()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dtSALARY_TYPE = new DataTable();
            dtSALARY_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE_search.Items.Clear();
            ddl_SALARY_TYPE_search.Items.Add(new ListItem("", ""));
            if (dtSALARY_TYPE.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_TYPE.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE_search.Items.Add(new ListItem(dtSALARY_TYPE.Rows[i]["sub_desc"].ToString(), dtSALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SC530"] != null && Session["FileType_SC530"].ToString() != "")
            {
                string fileType = Session["FileType_SC530"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SC530"];
                    Session["FileType_SC530"] = "";
                    Session["workbook_SC530"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SC530_1.xlsx");

                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    private void getJPN_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getJPN_CD();
            ddl_JPN_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_JPN_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
                //ddl_JPN_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                ddl_JPN_CD.Items.Add(new ListItem("其他", "99"));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }




    protected void WFB2SC5300ExcelExport_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC5300BO service = new CFB2SC5300BO();
            string msg = "";
            CFB2SC5300DAO wfb2sc = new CFB2SC5300DAO();
            wfb2sc.SALARY_DT = txt_SALARY_DT_search.Text;
            wfb2sc.SALARY_TYPE = ddl_SALARY_TYPE_search.SelectedValue;
            wfb2sc.JPN_CD = ddl_JPN_CD.SelectedValue;
            wfb2sc.EMP_ID = txt_EMP_ID.Text;
            wfb2sc.DEPT_NO = txt_DEPT_NO.Text;
            // int day = DateTime.DaysInMonth(Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "").Substring(0, 4)), Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "").Substring(4, 2)));
            //wfb2sc.day = day.ToString();
            wfb2sc.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2sc.UPDATED_BY = SessionHandle.Current.emp_id;

            //1.刪除員工薪資明細表薪資項目檔(TB_S_M_SALARY_REPORT_H)
            //2.刪除員工薪資明細表(TB_S_M_SALARY_REPORT_D)
            //msg = service.deleteData(SessionHandle.Current.emp_id);
            msg = service.deleteData(wfb2sc);
            ////3.將符合查詢條件，且存在薪資明細歷史檔之薪資項目，寫入員工薪資明細表薪資項目檔(TB_S_M_SALARY_REPORT_H)
            msg = service.addData_H(wfb2sc);
            //4.讀取薪資明細歷史檔(TB_S_M_SALARY_PAY)、員工薪資明細表薪資項目檔
            //DataTable tmp1 = wfb2sc.searchAmount(SessionHandle.Current.emp_id);
            msg = service.addData_D(wfb2sc);

            bool isTB_S_S = service.getProcess_Status(wfb2sc);
            IWorkbook workbook = service.createExcel(txt_SALARY_DT_search.Text, ddl_SALARY_TYPE_search.SelectedValue, txt_DEPT_NO.Text, txt_EMP_ID.Text, ddl_JPN_CD.SelectedValue, SessionHandle.Current.emp_id, isTB_S_S);

            if (workbook == null)
            {
                string err = "查無資料!\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                return;
            }
            Session["workbook_SC530"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SC5300_Qry.aspx?FileType_SC530 = excel";
            Session["FileType_SC530"] = "excel";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC5300ExcelExport, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}


