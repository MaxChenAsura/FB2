using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sd_WFB2SD1200_Qry : BasePage
{
    CFB2SD1200BO service = new CFB2SD1200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生在轉帳類別下拉
            getddl_TRANSCLASS();
            getddl_SALARY_ACCOUNT_BANK();
            this.exportTXT();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    //protected void paykindCheck()
    //{
    //    try
    //    {
    //        string PAY_KIND = txt_PAY_KIND.Text;
    //        CFB2SD1200DAO fb2sd = new CFB2SD1200DAO();
    //        DataTable dt = fb2sd.paykind(PAY_KIND);
    //        string msg = "輸入代碼不存在!";
    //        if (dt.Rows.Count == 0)
    //        {
    //            txt_PAY_KIND.Text = "";
    //            txt_SALARY_NAME.Text = "";
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
    //        }
    //        else
    //        {
    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                txt_SALARY_NAME.Text = Convert.ToString(dr["SALARY_NAME"]);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
      //轉帳組別下拉
    private void getddl_TRANSCLASS()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("SD", "TRANSCLASS", "050", "","Y");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRANSCLASS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getddl_SALARY_ACCOUNT_BANK()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("SC", "SALARY_BANK_ID", "", "", "Y");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["sub_cd"].ToString() != "007") //目前不會產出一銀的媒體檔
                    {
                        ddl_SALARY_ACCOUNT_BANK.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                    
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void ddl_SALARY_ACCOUNT_BANK_Changed(object sender, EventArgs e)
    {
        try
        {
            ddl_TRANSCLASS.Items.Clear();
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("SD", "TRANSCLASS", ddl_SALARY_ACCOUNT_BANK.SelectedValue, "","Y");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRANSCLASS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SD1200Txt_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SD1200DAO fb2sd = new CFB2SD1200DAO();
            string Bank_Id = ddl_SALARY_ACCOUNT_BANK.SelectedValue;
            //檢查資料
            //string msg = "0";
            string aa = ddl_TRANSCLASS.SelectedItem.Text;
            string Remark = txt_REMARK.Text;//中信使用  -中文+英數字 總長度要在14 Byte
            if (System.Text.Encoding.Default.GetBytes(txt_REMARK.Text).Length > 14)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('自訂附言長度超出14 Byte !!');<", true);
                return;
            }
            logger.Error(System.Text.Encoding.Default.GetBytes(txt_REMARK.Text).Length);
            aa = aa.Substring(2, 7);//未來要看中信是幾碼長度
            DataTable dt = fb2sd.TxtFirstLine(txt_REMIT_DT.Text, txt_PAY_KIND.Text, Bank_Id);
            if (dt.Rows.Count > 0 && Convert.ToInt16(dt.Rows[0]["CNT"].ToString()) > 0)
            {
                MemoryStream fileStream = null;
                string session_name = "";
                if (Bank_Id == "050")//台企銀
                {
                    fileStream = service.getTxtData(dt, fb2sd, txt_REMIT_DT.Text, txt_PAY_KIND.Text, aa, ddl_TRANSCLASS.SelectedValue, txt_PAY_ID.Text);
                    session_name = "sd120";
                }

                if (Bank_Id == "822")//中信銀
                {
                    fileStream = service.getTxtData_2(dt, fb2sd, txt_REMIT_DT.Text, txt_PAY_KIND.Text, aa, ddl_TRANSCLASS.SelectedValue, Bank_Id, Remark, txt_PAY_ID.Text);
                    session_name = "sd120_1";
                }

                
                Session["fileStream_SD120"] = fileStream;
                dwnframe.Attributes["src"] = "WFB2SD1200_Qry.aspx?FileType_SD120 = " + session_name;
                Session["FileType_SD120"] = session_name;
                if (fileStream != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }

            }
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無匯出資料!');", true);
                //json.errMsg = "無匯出資料!";
            //}
                
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    public void exportTXT()
    {
        try
        {
            if (Session["FileType_SD120"] != null && Session["FileType_SD120"].ToString() != "")
            {
                string FileType_SD120 = Session["FileType_SD120"].ToString();
                if (FileType_SD120 == "sd120")
                {
                    MemoryStream fileStream = (MemoryStream)Session["fileStream_SD120"];
                    Session["FileType_SD120"] = "";
                    Session["fileStream_SD120"] = null;

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("WAGE"));
                    System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    fileStream.Close();
                    fileStream.Dispose();
                    System.Web.HttpContext.Current.Response.End();
                }

                if (FileType_SD120 == "sd120_1")
                {
                    MemoryStream fileStream = (MemoryStream)Session["fileStream_SD120"];
                    Session["FileType_SD120"] = "";
                    Session["fileStream_SD120"] = null;

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("WAGE_822"));
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

    protected void txt_PAY_KIND_TextChanged(object sender, EventArgs e)
    {
        CFB2SD1200DAO dao = new CFB2SD1200DAO();
        string pay_kind = txt_PAY_KIND.Text;
        if (!string.IsNullOrEmpty(pay_kind))
        {
            DataTable dt = dao.paykind(pay_kind);
            if (dt.Rows.Count == 1)
            {
                txt_SALARY_NAME.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
            }
            else
            {
                txt_PAY_KIND.Text = "";
                txt_SALARY_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PAY_KINDerror", "alert('輸入薪資項目錯誤!');", true);
            }
        }
        else
        {
            txt_SALARY_NAME.Text = "";
        }
    }
}