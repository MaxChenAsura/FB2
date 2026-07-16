using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SP0400_Qry : BasePage
{
    CFB2SP0400BO bo = new CFB2SP0400BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //預設值
            getInitData();
           

        }
    }

    #region DB資料取得

    //取得查詢條件的資料及預設值
    private void getInitData()
    {
        try
        {
            //計算類別
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("SP","COMPUTER_TYPE", "", "");
            if (dt.Rows.Count > 0 && SessionHandle.Current.is_super == "Y")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPUTER_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            else {
                //若非擔當
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["sub_cd"].ToString() == "B") {
                        continue;
                    }
                    ddl_COMPUTER_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
                COMMGEOBO service = new COMMGEOBO();
                dt = service.getEMPFile(SessionHandle.Current.emp_id);
                if (dt.Rows.Count > 0)
                {
                    txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                    txt_EMP_ID.Enabled = false;
                    txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    //txt_RETIRE_DT.Text =dt.Rows[0]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    txt_RETIRE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
                    hid_defalut_EMP_NAME.Value = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    hid_defalut_RETIRE_DT.Value = dt.Rows[0]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                }

            }
            //委任經理人
            //ddl_DELEGATE_YN.Items.Add(new ListItem("N-否", "N"));
            //ddl_DELEGATE_YN.Items.Add(new ListItem("Y-是", "Y"));
            //ddl_DELEGATE_YN.SelectedValue = "N";

            hid_sys_date.Value = DateTime.Now.ToString("yyyy/MM/dd");
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
  


    #endregion


    
    //執行
    protected void WFB2SP0400Execute_Click(object sender, EventArgs e)
    {
        try
        {
            //資料檢核
            //if (ddl_DELEGATE_YN.Text == "Y" && string.IsNullOrEmpty(txt_DELEGATE_DT.Text))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('委任經理人,委任日不允空白!');", true);
            //    return;
            //}
            CFB2SP0400DAO dao = new CFB2SP0400DAO();
            dao.COMPUTER_TYPE = ddl_COMPUTER_TYPE.SelectedValue;
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.RETIRE_DT = txt_RETIRE_DT.Text;
            dao.SPECIAL_PAY = txt_SPECIAL_PAY.Text == "" ? "0" : txt_SPECIAL_PAY.Text;
            dao.OTHER_PAY = txt_OTHER_PAY.Text == "" ? "0" : txt_OTHER_PAY.Text;
            dao.STOP_YY = txt_STOP_YY.Text == "" ? "0" : txt_STOP_YY.Text;
            dao.STOP_MM = txt_STOP_MM.Text == "" ? "0" : txt_STOP_MM.Text;
            dao.STOP_DD = txt_STOP_DD.Text == "" ? "0" : txt_STOP_DD.Text;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            string msg = bo.execute(dao);

            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
                // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                showMessage("executeSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //退休金計算類型的連動
    protected void ddl_COMPUTER_TYPE_TextChanged(object sender, EventArgs e)
    {
        string computerType = ddl_COMPUTER_TYPE.SelectedValue;
        if (computerType == "A")
        {
            txt_RETIRE_DT.Text = hid_sys_date.Value;
        }
        else {
            txt_RETIRE_DT.Text = hid_sys_RETIRE_DT.Value;
        }

    }
}