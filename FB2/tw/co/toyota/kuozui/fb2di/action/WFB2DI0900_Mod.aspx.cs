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

public partial class WebContent_fb2di_WFB2DI0900_Mod : BasePage
{
    string mod = "";
    string start_dt = "";
    string stime = "";
    //Service 物件
    private CFB2DI0900BO service = new CFB2DI0900BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        mod = Request.QueryString["mod"] == null ? "" : Request.QueryString["mod"].ToString();
        start_dt = Request.QueryString["start_dt"] == null ? "" : Request.QueryString["start_dt"].ToString();
        stime = Request.QueryString["stime"] == null ? "" : Request.QueryString["stime"].ToString();
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
            //產生相關下拉選單
            DataTable dt = new DataTable();
            #region 開始時間
            ddl_STIME.Items.Add(new ListItem("", "-1"));
            for (int i = 0; i <= 23; i++)
            {
                ddl_STIME.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
            }
            ddl_STIME2.Items.Add(new ListItem("", "-1"));
            ddl_STIME2.Items.Add(new ListItem("00", "00"));
            ddl_STIME2.Items.Add(new ListItem("10", "10"));
            ddl_STIME2.Items.Add(new ListItem("20", "20"));
            ddl_STIME2.Items.Add(new ListItem("30", "30"));
            ddl_STIME2.Items.Add(new ListItem("40", "40"));
            ddl_STIME2.Items.Add(new ListItem("50", "50"));

            #endregion
            #region 結束時間
            ddl_ETIME.Items.Add(new ListItem("", "-1"));
            for (int i = 0; i <= 23; i++)
            {
                ddl_ETIME.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
            }
            ddl_ETIME2.Items.Add(new ListItem("", "-1"));
            ddl_ETIME2.Items.Add(new ListItem("00", "00"));
            ddl_ETIME2.Items.Add(new ListItem("10", "10"));
            ddl_ETIME2.Items.Add(new ListItem("20", "20"));
            ddl_ETIME2.Items.Add(new ListItem("30", "30"));
            ddl_ETIME2.Items.Add(new ListItem("40", "40"));
            ddl_ETIME2.Items.Add(new ListItem("50", "50"));
            #endregion

            ddl_STIME.SelectedValue = "00";
            ddl_STIME2.SelectedValue = "00";
            ddl_ETIME.SelectedValue = "00";
            ddl_ETIME2.SelectedValue = "00";

            if (mod == "mod")
            {
                //開始日期
                txt_START_DT.Text = start_dt;
                txt_START_DT.BorderWidth = 0;
                txt_START_DT.ReadOnly = true;
                txt_START_DT.CssClass = "";
                ddl_STIME.SelectedValue = stime.Split(':')[0];
                ddl_STIME.Enabled = false;
                ddl_STIME2.SelectedValue = stime.Split(':')[1];
                ddl_STIME2.Enabled = false;
                ddl_STIME.CssClass = "";
                ddl_STIME2.CssClass = "";
            }
            else
            {
                ddl_STIME.CssClass = "MandatoryField";
                ddl_STIME2.CssClass = "MandatoryField";
            }

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
            dt = service.getDefaultData(start_dt);
            if (dt.Rows.Count > 0)
            {
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                ddl_ETIME.SelectedValue = dt.Rows[0]["ETIME"].ToString();
                ddl_ETIME2.SelectedValue = dt.Rows[0]["ETIME2"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0900Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DI0900DAO wfb2di = new CFB2DI0900DAO();
            if (mod == "mod")
            {
                wfb2di.START_DT = start_dt;
                wfb2di.STIME = stime.Replace(":","");
            }
            else
            {
                wfb2di.START_DT = txt_START_DT.Text;
                wfb2di.STIME = ddl_STIME.SelectedValue + ddl_STIME2.SelectedValue;
            }
            wfb2di.END_DT = txt_END_DT.Text;
            wfb2di.ETIME = ddl_ETIME.SelectedValue + ddl_ETIME2.SelectedValue;
            wfb2di.REMARK = txt_REMARK.Text;
            wfb2di.START_TIME = wfb2di.START_DT + " " + ddl_STIME.SelectedValue + ":" + ddl_STIME2.SelectedValue;
            wfb2di.END_TIME = txt_END_DT.Text + " " + ddl_ETIME.SelectedValue + ":" + ddl_ETIME2.SelectedValue;
            wfb2di.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2di.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2di.FUNC_ID = "FB2DI090";

            string msg = service.saveDISASTER_DT(wfb2di, mod);
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
                Session["DI0900_Is_Search"] = "Y";
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
        Session["DI0900_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0900_Qry.aspx");
    }


}