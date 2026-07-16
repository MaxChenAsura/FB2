using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SA1600_Add : BasePage
{
    CFB2SA1600BO sa160BO = new CFB2SA1600BO();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txt_END_DT.Text = "9999/12/31";
            initialValue();
        }


    }
    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
          

            //類別
            dt = utilities.getCommCode("SA", "HIRE_TYPE", "", "", "Y");
            ddl_HIRE_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_HIRE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //薪資項目
            dt = sa160BO.getAllSALARY_ID();
            ddl_SALARY_ID.Items.Clear();
            ddl_SALARY_ID.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_ID.Items.Add(new ListItem(dt.Rows[i]["SALARY_ID"].ToString() + "-" + dt.Rows[i]["SALARY_NAME"].ToString(), dt.Rows[i]["SALARY_ID"].ToString()));
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
        hashtable_set("SA1600_Is_Search", "Y");
        Response.Redirect("WFB2SA1600_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SA1600Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
            sa160DAO.PJOB_CD = txt_PJOB_CD.Text.ToUpper();
            sa160DAO.PAY = txt_PAY.Text.Replace(",","");
            sa160DAO.START_DT = txt_START_DT.Text;
            sa160DAO.END_DT = txt_END_DT.Text;
            sa160DAO.SALARY_ID = ddl_SALARY_ID.SelectedValue;
            sa160DAO.HIRE_TYPE = ddl_HIRE_TYPE.SelectedValue;
            sa160DAO.REMARK = txt_REMARK.Text;
            sa160DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.FUNC_ID = "FB2SA160";

            string msg = "";

            msg = sa160BO.addSave(sa160DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SA1600_Is_Search", "Y");
                showMessage("addSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SA1600_Qry.aspx';</script>";
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