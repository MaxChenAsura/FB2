using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0120_Upd : BasePage
{
    CFB2SJ0120BO sj0120BO = new CFB2SJ0120BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;

        if (!IsPostBack)
        {
            initialValue();
        }


    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ0120DAO sj0120DAO = new CFB2SJ0120DAO();
            sj0120DAO.DISTING_CD = hashtable_get("SJ0120_UPD_DISTING_CD").ToString();
            logger.Debug(sj0120DAO.DISTING_CD);

            DataTable dt = new DataTable();
            //外數區分
            ddl_IS_OUT.Items.Add(new ListItem("", "-1"));
            ddl_IS_OUT.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_OUT.Items.Add(new ListItem("N", "N"));

            //備考對象
            ddl_IS_REMARK.Items.Add(new ListItem("", "-1"));
            ddl_IS_REMARK.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_REMARK.Items.Add(new ListItem("N", "N"));

            //是否生效
            ddl_IS_VALID.Items.Add(new ListItem("", "-1"));
            ddl_IS_VALID.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_VALID.Items.Add(new ListItem("N", "N"));

            //基本資料
            dt = sj0120BO.getUpdData(sj0120DAO);

            if (dt.Rows.Count > 0)
            {
                txt_DISTING_CD.Text = dt.Rows[0]["DISTING_CD"].ToString();
                hid_DISTING_CD.Value = dt.Rows[0]["DISTING_CD"].ToString();
                txt_DISTING_DESC.Text = dt.Rows[0]["DISTING_DESC"].ToString();
                ddl_IS_OUT.SelectedValue = dt.Rows[0]["IS_OUT"].ToString();
                ddl_IS_REMARK.SelectedValue = dt.Rows[0]["IS_REMARK"].ToString();
                ddl_IS_VALID.SelectedValue = dt.Rows[0]["IS_VALID"].ToString();
                txt_CONTENT.Text = dt.Rows[0]["CONTENT"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                hid_USER_UP_YN.Value = dt.Rows[0]["USER_UP_YN"].ToString();
                if (dt.Rows[0]["User_UP_YN"].ToString() == "N")
                {
                    txt_DISTING_DESC.Enabled = false;
                    ddl_IS_OUT.Enabled = false;
                    ddl_IS_REMARK.Enabled = false;
                    txt_CONTENT.Enabled = false;
                    txt_REMARK.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

  

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0120_Is_Search", "Y");
        Response.Redirect("WFB2SJ0120_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0120Save_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SJ0120DAO sj0120DAO = new CFB2SJ0120DAO();
            sj0120DAO.DISTING_CD = hid_DISTING_CD.Value.ToUpper();
            sj0120DAO.DISTING_DESC = txt_DISTING_DESC.Text.Replace(",", "");
            sj0120DAO.IS_OUT = ddl_IS_OUT.SelectedValue;
            sj0120DAO.IS_REMARK = ddl_IS_REMARK.SelectedValue;
            sj0120DAO.IS_VALID = ddl_IS_VALID.SelectedValue;
            sj0120DAO.REMARK = txt_REMARK.Text;
            sj0120DAO.USER_UP_YN = hid_USER_UP_YN.Value;
            sj0120DAO.CONTENT = txt_CONTENT.Text;
            sj0120DAO.REMARK = txt_REMARK.Text;
            sj0120DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0120DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0120DAO.FUNC_ID = "FB2SJ0120";

            string msg = "";

            msg = sj0120BO.updateDISTING(sj0120DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0120_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0120_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }
             

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}