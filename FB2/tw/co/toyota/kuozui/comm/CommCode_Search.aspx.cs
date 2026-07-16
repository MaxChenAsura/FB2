using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_CommCode_Search : BasePage
{
    string SYS_CD = "";
    string MAIN_CD = "";
    string SUB_CD = "";
    string CODE_VAL1 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        SYS_CD = Request.QueryString["SYS_CD"] == null ? "" : Request.QueryString["SYS_CD"].ToString();
        MAIN_CD = Request.QueryString["MAIN_CD"] == null ? "" : Request.QueryString["MAIN_CD"].ToString();
        SUB_CD = Request.QueryString["SUB_CD"] == null ? "" : Request.QueryString["SUB_CD"].ToString();
        CODE_VAL1 = Request.QueryString["CODE_VAL1"] == null ? "" : Request.QueryString["CODE_VAL1"].ToString();

        if (!IsPostBack)
        {
            if (SYS_CD != "" || MAIN_CD != "" || SUB_CD != "")
            {
                
                getGridView("SUB_CD");
            }
            if (SUB_CD != "")
                txt_SUB_CD.Text = SUB_CD;
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            SUB_CD = txt_SUB_CD.Text;
            getGridView("SUB_CD");
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
            CommCode_Search change = new CommCode_Search();
            change.MAIN_CD = MAIN_CD;
            change.SYS_CD = SYS_CD;
            change.SUB_CD = SUB_CD;
            change.SUB_DESC = txt_SUB_DESC.Text;
            change.CODE_VAL1 = CODE_VAL1;

            DataTable dt = change.getCommCodeData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "SUB_CD" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_SUB_CD");
                if (other != null && other.Checked)
                {
                    //取得選擇列，產生Pjob_CD json資料
                    OpenWindowRtnJson json = new OpenWindowRtnJson();
                    json.CD = gv_result.Rows[i].Cells[1].Text;
                    json.DESC = gv_result.Rows[i].Cells[2].Text;
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

            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_SUB_CD");

            string script = "SelectOne('gv_result.*rblg_SUB_CD',this)";

            rdo.Attributes.Add("onclick", script);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
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