using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2df_WFB2DF0400_Qry : BasePage
{
    CFB2DF0400BO service = new CFB2DF0400BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }
    }
    protected void WFB2DF0400ExcelDown_Click(object sender, EventArgs e)
    {
        string err = "";
        CFB2DF0400DAO dao = new CFB2DF0400DAO();
        dao.MANAGER_YM = txt_MANAGER_DT.Text;
        DataTable dt = service.searchData(dao);
        if(dt.Rows.Count == 0){
            err += "查無資料!\\n";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            return;
        }
        else
        {
           //檢核有效卡號是否有重複 有的話就拋出錯誤訊息
            string st = "";
            string id = "";
            DataTable dt1 = service.checkData(dao);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    st = dt1.Rows[i]["count"].ToString();
                    if (Convert.ToInt32(st) > 1 )
                    {
                        id = dt1.Rows[i]["EMP_ID"].ToString()+"\\n";
                    }
                }
            }

            string msg = "住宿員工有效卡號大於1張，請洽詢勤務擔當協助處理，工號如下: \\n";
            if (id != "")
            {
                msg = msg + id;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                return;
            }

            //有block
            IWorkbook workbook = service.createExcel(dao, "xlsx");
            Session["workbook_DF040"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DF0400_Qry.aspx?FileType_DF040 = excel";
            Session["FileType_DF040"] = "excel";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        
    }
    protected void WFB2DF0400TxtDown_Click(object sender, EventArgs e)
    {
        string err = "";
        CFB2DF0400DAO dao = new CFB2DF0400DAO();
        dao.MANAGER_YM = txt_MANAGER_DT.Text;
        DataTable dt = service.searchData(dao);
        if (dt.Rows.Count == 0)
        {
            err += "查無資料!\\n";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            return;
        }
        else
        {
            //檢核有效卡號是否有重複 有的話就拋出錯誤訊息
            string st = "";
            string id = "";
            DataTable dt1 = service.checkData(dao);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    st = dt1.Rows[i]["count"].ToString();
                    if (Convert.ToInt32(st) > 1)
                    {
                        id = dt1.Rows[i]["EMP_ID"].ToString() + "\\n";
                    }
                }
            }

            string msg = "住宿員工有效卡號大於1張，請洽詢勤務擔當協助處理，工號如下: \\n";
            if (id != "")
            {
                msg = msg + id;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                return;
            }

            service.createTxt(dao);
        }
       
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DF040"] != null && Session["FileType_DF040"].ToString() != "")
            {
                string fileType = Session["FileType_DF040"].ToString();
               
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DF040"];
                    Session["FileType_DF040"] = "";
                    Session["workbook_DF040"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2DF040_EMP_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

}