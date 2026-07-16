using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0110_Add : BasePage
{
    CFB2SJ0110BO sj0110BO = new CFB2SJ0110BO();
    CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
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
            //考核類型
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //職種
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            dt = sj0150BO.getLevelData();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
            //職務類型
            ddl_PJOB_TYPE.Items.Add(new ListItem("", "-1"));
            dt = utilities.getCommCode("SE", "PJOB_TYPE", "", "");
            ddl_PJOB_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PJOB_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
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
        hashtable_set("SJ0110_Is_Search", "Y");
        Response.Redirect("WFB2SJ0110_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0110Save_Click(object sender, EventArgs e)
    {
        try
        {
           
            CFB2SJ0110DAO sj0110DAO = new CFB2SJ0110DAO();
            sj0110DAO.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
            sj0110DAO.WS_CD = ddl_WS_CD.SelectedValue;
            sj0110DAO.LEVEL_CD = "";
            if (ddl_LEVEL_CD.SelectedValue!="-1")sj0110DAO.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            sj0110DAO.PJOB_TYPE = "";
            if (ddl_PJOB_TYPE.SelectedValue != "-1") sj0110DAO.PJOB_TYPE = ddl_PJOB_TYPE.SelectedValue;
            sj0110DAO.ITEM_GROUP = txt_ITEM_GROUP.Text;
            sj0110DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0110DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0110DAO.FUNC_ID = "FB2SJ0110";

            string msg = "";

            msg = sj0110BO.addITEM(sj0110DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0110_Is_Search", "Y");
                showMessage("addSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0110_Qry.aspx';</script>";
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