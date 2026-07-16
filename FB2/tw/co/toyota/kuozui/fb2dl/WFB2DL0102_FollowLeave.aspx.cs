using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dl_WFB2DL0102_FollowLeave : BasePage
{
    //Service 物件
    private CFB2DL0100BO dl010BO = new CFB2DL0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_Year.Text = DateTime.Now.ToString("yyyy");
            getQryItem();
        }
    }

    //取得生成子假別
    private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dl010BO.getSub_Leave_CD();
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", ""));//加個空白的預設值(text='',value='')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //執行
    protected void WFB2DL0102Save_Click(object sender, EventArgs e)
    {
        try
        {
            string year = txt_Year.Text;
            string sub_leave_cd = ddl_SUB_LEAVE_CD.SelectedValue;
            string msg = "";
            msg = dl010BO.executeFollowLeave(year, sub_leave_cd);
            if (msg != "0")
            {
                showMessage("executeFailMessage", "//n" + msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                showMessage("executeSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0100_Qry.aspx");
    }
}