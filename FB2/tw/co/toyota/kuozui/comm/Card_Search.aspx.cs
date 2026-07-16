using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_Card_Search : BasePage
{
    string CARD_TYPE = "";
    string TEMP_CARD_CD = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        CARD_TYPE = Request.QueryString["CARD_TYPE"] == null ? "" : Request.QueryString["CARD_TYPE"].ToString();
        TEMP_CARD_CD = Request.QueryString["TEMP_CARD_CD"] == null ? "" : Request.QueryString["TEMP_CARD_CD"].ToString();
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            getGridView("CARD_NO");
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
            Card_Search change = new Card_Search();
            change.CARD_NO = txt_CARD_NO.Text;
            change.CARD_NAME = txt_CARD_NAME.Text;
            change.TEMP_CARD_CD = TEMP_CARD_CD;

            DataTable dt = new DataTable();
            if (CARD_TYPE == "Borrow")
            {
                if (TEMP_CARD_CD != "")
                    dt = change.getCardData2(SortExpression + " " + getSortDirection(SortExpression));
            }
            else if (CARD_TYPE == "Return")
            {
                if (TEMP_CARD_CD != "")
                    dt = change.getCardData3(SortExpression + " " + getSortDirection(SortExpression));
            }
            else if (CARD_TYPE == "Qry")
            {
                dt = change.getCardData4(SortExpression + " " + getSortDirection(SortExpression));
            }
            else
            {
                dt = change.getCardData(SortExpression + " " + getSortDirection(SortExpression));
            }
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "CARD_NO" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_CARD_NO");
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
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_CARD_NO");

            string script = "SelectOne('gv_result.*rblg_CARD_NO',this)";

            rdo.Attributes.Add("onclick", script);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            string month = DateTime.Now.Month.ToString() + "月";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.Cells[i].Text == month)
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Red;

            }
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