using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ3100_Add : BasePage
{
    CFB2SJ3100BO sj3100BO = new CFB2SJ3100BO();
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
        hashtable_set("SJ3100_Is_Search", "Y");
        Response.Redirect("WFB2SJ3100_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ3100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SJ3100DAO sj3100DAO = new CFB2SJ3100DAO();
            sj3100DAO.DISTING_CD = txt_DISTING_CD.Text.ToUpper();
            sj3100DAO.DISTING_DESC = txt_DISTING_DESC.Text;
            sj3100DAO.IS_OUT = ddl_IS_OUT.SelectedValue;
            sj3100DAO.IS_REMARK = ddl_IS_REMARK.SelectedValue;
            sj3100DAO.IS_VALID = ddl_IS_VALID.SelectedValue;
            sj3100DAO.USER_UP_YN = "Y";
            sj3100DAO.CONTENT = txt_CONTENT.Text;
            sj3100DAO.REMARK = txt_REMARK.Text;
            sj3100DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj3100DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj3100DAO.FUNC_ID = "FB2SJ3100";

            string msg = "";

            msg = sj3100BO.addITEM(sj3100DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ3100_Is_Search", "Y");
                showMessage("addSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ3100_Qry.aspx';</script>";
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