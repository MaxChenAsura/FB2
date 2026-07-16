using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_WorkShift_Search : BasePage
{
    string WORK_SHIFT_CD = "";
    string WORKER_WORK_SHIFT = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        WORK_SHIFT_CD = Request.QueryString["WORK_SHIFT_CD"] == null ? "" : Request.QueryString["WORK_SHIFT_CD"].ToString();
        WORKER_WORK_SHIFT = Request.QueryString["WORKER_WORK_SHIFT"] == null ? "" : Request.QueryString["WORKER_WORK_SHIFT"].ToString();

        if (!IsPostBack)
        {
            getCalendarCd();

            if (WORK_SHIFT_CD != "")
                getGridView("WORK_SHIFT_CD");
           txt_WORK_SHIFT_CD.Text =  WORK_SHIFT_CD;
        }
    }

    private void getCalendarCd()
    {
        try
        {
            WorkShift_Search search = new WorkShift_Search();
            DataTable dt = new DataTable();
            dt = search.getCalendarCd();
            ddl_CALENDAR_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CALENDAR_CD.Items.Add(new ListItem(dt.Rows[i]["CALENDAR_DESC"].ToString(), dt.Rows[i]["CALENDAR_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            WORK_SHIFT_CD = txt_WORK_SHIFT_CD.Text;
            getGridView("WORK_SHIFT_CD");
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void getGridView(string SortExpression)
    {
        try
        {
            //取得職務代碼並繫結至Gridview
            WorkShift_Search search = new WorkShift_Search();
            search.WORK_SHIFT_CD = WORK_SHIFT_CD;
            search.WORK_SHIFT_DESC = txt_WORK_SHIFT_DESC.Text;
            search.CALENDAR_CD = ddl_CALENDAR_CD.SelectedValue;
            search.CALENDAR_DESC = txt_CALENDAR_DESC.Text;
            search.WORKER_WORK_SHIFT = WORKER_WORK_SHIFT;

            DataTable dt = search.getWorkShiftData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "WORK_SHIFT_CD" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_WORK_SHIFT_CD");
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

            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_WORK_SHIFT_CD");

            string script = "SelectOne('gv_result.*rblg_WORK_SHIFT_CD',this)";

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