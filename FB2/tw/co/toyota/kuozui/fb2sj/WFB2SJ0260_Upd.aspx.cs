using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0260_Upd : BasePage
{
    CFB2SJ0260BO sj0260BO = new CFB2SJ0260BO();
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
           
           
            hid_ASSESS_YEAR.Value = hashtable_get("SJ0260_UPD_ASSESS_YEAR").ToString();
            hid_ASSESS_TYPE.Value = hashtable_get("SJ0260_UPD_ASSESS_TYPE").ToString();
            txt_EMP_ID.Text = hashtable_get("SJ0260_UPD_EMP_ID").ToString();
            //txt_DEPT_NO_OLD.Text = hashtable_get("SJ0260_UPD_DEPT_NO_OLD").ToString();
                //hid_CREATED_BY.Value = SessionHandle.Current.emp_id;
                //hid_CREATED_BY.Value = "14232";
            //CFB2SJ0500DAO sj0500Dao = new CFB2SJ0500DAO();
            //sj0500Dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            //sj0500Dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            CFB2SJ0260DAO daoObj = new CFB2SJ0260DAO();
            daoObj.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            daoObj.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            daoObj.EMP_ID = txt_EMP_ID.Text;
            //daoObj.DEPT_NO_OLD = txt_DEPT_NO_OLD.Text;
            dt = daoObj.getUpdData();
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_SURE_YN_DESC.Text = dt.Rows[0]["SURE_YN_DESC"].ToString(); 
                txt_DEPT_NO_OLD.Text = dt.Rows[0]["DEPT_NO_OLD"].ToString();
                txt_DEPT_NAME_OLD.Text = dt.Rows[0]["DEPT_NAME_OLD"].ToString();
                txt_HEAD_EMP_NAME_OLD.Text = dt.Rows[0]["HEAD_EMP_NAME_OLD"].ToString();
                hid_HEAD_EMP_ID_OLD.Value = dt.Rows[0]["HEAD_EMP_ID_OLD"].ToString();
                txt_DEPT_NO_NEW.Text = dt.Rows[0]["DEPT_NO_NEW"].ToString();
                txt_DEPT_NAME_NEW.Text = dt.Rows[0]["DEPT_NAME_NEW"].ToString();
                txt_HEAD_EMP_NAME_NEW.Text = dt.Rows[0]["HEAD_EMP_NAME_NEW"].ToString();
                hid_HEAD_EMP_ID_NEW.Value = dt.Rows[0]["HEAD_EMP_ID_NEW"].ToString();
                hid_SURE_YN.Value = dt.Rows[0]["SURE_YN"].ToString();
                if (hid_SURE_YN.Value == "Y")
                {
                    txt_DEPT_NO_NEW.Enabled = false;
                   
                    WFB2SJ0260Confirm.Visible = false;
                }
                WFB2SJ0260Save_U.Visible = false;
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
        hashtable_set("SJ0260_Is_Search", "Y");
        Response.Redirect("WFB2SJ0260_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0260Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
           //檢核:檢查新直屬主管 不可等於.原直屬主管。
           
            if (hid_HEAD_EMP_ID_NEW.Value == hid_HEAD_EMP_ID_OLD.Value)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('新直屬主管 不可等於.原直屬主管');", true);
                return;
            }
            CFB2SJ0260DAO sj0260DAO = new CFB2SJ0260DAO();

            sj0260DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0260DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0260DAO.EMP_ID = txt_EMP_ID.Text; 
            sj0260DAO.DEPT_NO_OLD = txt_DEPT_NO_OLD.Text;
            sj0260DAO.DEPT_NAME_OLD = txt_DEPT_NAME_OLD.Text;
            sj0260DAO.HEAD_EMP_ID_OLD = hid_HEAD_EMP_ID_OLD.Value;            
            sj0260DAO.DEPT_NO_NEW = txt_DEPT_NO_NEW.Text;
            sj0260DAO.DEPT_NAME_NEW = txt_DEPT_NAME_NEW.Text;
            sj0260DAO.HEAD_EMP_ID_NEW = hid_HEAD_EMP_ID_NEW.Value;
            sj0260DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0260DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0260DAO.FUNC_ID = "FB2SJ0260";

            
           
             msg = sj0260BO.updateEMP_CHG(sj0260DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0260_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0260_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SJ0260Confirm_Click(object sender, EventArgs e)
    {
        try{
            CFB2SJ0260DAO sj0260DAO = new CFB2SJ0260DAO();

            sj0260DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0260DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0260DAO.EMP_ID = txt_EMP_ID.Text;
            sj0260DAO.DEPT_NO_NEW = txt_DEPT_NO_NEW.Text;
            sj0260DAO.DEPT_NO_OLD = txt_DEPT_NO_OLD.Text;
            sj0260DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0260DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0260DAO.FUNC_ID = "FB2SJ0260";

            string msg="";
           
             msg = sj0260BO.confirmEMP_CHG(sj0260DAO);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0260_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0260_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void doDeptNoChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_DEPT_NO_NEW.Text == "") return;
            CFB2SJ0260DAO sj0260Dao = new CFB2SJ0260DAO();
            sj0260Dao.DEPT_NO = txt_DEPT_NO_NEW.Text;

            DataTable dt = sj0260Dao.getDeptData();
            if (dt.Rows.Count > 0)
            {

                txt_DEPT_NO_NEW.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME_NEW.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                txt_HEAD_EMP_NAME_NEW.Text = dt.Rows[0]["HEAD_EMP_NAME"].ToString();
                hid_HEAD_EMP_ID_NEW.Value = dt.Rows[0]["HEAD_EMP_ID"].ToString();
            }
            else
            {
                string msg = "";
                msg = "無部門相關資料!!";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    } 
}