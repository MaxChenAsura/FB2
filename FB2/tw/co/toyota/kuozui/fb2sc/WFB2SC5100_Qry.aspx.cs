using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC5100_Qry : BasePage
{
    CFB2SC5100BO service = new CFB2SC5100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        if (!IsPostBack)
        {
            this.exportPDF();
            createddl_SALARY_TYPE();
        }
    }

    private void createddl_SALARY_TYPE()
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            DataTable dtSALARY_TYPE = new DataTable();
            dtSALARY_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE.Items.Clear();
            ddl_SALARY_TYPE.Items.Add(new ListItem("", ""));
            if (dtSALARY_TYPE.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_TYPE.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dtSALARY_TYPE.Rows[i]["sub_desc"].ToString(), dtSALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //PDF匯出按鈕事件
    protected void WFB2SC5100Print_Click(object sender, EventArgs e)
    {
        try
        {
            string pay_id = txt_PAY_ID.Text;
            string salary_type = ddl_SALARY_TYPE.SelectedValue;
            string dept_no = txt_DEPT_NO.Text.Trim();
            string emp_id = txt_EMP_ID.Text.Trim();
            //取得基本資料
            DataTable dtPDFData = service.getExcelData(pay_id, salary_type, dept_no, emp_id);
            if (dtPDFData.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            MemoryStream fileStream = service.createExcelFromTemplate(Server.MapPath("~/Fonts/kaiu.ttf"), pay_id, salary_type, dept_no, emp_id, dtPDFData);
            Session["SC5100_fileStream"] = fileStream;
            dwnframe.Attributes["src"] = "WFB2SC5100_Qry.aspx?SC5100_FileType = pdfDefault";
            Session["SC5100_FileType"] = "pdfDefault";
            if (fileStream != null)
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void exportPDF()
    {
        try
        {
            if (Session["SC5100_FileType"] != null && Session["SC5100_FileType"].ToString() != "")
            {
                string fileType = Session["SC5100_FileType"].ToString();
                if (fileType == "pdfDefault")
                {
                    MemoryStream fileStream = (MemoryStream)Session["SC5100_fileStream"];
                    Session["SC5100_FileType"] = "";
                    Session["SC5100_fileStream"] = null;

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("WFB2SC5100_1.pdf"));
                    System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    fileStream.Close();
                    fileStream.Dispose();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}