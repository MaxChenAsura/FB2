using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//IWorkbook需要
using System.IO;
using NPOI.SS.UserModel;

public partial class WebContent_fb2ia_WFB2IA2200_Qry : BasePage
{
    CFB2IA2200BO service = new CFB2IA2200BO();
    string year = "";
    string month = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            txt_INS_YM.Text = DateTime.Now.AddMonths(-1).ToString("yyyy/MM");

        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    protected void WFB2IA2200Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA2200DAO fb2ia = new CFB2IA2200DAO();
            string INS_YM = txt_INS_YM.Text.Replace("/", "");
            DataTable dt = fb2ia.getExcelData("2200", INS_YM);
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/團保每月加保名單.xlsx"), "2200", INS_YM);
                Session["workbook_IA220"] = workbook;
                dwnframe.Attributes["src"] = "WFB2IA2200_Qry.aspx?FileType_IA220 = 2200";
                Session["FileType_IA220"] = "2200";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
                
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nodata", "alert('" + Resources.Resource.wfd2ia_nodata + "');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA2201Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA2200DAO fb2ia = new CFB2IA2200DAO();
            string INS_YM = txt_INS_YM.Text.Replace("/", "");
            DataTable dt = fb2ia.getExcelData("2201", INS_YM);
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/團保每月退保名單.xlsx"), "2201", INS_YM);
                Session["workbook_IA220"] = workbook;
                dwnframe.Attributes["src"] = "WFB2IA2200_Qry.aspx?FileType_IA220 = 2201";
                Session["FileType_IA220"] = "2201";
                if (workbook != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
                
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nodata", "alert('" + Resources.Resource.wfd2ia_nodata + "');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA2203Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA2200DAO fb2ia = new CFB2IA2200DAO();
            string INS_YM = txt_INS_YM.Text.Replace("/", "");
            DataTable dt = fb2ia.getExcelData("2203", INS_YM);
            if (dt.Rows.Count > 0)
            {

                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/團保在保名單.xlsx"), "2203", INS_YM);
                Session["workbook_IA220"] = workbook;
                dwnframe.Attributes["src"] = "WFB2IA2200_Qry.aspx?FileType_IA220 = 2203";
                Session["FileType_IA220"] = "2203";
                if (workbook != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nodata", "alert('" + Resources.Resource.wfd2ia_nodata + "');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_IA220"] != null && Session["FileType_IA220"].ToString() != "")
            {
                string FileType_IA220 = Session["FileType_IA220"].ToString();

                IWorkbook workBook = (IWorkbook)Session["workbook_IA220"];
                Session["FileType_IA220"] = "";
                Session["workbook_IA220"] = null;

                if (FileType_IA220 == "2200")
                    ExcelHandle.exportExcel(workBook, "FB2IA220_1.xlsx");
                if (FileType_IA220 == "2201")
                    ExcelHandle.exportExcel(workBook, "FB2IA220_2.xlsx");
                if (FileType_IA220 == "2203")
                    ExcelHandle.exportExcel(workBook, "FB2IA220_3.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void WFB2IA2200Execute_Click(object sender, EventArgs e)
    {
        string INS_YM = txt_INS_YM.Text;
        string date = DateTime.DaysInMonth(Convert.ToInt16(INS_YM.Substring(0, 4)), Convert.ToInt16(INS_YM.Substring(5, 2))).ToString();
        string INS_DT = INS_YM.Replace("/", "") + date;
        CFB2IA2200DAO fb2ia = new CFB2IA2200DAO();
        string msg = service.Execute(fb2ia, INS_DT);
        string SELF_TOTAL = "";
        if (msg != "")
            SELF_TOTAL = Convert.ToInt32(msg).ToString("N0");
        else
            SELF_TOTAL = "0";
        string showmsg = "團保之保險年月:" + INS_YM + "\\n國瑞員工自付保費金額" + SELF_TOTAL;
        //if (msg != "0")
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
        //else
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + showmsg + "');", true);

    }
}