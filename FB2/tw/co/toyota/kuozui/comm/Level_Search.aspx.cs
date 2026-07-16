using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_Level_Search : BasePage
{
    string INS_AMT = "";
    string INS= "";
    string INS_TYPE = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        INS = Request.QueryString["INS"] == null ? "" : Request.QueryString["INS"].ToString();
        INS_TYPE = Request.QueryString["INS_TYPE"] == null ? "" : Request.QueryString["INS_TYPE"].ToString();
        if (!IsPostBack)
        {
            if (INS_TYPE != "" || INS != "")
                getGridView("INS_AMT");
            txt_INS_AMT.Text = INS_AMT;
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            INS_AMT = txt_INS_AMT.Text;
            getGridView("INS_AMT");
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    private void getGridView(string SortExpression)
    {
        try
        {
            //取得職務代碼並繫結至Gridview
            Level_Search change = new Level_Search();
            change.INS_AMT = INS_AMT;
            change.INS = INS;
            change.INS_TYPE = INS_TYPE;

            DataTable dt = change.getLevelData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "INS_AMT" };
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    //按下確認時事件
    protected void btn_confirm_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_INS_AMT");
                if (other != null && other.Checked)
                {
                    //取得選擇列，產生Pjob_CD json資料
                    OpenWindowRtnJson json = new OpenWindowRtnJson();
                    json.CD = gv_result.Rows[i].Cells[1].Text;
                    json.DESC = gv_result.Rows[i].Cells[2].Text;
                    json.Val1 = gv_result.Rows[i].Cells[3].Text;
                    json.Val2 = gv_result.Rows[i].Cells[4].Text;
                    string strJson = JsonConvert.SerializeObject(json, Formatting.None);

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "return", "ReturnValue('" + strJson + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_INS_AMT");

            string script = "SelectOne('gv_result.*rblg_INS_AMT',this)";

            rdo.Attributes.Add("onclick", script);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.CssClass = "header";

        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }
}