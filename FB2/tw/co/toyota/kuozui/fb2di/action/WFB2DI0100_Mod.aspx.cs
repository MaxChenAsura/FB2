using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0100_Mod : BasePage
{
    string mod = "";
    string overtime_cd = "";
    string overtime_desc = "";
    string overtime_dt_type = "";
    string overtime_dt_desc = "";

    //Service 物件
    private CFB2DI0100BO service = new CFB2DI0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        mod = Request.QueryString["mod"] == null ? "" : Request.QueryString["mod"].ToString();
        overtime_cd = Request.QueryString["overtime_cd"] == null ? "" : Request.QueryString["overtime_cd"].ToString();
        overtime_desc = Request.QueryString["overtime_desc"] == null ? "" : Request.QueryString["overtime_desc"].ToString();
        overtime_dt_type = Request.QueryString["overtime_dt_type"] == null ? "" : Request.QueryString["overtime_dt_type"].ToString();
        overtime_dt_desc = Request.QueryString["overtime_dt_desc"] == null ? "" : Request.QueryString["overtime_dt_desc"].ToString();

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getInitData();
            if (mod == "mod")
            {
                //產生修改資料
                getDate();
            }

        }

    }

    private void getInitData()
    {
        try
        {
            if (mod == "mod")
            {
                //加班類型
                txt_OVERTIME_CD.Text = overtime_cd; 
                txt_OVERTIME_CD.BorderWidth = 0;
                txt_OVERTIME_CD.ReadOnly = true;
            }
            else
            {
                txt_OVERTIME_CD.MaxLength = 1;
                txt_OVERTIME_CD.CssClass = "MandatoryField";
                //ddl_OVERTIME_DT_TYPE.CssClass = "MandatoryField";
            }

            //產生相關下拉選單
            DataTable dt = new DataTable();
            #region 出勤別
            dt = utilities.getCommCode("DA", "WORK_DAY_CD", "", "");
            ddl_WORK_DAY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_DAY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 加班日期類型
            dt = utilities.getCommCode("DI", "OVERTIME_DT_TYPE", "", "");
            ddl_OVERTIME_DT_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_DT_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 加班計算時數
            dt = service.getO_HOUR_CD();
            ddl_O_HOUR_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_O_HOUR_CD.Items.Add(new ListItem(dt.Rows[i]["O_HOUR_DESC"].ToString(), dt.Rows[i]["O_HOUR_CD"].ToString()));
                }
            }
            #endregion
            #region 加班倍數代碼
            dt = service.getO_MUL_CD();
            ddl_O_MUL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_O_MUL_CD.Items.Add(new ListItem(dt.Rows[i]["O_MUL_DESC"].ToString(), dt.Rows[i]["O_MUL_CD"].ToString()));
                }
            }
            #endregion
            #region 換休/申告
            dt = new DataTable();
            dt = utilities.getCommCode("DI", "OVERTIME_EXCHANGE_CD", "", "");
            ddl_OVERTIME_EXCHANGE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_EXCHANGE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 換休結算週期
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_CD", "", "");
            ddl_SALARY_SETTLE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 使用狀態
            ddl_IS_USED.Items.Add(new ListItem("", "-1"));
            ddl_IS_USED.Items.Add(new ListItem("Y-使用中", "Y"));
            ddl_IS_USED.Items.Add(new ListItem("N-停用", "N"));
            #endregion

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();

            //基本資料
            dt = service.getDefaultData(overtime_cd, overtime_dt_type);
            if (dt.Rows.Count > 0)
            {
                ddl_OVERTIME_DT_TYPE.SelectedValue = overtime_dt_type;
                //ddl_OVERTIME_DT_TYPE.BorderWidth = 0;
                //ddl_OVERTIME_DT_TYPE.Enabled = false;
                txt_OVERTIME_DESC.Text = dt.Rows[0]["OVERTIME_DESC"].ToString();
                ddl_WORK_DAY_CD.SelectedValue = dt.Rows[0]["WORK_DAY_CD"].ToString();
                ddl_O_HOUR_CD.SelectedValue = dt.Rows[0]["O_HOUR_CD"].ToString();
                ddl_O_MUL_CD.SelectedValue = dt.Rows[0]["O_MUL_CD"].ToString();

                if (dt.Rows[0]["HYPER_SHOUR"].ToString() != "" && dt.Rows[0]["HYPER_EHOUR"].ToString() != "" &&
                    (Convert.ToDouble(dt.Rows[0]["HYPER_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["HYPER_EHOUR"]) != 0))
                {
                    txt_HYPER_SHOUR.Text = dt.Rows[0]["HYPER_SHOUR"].ToString();
                    txt_HYPER_EHOUR.Text = dt.Rows[0]["HYPER_EHOUR"].ToString();
                }
                if (dt.Rows[0]["NONTAX_SHOUR"].ToString() != "" && dt.Rows[0]["NONTAX_EHOUR"].ToString() != "" && 
                    (Convert.ToDouble(dt.Rows[0]["NONTAX_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["NONTAX_EHOUR"]) != 0))
                {
                    txt_NONTAX_SHOUR.Text = dt.Rows[0]["NONTAX_SHOUR"].ToString();
                    txt_NONTAX_EHOUR.Text = dt.Rows[0]["NONTAX_EHOUR"].ToString();
                }
                if (dt.Rows[0]["NORMAL_SHOUR"].ToString() != "" && dt.Rows[0]["NORMAL_EHOUR"].ToString() != "" &&
                    (Convert.ToDouble(dt.Rows[0]["NORMAL_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["NORMAL_EHOUR"]) != 0))
                {
                    txt_NORMAL_SHOUR.Text = dt.Rows[0]["NORMAL_SHOUR"].ToString();
                    txt_NORMAL_EHOUR.Text = dt.Rows[0]["NORMAL_EHOUR"].ToString();
                }
                if (dt.Rows[0]["OTHER_SHOUR"].ToString() != "" && dt.Rows[0]["OTHER_EHOUR"].ToString() != "" && 
                    (Convert.ToDouble(dt.Rows[0]["OTHER_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["OTHER_EHOUR"]) != 0))
                {
                    txt_OTHER_SHOUR.Text = dt.Rows[0]["OTHER_SHOUR"].ToString();
                    txt_OTHER_EHOUR.Text = dt.Rows[0]["OTHER_EHOUR"].ToString();
                }
                if (dt.Rows[0]["BASE_SHOUR"].ToString() != "" && dt.Rows[0]["BASE_EHOUR"].ToString() != "" &&
                    (Convert.ToDouble(dt.Rows[0]["BASE_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["BASE_EHOUR"]) != 0))
                {
                    txt_BASE_SHOUR.Text = dt.Rows[0]["BASE_SHOUR"].ToString();
                    txt_BASE_EHOUR.Text = dt.Rows[0]["BASE_EHOUR"].ToString();
                }
                if (dt.Rows[0]["TAX_SHOUR"].ToString() != "" && dt.Rows[0]["TAX_EHOUR"].ToString() != "" && 
                    (Convert.ToDouble(dt.Rows[0]["TAX_SHOUR"]) != 0 || Convert.ToDouble(dt.Rows[0]["TAX_EHOUR"]) != 0))
                {
                    txt_TAX_SHOUR.Text = dt.Rows[0]["TAX_SHOUR"].ToString();
                    txt_TAX_EHOUR.Text = dt.Rows[0]["TAX_EHOUR"].ToString();
                }

                ddl_OVERTIME_EXCHANGE_CD.SelectedValue = dt.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString();
                ddl_SALARY_SETTLE_CD.SelectedValue = dt.Rows[0]["SALARY_SETTLE_CD"].ToString();
                txt_CHG_WORK_CD.Text = dt.Rows[0]["CHG_WORK_CD"].ToString();
                ddl_IS_USED.SelectedValue = dt.Rows[0]["IS_USED"].ToString();
                ddl_IS_IFLOW_SHOW.SelectedValue = dt.Rows[0]["IS_IFLOW_SHOW"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            if (txt_HYPER_EHOUR.Text != "")
            {
                if (Convert.ToDouble(txt_HYPER_EHOUR.Text) < 0 || Convert.ToDouble(txt_HYPER_EHOUR.Text) > 24)
                    errmsg += "三高累計時數需為正整數且介於0~24\\n";
            }

            if (txt_NORMAL_EHOUR.Text != "")
            {
                if (Convert.ToDouble(txt_NORMAL_EHOUR.Text) < 0 || Convert.ToDouble(txt_NORMAL_EHOUR.Text) > 24)
                    errmsg += "一般累計時數需為正整數且介於0~24\\n";
            }

            if (txt_BASE_EHOUR.Text != "")
            {
                if (Convert.ToDouble(txt_BASE_EHOUR.Text) < 0 || Convert.ToDouble(txt_BASE_EHOUR.Text) > 24)
                    errmsg += "加計本薪時數需為正整數且介於0~24\\n";
            }

            if (txt_NONTAX_EHOUR.Text !="")
            {
                if (Convert.ToDouble(txt_NONTAX_EHOUR.Text) > 24)
                    errmsg += "免稅時數迄值不可大於24\\n";
                if (txt_OTHER_SHOUR.Text != "" && (Convert.ToDouble(txt_NONTAX_EHOUR.Text) != Convert.ToDouble(txt_OTHER_SHOUR.Text)))
                    errmsg += "累計時數起值必須等於免稅時數迄值\\n";
            }

            if (txt_OTHER_EHOUR.Text != "")
            {
                if (Convert.ToDouble(txt_OTHER_EHOUR.Text) > 24)
                    errmsg += "累計時數迄值不可大於24\\n";
                if (txt_TAX_SHOUR.Text != "" && (Convert.ToDouble(txt_OTHER_EHOUR.Text) != Convert.ToDouble(txt_TAX_SHOUR.Text)))
                    errmsg += "應稅時數起值必須等於累計時數迄值\\n";
            }

            if (txt_TAX_EHOUR.Text != "")
            {
                if (Convert.ToDouble(txt_TAX_EHOUR.Text) > 24)
                    errmsg += "應稅時數迄值不可大於24\\n";
            }
            //換休對象需存在共用代碼檔
            if (txt_CHG_WORK_CD.Text != "")
            {
                string result = service.getCHG_WORK_CD(txt_CHG_WORK_CD.Text);
                if (result != "0")
                    errmsg += result;
            }

            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
                return;
            }

            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            if (mod == "mod")
            {
                wfb2di.OVERTIME_CD = overtime_cd;
                wfb2di.OVERTIME_DT_TYPE = overtime_dt_type;
            }
            else
            {
                wfb2di.OVERTIME_CD = txt_OVERTIME_CD.Text.ToUpper();
                wfb2di.OVERTIME_DT_TYPE = ddl_OVERTIME_DT_TYPE.SelectedValue;
            }
            wfb2di.OVERTIME_DESC = txt_OVERTIME_DESC.Text;
            wfb2di.WORK_DAY_CD = ddl_WORK_DAY_CD.SelectedValue;
            wfb2di.O_HOUR_CD = ddl_O_HOUR_CD.SelectedValue;
            wfb2di.O_MUL_CD = ddl_O_MUL_CD.SelectedValue;
            wfb2di.HYPER_SHOUR = txt_HYPER_SHOUR.Text;
            wfb2di.HYPER_EHOUR = txt_HYPER_EHOUR.Text;
            wfb2di.NORMAL_SHOUR = txt_NORMAL_SHOUR.Text;
            wfb2di.NORMAL_EHOUR = txt_NORMAL_EHOUR.Text;
            wfb2di.BASE_SHOUR = txt_BASE_SHOUR.Text;
            wfb2di.BASE_EHOUR = txt_BASE_EHOUR.Text;
            wfb2di.NONTAX_SHOUR = txt_NONTAX_SHOUR.Text;
            wfb2di.NONTAX_EHOUR = txt_NONTAX_EHOUR.Text;
            wfb2di.OTHER_SHOUR = txt_OTHER_SHOUR.Text;
            wfb2di.OTHER_EHOUR = txt_OTHER_EHOUR.Text;
            wfb2di.TAX_SHOUR = txt_TAX_SHOUR.Text;
            wfb2di.TAX_EHOUR = txt_TAX_EHOUR.Text;
            wfb2di.OVERTIME_EXCHANGE_CD = ddl_OVERTIME_EXCHANGE_CD.SelectedValue;
            wfb2di.SALARY_SETTLE_CD = ddl_SALARY_SETTLE_CD.SelectedValue;
            wfb2di.CHG_WORK_CD = txt_CHG_WORK_CD.Text;
            wfb2di.IS_USED = (ddl_IS_USED.SelectedValue == "-1") ? "" : ddl_IS_USED.SelectedValue;
            wfb2di.IS_IFLOW_SHOW = (ddl_IS_IFLOW_SHOW.SelectedValue == "-1") ? "" : ddl_IS_IFLOW_SHOW.SelectedValue;
            wfb2di.REMARK = txt_REMARK.Text;
            wfb2di.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2di.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2di.FUNC_ID = "FB2DI010";

            string msg = service.saveOVERTIME_TYPE(wfb2di, mod);
            if (msg != "0")
            {
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                Session["DI0100_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
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
        Session["DI0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0100_Qry.aspx");
    }
}