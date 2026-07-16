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
public partial class WebContent_fb2sd_WFB2SD1300_Qry : BasePage
{
    //Service 物件
    private CFB2SD1300BO service = new CFB2SD1300BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //txt_SALARY_YM.Text = DateTime.Now.ToString("yyyy/MM");
            //下拉式選單ddl_JPN_CD
            //getJPN_CD();
            ViewState["NewPageIndex"] = 0;
            this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
        }
        Session["FileType_fb2sd1300"] = "";
        Session["workbook_fb2sd1300"] = null;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
       
    }
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_fb2sd1300"] != null && Session["FileType_fb2sd1300"].ToString() != "")
            {
                string FileType_fb2sd1300 = Session["FileType_fb2sd1300"].ToString();
                if (FileType_fb2sd1300 == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_fb2sd1300"];
                    Session["FileType_fb2sd1300"] = "";
                    Session["workbook_fb2sd1300"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2SD130.xlsx");
                }
                if (FileType_fb2sd1300 == "excel2")
                {
                   
                    Session["FileType_fb2sd1300"] = "";
                    Session["workbook_fb2sd1300"] = null;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料');", true);
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    //private void getJPN_CD()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = service.getJPN_CD();
    //        ddl_JPN_CD.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_JPN_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
    //            }
    //            //ddl_JPN_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}




    protected void WFB2SD130ExcelExport_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            CFB2SD1300DAO wfb2sc = new CFB2SD1300DAO();
            wfb2sc.REMIT_DT = txt_REMIT_DT.Text;
            //int day = DateTime.DaysInMonth(Convert.ToInt32(txt_REMIT_DT.Text.Replace("/", "").Substring(0, 4)), Convert.ToInt32(txt_REMIT_DT.Text.Replace("/", "").Substring(4, 2)));
            //wfb2sc.day = Convert.ToString(day);
            //wfb2sc.SALARY_TYPE = ddl_SALARY_TYPE.SelectedValue;

            

            //if (ddl_SALARY_TYPE.SelectedValue == "A" )
            //{
            //    IWorkbook workbook = service.createExcel(txt_REMIT_DT.Text, ddl_SALARY_TYPE.SelectedValue);
               
            //    if (workbook != null)
            //    {
            //        Session["workbook_fb2sd1300"] = workbook;
            //        dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel";
            //        Session["FileType_fb2sd1300"] = "excel";
            //    }
            //    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            //    else {
            //        dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel2";
            //        Session["FileType_fb2sd1300"] = "excel2";
            //    }
            //    }

            //else if (ddl_SALARY_TYPE.SelectedValue == "B")
            //{
            //    IWorkbook workbook = service.createExcel2(txt_REMIT_DT.Text, ddl_SALARY_TYPE.SelectedValue);
               
            //    if (workbook != null)
            //    {
            //        Session["workbook_fb2sd1300"] = workbook;
            //        dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel";
            //        Session["FileType_fb2sd1300"] = "excel";
            //    }
            //    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            //    else
            //    {
            //        dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel2";
            //        Session["FileType_fb2sd1300"] = "excel2";
            //    }
                    
            //}
            //else
            //{

                IWorkbook workbook = service.createExcel3(txt_REMIT_DT.Text);
               
                if (workbook != null)
                {
                    Session["workbook_fb2sd1300"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel";
                    Session["FileType_fb2sd1300"] = "excel";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無匯出資料!');", true);
                    //dwnframe.Attributes["src"] = "WFB2SD1300_Qry.aspx?FileType_fb2sd1300 = excel2";
                    //Session["FileType_fb2sd1300"] = "excel2";
                }
            //}
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SD130ExcelExport, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}


