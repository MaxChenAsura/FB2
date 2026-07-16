using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_EmpGrade_Search : BasePage
{
    string GRADE_CD = "";
    string IS_VALID = "";
    string LEVEL_CD = "";
    string EMP_ID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GRADE_CD = Request.QueryString["GRADE_CD"] == null ? "" : Request.QueryString["GRADE_CD"].ToString();
            IS_VALID = Request.QueryString["IS_VALID"] == null ? "" : Request.QueryString["IS_VALID"].ToString();
            LEVEL_CD = Request.QueryString["LEVEL_CD"] == null ? "" : Request.QueryString["LEVEL_CD"].ToString();
            EMP_ID = Request.QueryString["EMP_ID"] == null ? "" : Request.QueryString["EMP_ID"].ToString();
            txt_GRADE_CD.Text = GRADE_CD;
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            getGridView("GRADE_CD");
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
            EmpGrade_Search change = new EmpGrade_Search();
            change.GRADE_CD = txt_GRADE_CD.Text;
            //change.GRADE_DESC = txt_GRADE_DESC.Text;
            change.IS_VALID = IS_VALID;
            change.LEVEL_CD = LEVEL_CD;
            change.EMP_ID = EMP_ID;

            DataTable dt = change.getGradeCdData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "GRADE_CD" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_GRADE_CD");
                if (other != null && other.Checked)
                {
                    //取得選擇列，產生Pjob_CD json資料
                    OpenWindowRtnJson json = new OpenWindowRtnJson();
                    if (string.IsNullOrEmpty(gv_result.Rows[i].Cells[1].Text) || gv_result.Rows[i].Cells[1].Text == "&nbsp;")
                        json.CD = "";
                    else
                        json.CD = gv_result.Rows[i].Cells[1].Text;
                    json.DESC = "";
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
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_GRADE_CD");

            string script = "SelectOne('gv_result.*rblg_GRADE_CD',this)";

            rdo.Attributes.Add("onclick", script);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            //string month = DateTime.Now.Month.ToString() + "月";
            //for (int i = 2; i < e.Row.Cells.Count; i++)
            //{
            //    if (e.Row.Cells[i].Text == month)
            //        e.Row.Cells[i].BackColor = System.Drawing.Color.Red;

            //}
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