using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2se_WFB2SE1300_Dtl : BasePage
{
    string fun_name = "FB2SE130";
    string qdatakey = string.Empty;
    //Service 物件
    private CFB2SE1300BO service = new CFB2SE1300BO();
    private CFB2SE1300DAO fb2se = new CFB2SE1300DAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得table 顯示欄位 值
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["qdatakey"])))
            {
                qdatakey = Convert.ToString(Request.QueryString["qdatakey"]);
                DataTable dt = fb2se.getData_DT(qdatakey);

                if (dt.Rows.Count > 0)
                {
                    lbl_EFFECT_YM.Text = string.Format("{0}/{1}", Convert.ToString(dt.Rows[0]["EFFECT_YM"]).Substring(0, 4), Convert.ToString(dt.Rows[0]["EFFECT_YM"]).Substring(4, 2));

                    string x = dt.Rows[0]["RELEASE_BY"].ToString();

                    if (Convert.ToString(dt.Rows[0]["RELEASE_BY"].ToString()) == "")
                    {
                        lbl_RELEASE_NAME.Text = "";
                        lbl_RELEASE_DT.Text = "";
                        lbl_SUB_DESC.Text = Convert.ToString(dt.Rows[0]["SUB_DESC"]);
                        lbl_APPROVE_NAME.Text = "";
                        lbl_APPROVE_DT.Text = "";
                        txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);
                        WFB2SE1300Release.Enabled = true;
                    }
                    else
                    {
                        lbl_RELEASE_NAME.Text = Convert.ToString(dt.Rows[0]["RELEASE_BY"]).ToString() + "-" + Convert.ToString(dt.Rows[0]["RELEASE_NAME"]).ToString();
                        lbl_RELEASE_DT.Text = Convert.ToDateTime(dt.Rows[0]["RELEASE_DT"]).ToString("yyyy/MM/dd");
                        lbl_SUB_DESC.Text = Convert.ToString(dt.Rows[0]["SUB_DESC"]);
                        lbl_APPROVE_NAME.Text = Convert.ToString(dt.Rows[0]["APPROVE_NAME"]);
                        txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["APPROVE_DT"])))
                        {
                            lbl_APPROVE_DT.Text = Convert.ToDateTime(dt.Rows[0]["APPROVE_DT"]).ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            lbl_APPROVE_DT.Text = "";
                        }

                    }

                    iframe3.Attributes["src"] = "WFB2SE1300_SubDtl3.aspx?qdatakey=" + qdatakey;
                    iframe1.Attributes["src"] = "WFB2SE1300_SubDtl1.aspx?qdatakey=" + qdatakey;
                    iframe2.Attributes["src"] = "WFB2SE1300_SubDtl2.aspx?qdatakey=" + qdatakey;
                }
            }

        }
    }

    protected void WFB2SE1300Release_Click(object sender, EventArgs e)
    {
        CFB2SE1300BO service = new CFB2SE1300BO();
        string msg = string.Empty;
        string emp_id = SessionHandle.Current.emp_id;
        fb2se.EMP_ID = emp_id;
        fb2se.EFFECT_YM = Convert.ToString(Request.QueryString["qdatakey"]);
        fb2se.FUNC_ID = "FB2SE130";

        msg = service.updateData(fb2se);
        if (msg == "0")
        {
            showMessage("releaseSuccessMessage");
            Session["SE1300_Is_Search"] = "Y";
            ScriptManager.RegisterClientScriptBlock(WFB2SE1300Release, this.GetType(), "success", "location.href='WFB2SE1300_Qry.aspx';", true);
        }
        else
        {
            showMessage("modFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(WFB2SE1300Release, this.GetType(), "init", "location.reload()", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SE1300_Is_Search"] = "Y";
        Response.Redirect("WFB2SE1300_Qry.aspx");
    }
}