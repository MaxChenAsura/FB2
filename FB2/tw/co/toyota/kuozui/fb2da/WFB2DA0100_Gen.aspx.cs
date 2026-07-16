using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100_Gen : BasePage
{
    //Service 物件
    private WFB2DA0100BO service = new WFB2DA0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            txt_START_DATE.Text = DateTime.Now.AddYears(1).ToString("yyyy") + "/01/01";
            txt_END_DATE.Text = DateTime.Now.AddYears(1).ToString("yyyy") + "/12/31";

            //取得行事曆代碼下拉清單  
            getCALENDAR_CD();
        }
    }

    private void getCALENDAR_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCALENDAR_CD();
            ddl_CALENDAR_CD.Items.Add(new ListItem("", "-1"));
            ddl_CALENDAR_CD.Items.Add(new ListItem("All-全部", "All"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CALENDAR_CD.Items.Add(new ListItem(dt.Rows[i]["CALENDAR_DESC"].ToString(), dt.Rows[i]["CALENDAR_CD"].ToString()));
                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    protected void WFB2DA0100GEN_Click(object sender, EventArgs e)
    {
        try
        {
            string result = "";
            WFB2DA0100DAO dao = new WFB2DA0100DAO();
            dao.START_DATE = txt_START_DATE.Text;
            dao.END_DATE = txt_END_DATE.Text;
            dao.CALENDAR_CD = ddl_CALENDAR_CD.SelectedValue;

            result = service.SP_DA010_01(dao);
            if (result != "0")
            {
                //SP記錄檔.處理訊息
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + result.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行成功!');", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DA0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DA0100_Qry.aspx");
    }


}