using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0300_Add : BasePage
{
    //Service 物件
    private WFB2DA0300BO service = new WFB2DA0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);

        if (!IsPostBack)
        {
            //產生行事曆下拉式選單
            createCALENDAR_CD();
            //產生日期類型(新) 下拉式選單
            createDT_TYPE();

        }
    }

    #region 下拉選單設定
    private void createCALENDAR_CD()
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            DataTable dt = dao.get_CALENDAR_CD_Data();
            ddl_CALENDAR_CD.Items.Clear();
            ddl_CALENDAR_CD.Items.Add(new ListItem("", "-1"));
            ddl_CALENDAR_CD.Items.Add(new ListItem("All-全部行事曆", "All"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CALENDAR_CD.Items.Add(new ListItem(dt.Rows[i]["CALENDAR_DESC"].ToString(), dt.Rows[i]["CALENDAR_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_CALENDAR_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createDT_TYPE()
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            DataTable dt = utilities.getCommCode("DA", "DT_TYPE", "", "");
            ddl_DT_TYPE_N.Items.Clear();
            ddl_DT_TYPE_N.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DT_TYPE_N.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    //儲存按鈕事件
    protected void WFF2DA0301Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            //日期需>已薪資月結 -1 月月底[dbo.FN_S_DUTY_EDT('1M')]																																															
            DateTime s_duty_edt = service.getFN_S_DUTY_EDT();
            //生效日期有起 迄
            DateTime start_dt = Convert.ToDateTime(txt_CALENDAR_DT.Text);
            if (s_duty_edt > start_dt)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('日期需大於已薪資月結前個月月底');", true);
                return;
            }

            //(2) 若日期類型(原) = 日期類型(新)																							
            //顯示訊息提示視窗「新舊日期類型不可一樣!」。																					
            string dt_type_o = txt_DT_TYPE_O.Text;
            string dt_type_n = ddl_DT_TYPE_N.SelectedValue;
            if (dt_type_o == dt_type_n)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('新舊日期類型不可一樣!');", true);
                return;
            }

            //(1)檢核   TB_D_M_CALENDAR_ADJ(行事曆調整檔) PK值是否已存在?																																
            //若已存在，顯示訊息提示視窗「行事曆 + 群組代碼+日期起 已存在」。
            dao.CALENDAR_CD = ddl_CALENDAR_CD.SelectedValue;
            dao.CALENDAR_DT = txt_CALENDAR_DT.Text;
            dao.DT_TYPE_O = txt_DT_TYPE_O.Text;
            dao.DT_TYPE_N = ddl_DT_TYPE_N.SelectedValue;
            //msg = service.getTB_D_M_CALENDAR_ADJ(dao);
            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('行事曆+日期+日期類型(原) 已存在!');", true);
            //    return;
            //}

            msg = service.addData(dao);
            if (msg == "0")
            {
                Session["DA0300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('新增成功!!');location.href='WFB2DA0300_Qry.aspx'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');$.unblockUI();", true);
        }
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DA0300_Is_Search"] = "Y";
            Response.Redirect("WFB2DA0300_Qry.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    protected void txt_CALENDAR_DT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_CALENDAR_CD.SelectedValue == "-1")
            {
                return;
            }
            if (ddl_CALENDAR_CD.SelectedValue == "All")
            {
                txt_DT_TYPE_O.Text = "";
            }
            else
            {
                txt_DT_TYPE_O.Text = service.getCALENDAR_DT(ddl_CALENDAR_CD.SelectedValue, txt_CALENDAR_DT.Text);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void ddl_CALENDAR_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_CALENDAR_CD.SelectedValue == "-1")
            {
                return;
            }
            if (ddl_CALENDAR_CD.SelectedValue == "All")
            {
                txt_DT_TYPE_O.Text = "";
            }
            else
            {
                txt_DT_TYPE_O.Text = service.getCALENDAR_DT(ddl_CALENDAR_CD.SelectedValue, txt_CALENDAR_DT.Text);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}