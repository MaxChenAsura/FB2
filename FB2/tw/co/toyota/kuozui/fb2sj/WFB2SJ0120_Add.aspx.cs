using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0120_Add : BasePage
{
    CFB2SJ0120BO sj0120BO = new CFB2SJ0120BO();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //txt_END_DT.Text = "9999/12/31";
            initialValue();
        }


    }
    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //
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
            sj0120DAO.DISTING_CD = txt_DISTING_CD.Text.ToUpper();
            sj0120DAO.DISTING_DESC = txt_DISTING_DESC.Text;
            sj0120DAO.IS_OUT = ddl_IS_OUT.SelectedValue;
            sj0120DAO.IS_REMARK = ddl_IS_REMARK.SelectedValue;
            sj0120DAO.IS_VALID = ddl_IS_VALID.SelectedValue;
            sj0120DAO.USER_UP_YN = "Y";
            sj0120DAO.CONTENT = txt_CONTENT.Text;
            sj0120DAO.REMARK = txt_REMARK.Text;
            sj0120DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0120DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0120DAO.FUNC_ID = "FB2SJ0120";

            string msg = "";

            msg = sj0120BO.addDISTING(sj0120DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0120_Is_Search", "Y");
                showMessage("addSuccessMessage");
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