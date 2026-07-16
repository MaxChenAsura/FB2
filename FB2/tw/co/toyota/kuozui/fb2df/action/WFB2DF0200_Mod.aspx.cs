using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DF_WFB2DF0200_Mod : BasePage
{
    string mod = "";
    string emp_id = "";
    //Service 物件
    private CFB2DF0200BO service = new CFB2DF0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        mod = Request.QueryString["mod"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {
            
            //產生宿舍下拉式選單
            createAccom();
            //產生住宿費基準檔下拉選單
            createBase();
            if (mod == "mod")
            {
                //產生修改資料
                getDate();
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(txt_START_DT, this.GetType(), "init", "initForm();", true);
    }

    private void createBase()
    {
        try
        {
            DataTable amount = new DataTable();
            amount = service.getAMOUNT();
            ddl_AMOUNT.Items.Clear();
            ddl_AMOUNT.Items.Add(new ListItem("", "-1"));
            if (amount.Rows.Count > 0)
            {
                for (int i = 0; i < amount.Rows.Count; i++)
                {
                    ddl_AMOUNT.Items.Add(new ListItem(amount.Rows[i]["BASE_NAME"].ToString(), amount.Rows[i]["BASE_NO"].ToString()));
                }
            }
            else
                ddl_AMOUNT.Items.Add(new ListItem("0", "0"));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取得修改資料
    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getData(emp_id);
            
            if (dt.Rows.Count > 0)
            {
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                if (mod == "mod")
                    txt_EMP_ID.Enabled = false;
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_EMP_CD.Text = dt.Rows[0]["EMP_CD"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_WORK_SHIFT_CD.Text = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_JOIN_DT.Text = dt.Rows[0]["JOIN_DT"].ToString();
                txt_REGISTER_ADDR.Text = dt.Rows[0]["REGISTER_ADDR"].ToString();
                txt_CONTACT_ADDR.Text = dt.Rows[0]["CONTACT_ADDR"].ToString();
                txt_MOBILE_TEL_1.Text = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                txt_CONTACT_TEL.Text = dt.Rows[0]["CONTACT_TEL"].ToString();
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                ddl_ACCOM.SelectedValue = dt.Rows[0]["ACCOM_CD"].ToString();
                ddl_ACCOM_SelectedIndexChanged(null, null);
                ddl_ACCOM_BUILDING.SelectedValue = dt.Rows[0]["ACCOM_BUILD_CD"].ToString();
                txt_ROOM_NO.Text = dt.Rows[0]["ROOM_NO"].ToString();
                ddl_AMOUNT.SelectedValue = dt.Rows[0]["BASE_NO"].ToString() + "-" + dt.Rows[0]["AMOUNT"].ToString();
                lb_AMOUNT.Text = dt.Rows[0]["AMOUNT"].ToString();
                hid_AMOUNT.Value = dt.Rows[0]["AMOUNT"].ToString();
                txt_OTHER_AMOUNT.Text = dt.Rows[0]["OTHER_AMOUNT"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                txt_MOTOR_NO.Text = dt.Rows[0]["MOTOR_NO"].ToString();
                txt_CAR_NO.Text = dt.Rows[0]["CAR_NO"].ToString();
                ddl_public_car.SelectedValue = dt.Rows[0]["BUS_CD"].ToString();

                //if (mod == "mod")
                //    txt_START_DT.Enabled = false;               
                
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createAccom()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DF", "ACCOM_CD", "", "");
            ddl_ACCOM.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACCOM.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_ACCOM_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕
    protected void WFB2DF0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            wfb2df.EMP_ID = txt_EMP_ID.Text;
            wfb2df.EMP_NAME = txt_EMP_NAME.Text;
            wfb2df.START_DT = txt_START_DT.Text;
            wfb2df.END_DT = txt_END_DT.Text;
            wfb2df.ACCOM_CD = ddl_ACCOM.SelectedValue;
            wfb2df.ACCOM_BUILD_CD = ddl_ACCOM_BUILDING.SelectedValue;
            wfb2df.ROOM_NO = txt_ROOM_NO.Text;
            wfb2df.AMOUNT = hid_AMOUNT.Value;
            wfb2df.BASE_NO = ddl_AMOUNT.SelectedValue.Split('-').First();
            wfb2df.OTHER_AMOUNT = txt_OTHER_AMOUNT.Text.Replace(",", "");
            wfb2df.REMARK = txt_REMARK.Text;
            wfb2df.MOTOR_NO = txt_MOTOR_NO.Text;
            wfb2df.CAR_NO = txt_CAR_NO.Text;
            wfb2df.BUS_CD = ddl_public_car.SelectedValue;
            wfb2df.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2df.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2df.FUNC_ID = "FB2DF020";

            string msg = service.saveACCOM_MAIN(wfb2df,mod);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n","");
                msg = msg.Replace("'", "");
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                if (mod == "mod")
                {
                    Session["DF0200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('修改成功!');$(location).attr('href','WFB2DF0200_Qry.aspx');", true);
                    //showMessage("modSuccessMessage");
                }
                else
                {
                    Session["DF0200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('新增成功!');$(location).attr('href','WFB2DF0200_Qry.aspx');", true);
                    //    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
                }
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //宿舍別選擇後查詢宿舍棟別
    protected void ddl_ACCOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DF", "ACCOM_BUILD_CD", ddl_ACCOM.SelectedValue + " (宿舍別)", "");
            ddl_ACCOM_BUILDING.Items.Clear();
            ddl_ACCOM_BUILDING.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACCOM_BUILDING.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            string code_val1 = service.getCode_Val(ddl_ACCOM.SelectedValue);
            hid_CODE_VAL1.Value = code_val1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DF0200_Is_Search"] = "Y";
        Response.Redirect("WFB2DF0200_Qry.aspx");
    }
}