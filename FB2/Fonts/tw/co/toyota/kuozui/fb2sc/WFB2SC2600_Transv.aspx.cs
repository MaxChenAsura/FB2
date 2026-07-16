using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2600_Transv : BasePage
{
    CFB2SC2600BO service = new CFB2SC2600BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //查詢明細畫面-表頭資料
            txt_SALARY_TYPE.Value = Request.QueryString["SALARY_TYPE"];
            txt_SALARY_TYPE_NAME.Text = Request.QueryString["SALARY_TYPE_NAME"];
            txt_SALARY_DT.Text = Request.QueryString["START_DT"];
            txt_PAY_KIND.Value = Request.QueryString["PAY_KIND"];
            txt_PAY_KIND_NAME.Text = Request.QueryString["PAY_KIND_NAME"];
            txt_PROCESS_STATUS_NAME.Text = Request.QueryString["PROCESS_STATUS_NAME"];
            txt_PAY_ID.Text = Request.QueryString["PAY_ID"];
            txt_PAY_DT.Text = Request.QueryString["PAY_DT"];
            txt_SALARY_YM.Value = Request.QueryString["SALARY_YM"];
            txt_IACYC.Text = Request.QueryString["IACYC"];            

            #region 產生查詢下拉選單
            createCOMPANY_CD1();
            createCOMPANY_CD2();
            create_OTHER_REMIT_DT();
            create_INV_TYPE11();
            //create_INV_TYPE12();
            create_INV_TYPE21();
            //create_INV_TYPE22();
            #endregion
        }
    }
    #region 下拉式選單內容製作
    private void createCOMPANY_CD1()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCompany("COMPANY_CD<>'K'");
            ddl_COMPANY_CD1.Items.Clear();
            ddl_COMPANY_CD1.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD1.Items.Add(new ListItem(dt.Rows[i]["CODE_NAME"].ToString(), dt.Rows[i]["CODE"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_COMPANY_CD1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createCOMPANY_CD2()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCompany("COMPANY_CD<>'K'");
            ddl_COMPANY_CD2.Items.Clear();
            ddl_COMPANY_CD2.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD2.Items.Add(new ListItem(dt.Rows[i]["CODE_NAME"].ToString(), dt.Rows[i]["CODE"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_COMPANY_CD2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void create_OTHER_REMIT_DT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SC", "TMC_ACCOUNT_NO", "","","Y");
            ddl_TMC_PAY_TYPE.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TMC_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_TMC_PAY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void create_INV_TYPE11()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SC", "INV_TYPE", "21", "", "Y");
            ddl_INV_TYPE11.Items.Clear();
            ddl_INV_TYPE11.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INV_TYPE11.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INV_TYPE11, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
    private void create_INV_TYPE21()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SC", "INV_TYPE", "21", "", "Y");
            ddl_INV_TYPE21.Items.Clear();
            ddl_INV_TYPE21.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INV_TYPE21.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INV_TYPE21, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
  
   #endregion

    protected void btn_return_back_Click(object sender, EventArgs e)
    {
        Session["SC2600_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2600_Qry.aspx");
    }

    //結轉傳票
    protected void WFB2SC2600Execute1_Click(object sender, EventArgs e)
    {
        try
        {
            bool successed = true;
            #region 檢查
            string msg = "";
            if (txt_SALARY_TYPE.Value == "A")
            {
                if (ddl_COMPANY_CD1.SelectedValue == "")
                {
                    msg = "發薪類別為A月薪資時,第一家聘用公司不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
            }
            if (ddl_COMPANY_CD1.SelectedValue != "")
            {
                if (ddl_INV_TYPE11.SelectedValue == "")
                {
                    msg = "第一家:聘用公司不為空白,支付發票格式不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                if (txt_INV_NO11.Text == "")
                {
                    msg = "第一家:聘用公司不為空白,支付發票號碼不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                if (txt_INV_DT11.Text == "")
                {
                    msg = "第一家:聘用公司不為空白,支付發票日期不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                
            }
            if (ddl_COMPANY_CD2.SelectedValue != "")
            {
                if (ddl_INV_TYPE21.SelectedValue == "")
                {
                    msg = "第二家:聘用公司不為空白,支付發票格式不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                if (txt_INV_NO21.Text == "")
                {
                    msg = "第二家:聘用公司不為空白,支付發票號碼不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                if (txt_INV_DT21.Text == "")
                {
                    msg = "第二家:聘用公司不為空白,支付發票日期不允空白!!";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
               
            }
            #endregion
            //-- @salary_dt 發薪日期,@salary_type 發薪類別,@pay_kind 發放項目,@pay_id 關帳代號
//--@tmc_pay_type TMC付款,@other_remit_dt (媒體轉帳對象外) 實際匯款日, @salary_ym 薪資年月
//--@company_cd1 聘用公司1,@invno11 支付發票號碼1,@invtpe11 支付發票格式1,@intdt11 支付發票號日期,@invno12 收入發票號碼1,@invtype12 收入發票格式1,@intdt12 收入發票號日期
//--@company_cd2 聘用公司2,invno21 支付發票號碼21,@invtype21 支付發票格式21,@intdt21 支付發票號日期,@invno22 收入發票號碼2,@invtype22 收入發票格式2,@intdt22 收入發票號日期2
            //20200827 測試時註解
            successed = service.MarkVouch(txt_SALARY_DT.Text, txt_SALARY_TYPE.Value, txt_PAY_KIND.Value, txt_PAY_ID.Text,ddl_TMC_PAY_TYPE.SelectedValue,txt_OTHER_REMIT_DT.Text,
                                         txt_SALARY_YM.Value,ddl_COMPANY_CD1.SelectedValue,txt_INV_NO11.Text,ddl_INV_TYPE11.SelectedValue,txt_INV_DT11.Text
                                         , ddl_COMPANY_CD2.SelectedValue, txt_INV_NO21.Text, ddl_INV_TYPE21.SelectedValue, txt_INV_DT21.Text, txt_IaDat.Text);
            
            msg = service.SP_S_SC2600_VOUCHER_SAP0(txt_PAY_ID.Text);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('結轉傳票(SAP)失敗," + msg + "');$.unblockUI();", true);
                return;
            }
            
            if (successed)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.blockUI();alert('結轉傳票作業完成');$(location).attr('href','WFB2SC2600_Qry.aspx');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('結轉傳票作業失敗!');$.unblockUI();", true);
            }

           

            return;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');$.unblockUI();", true);
        }
    }

}