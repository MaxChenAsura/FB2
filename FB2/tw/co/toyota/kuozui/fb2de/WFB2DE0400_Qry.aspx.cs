using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0400_Qry : BasePage
{
    CFB2DE0400BO service = new CFB2DE0400BO();
    private string emp_id = "";
    private string emp_name = "";
    private string emp_company_cd = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = SessionHandle.Current.emp_id;          //取得使用者ID
        emp_name = SessionHandle.Current.emp_name;      //取得使用者Name
        CFB2DE0200DAO dao = new CFB2DE0200DAO();
        emp_company_cd = dao.getCOMPANY_CD(emp_id);     //取得KZ會社區分

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value
            getRESTAURANT_CD();
            getReportCD();

            //將Session 的workbook 匯出Excel
            this.exportExcel();            
        }
    }

    private void getRESTAURANT_CD()
    {
        CFB2DE0400DAO dao = new CFB2DE0400DAO();       
        try
        {
            DataTable dt = new DataTable();
            dt = dao.getCommCode("DE", "RESTAURANT_CD", "", "");
            ddl_RESTAURANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_RESTAURANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
          

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getReportCD()
    {
        CFB2DE0400DAO dao = new CFB2DE0400DAO();        
        try
        {
            DataTable dt = new DataTable();
            dt = dao.getCommCode("DE", "RESTAURANT_DOCUMENT", "", "");
            //ddl_DOCUMENT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DOCUMENT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    protected void WFB2DE0400ExcelDown_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            CFB2DE0400DAO dao = new CFB2DE0400DAO();
            dao.RESTAURANT_CD = ddl_RESTAURANT_CD.SelectedValue;
            dao.DOCUMENT_CD = ddl_DOCUMENT_CD.SelectedValue;
            dao.MANAGER_YM_S = txt_MANAGER_YM_S.Text.Replace("/", "-");
            dao.MANAGER_YM_E = txt_MANAGER_YM_E.Text.Replace("/", "-");
            dao.MANAGER_YM = txt_MANAGER_YM.Text.Replace("/","");
            dao.COMPANY_CD = emp_company_cd;

            if (dao.DOCUMENT_CD.Equals("1"))
            {
                //是否有資料
                DataTable dt1 = service.searchDayData(dao);
                if (dt1.Rows.Count == 0)
                {
                    err += "查無資料!\\n";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    return;
                }else
	            {
                    //有block
                     IWorkbook workbook = service.createExcelDate(dao, "xlsx");               
                     Session["workbook_DE040"] = workbook;
                     dwnframe.Attributes["src"] = "WFB2DE0400_Qry.aspx?FileType_DE040 = excel";
                     Session["FileType_DE040"] = "excelday";
                     
                     if (workbook != null)
                     {
                         
                     }
                     else
                     {
                         ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                     }

	            }
               
            }
            else {
                DataTable dt2 = service.searchMonthData(dao);
                if (dt2.Rows.Count == 0)
                {
                    err += "查無資料!\\n";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    return;
                }else
	            {                     
                     //有block
                    IWorkbook workbook = service.createExcelDateMonth(dao, "xlsx");
                     Session["workbook_DE040"] = workbook;
                     dwnframe.Attributes["src"] = "WFB2DE0400_Qry.aspx?FileType_DE040 = excel";
                     Session["FileType_DE040"] = "excelmonth";
                     
                     if (workbook != null)
                     {

                     }
                     else
                     {
                         ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                     }
	            }                
            }
            
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DE0400ExcelDown, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DE040"] != null && Session["FileType_DE040"].ToString() != "")
            {
                string fileType = Session["FileType_DE040"].ToString();
                if (fileType == "excelday")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DE040"];
                    Session["FileType_DE040"] = "";
                    Session["workbook_DE040"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2DE040_DAILY.xlsx");
                    
                }
                if (fileType == "excelmonth")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DE040"];
                    Session["FileType_DE040"] = "";
                    Session["workbook_DE040"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2DE040_MONTHLY.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

   
}