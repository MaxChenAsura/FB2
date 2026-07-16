using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_AssessDept_Search : BasePage
{
    string DEPT_NO = "";
    int DEPT_LEVEL = 0;
    string DEPT_NAME = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DEPT_NO = Request.QueryString["DEPT_NO"] == null ? "" : Request.QueryString["DEPT_NO"].ToString();
        DEPT_LEVEL = Request.QueryString["DEPT_LEVEL"] == null ? 0 : Int32.Parse(Request.QueryString["DEPT_LEVEL"].ToString());
        DEPT_NAME = Request.QueryString["DEPT_NAME"] == null ? "" : Request.QueryString["DEPT_NAME"].ToString();
       
        if (!IsPostBack)
        {
            if (DEPT_NO != "" || DEPT_LEVEL != 0 || DEPT_NAME != "")
            {
                
                getGridView("DEPT_NO");
            }
            if (DEPT_NO != "")
                txt_DEPT_NO.Text = DEPT_NO;
            if (DEPT_NAME != "")
                txt_DEPT_NAME.Text = DEPT_NAME;
            hid_DEPT_LEVEL.Value = DEPT_LEVEL.ToString();
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            DEPT_NO = txt_DEPT_NO.Text;
            DEPT_NAME = txt_DEPT_NAME.Text;
            getGridView("DEPT_NO");
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
            AssessDept_Search change = new AssessDept_Search();
            change.DEPT_NO = DEPT_NO;
            change.DEPT_NAME = DEPT_NAME;
            change.DEPT_LEVEL = DEPT_LEVEL;

            DataTable dt = change.getAssessDeptData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "DEPT_NO" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_DEPT_NO");
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
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_DEPT_NO");

            string script = "SelectOne('gv_result.*rblg_DEPT_NO',this)";

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