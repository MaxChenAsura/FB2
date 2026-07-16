using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_EmpFamily_Search : BasePage
{
    string EMP_ID = "";
    string FAMILY_LICENSE_ID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        EMP_ID = Request.QueryString["EMP_ID"] == null ? "" : Request.QueryString["EMP_ID"].ToString();
        FAMILY_LICENSE_ID = Request.QueryString["FAMILY_LICENSE_ID"] == null ? "" : Request.QueryString["FAMILY_LICENSE_ID"].ToString();
        if (!IsPostBack)
        {
            if (EMP_ID != "" || FAMILY_LICENSE_ID != "")
                getGridView("FAMILY_LICENSE_ID");
            txt_FAMILY_LICENSE_ID.Text = FAMILY_LICENSE_ID;
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            FAMILY_LICENSE_ID = txt_FAMILY_LICENSE_ID.Text;
            getGridView("FAMILY_LICENSE_ID");
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
            EmpFamily_Search change = new EmpFamily_Search();
            change.EMP_ID = EMP_ID;
            change.FAMILY_LICENSE_ID = FAMILY_LICENSE_ID;
            change.FAMILY_NAME = txt_FAMILY_NAME.Text;
            DataTable dt = change.getFamilyData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "FAMILY_LICENSE_ID" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_FAMILY_LICENSE_ID");
                if (other != null && other.Checked)
                {
                    //取得選擇列，產生Pjob_CD json資料
                    OpenWindowRtnJson json = new OpenWindowRtnJson();
                    json.CD = gv_result.Rows[i].Cells[1].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[1].Text;
                    json.DESC = gv_result.Rows[i].Cells[2].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[2].Text;
                    json.Val1 = gv_result.Rows[i].Cells[3].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[3].Text;
                    json.Val2 = gv_result.Rows[i].Cells[4].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[4].Text;
                    json.Val3 = gv_result.Rows[i].Cells[5].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[5].Text;
                    json.Val4 = gv_result.Rows[i].Cells[6].Text == "&nbsp;" ? "" : gv_result.Rows[i].Cells[6].Text;
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
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_FAMILY_LICENSE_ID");

            string script = "SelectOne('gv_result.*rblg_FAMILY_LICENSE_ID',this)";

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