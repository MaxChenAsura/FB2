using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Pjob_Search : BasePage
{

    string WS_CD = "";
    string LEVEL_CD = "";
    string PJOB_CD = "";
    string START_DT = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        WS_CD = Request.QueryString["WS_CD"] == null ? "" : Request.QueryString["WS_CD"].ToString();
        LEVEL_CD = Request.QueryString["LEVEL_CD"] == null ? "" : Request.QueryString["LEVEL_CD"].ToString();
        PJOB_CD = Request.QueryString["PJOB_CD"] == null ? "" : Request.QueryString["PJOB_CD"].ToString();
        START_DT = Request.QueryString["START_DT"] == null ? "" : Request.QueryString["START_DT"].ToString();

        if (!Page.IsPostBack)
        {
            //取得職種
            getWS();
            //取得資格代號
            getLevelCD();
            if (WS_CD != "" || LEVEL_CD != "" || PJOB_CD != "" || START_DT != "")
            {   //getGridView("PJOB_CD");

                ddl_WS.SelectedValue = WS_CD;
                ddl_LEVEL_CD.SelectedValue = LEVEL_CD;
                txt_PJOB_CD.Text = PJOB_CD;
                //txt_START_DT.Text = START_DT;
            }
        }
    }

    private void getLevelCD()
    {
        try
        {
            Pjob_Search search = new Pjob_Search();
            DataTable dt = new DataTable();
            dt = search.getLevelCD();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWS()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGridView(string SortExpression)
    {
        try
        {
            //取得職務代碼並繫結至Gridview
            Pjob_Search pjob = new Pjob_Search();
            pjob.PJOB_CD = PJOB_CD;
            pjob.PJOB_DESC = txt_PJOB_DESC.Text;
            pjob.LEVEL_CD = LEVEL_CD;
            pjob.WS_CD = WS_CD;
            pjob.START_DT = START_DT;

            DataTable dt = pjob.getPjobData(SortExpression + " " + getSortDirection(SortExpression));
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "PJOB_CD" };
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
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_PJOB_CD");
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
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_PJOB_CD");

            string script = "SelectOne('gv_result.*rblg_PJOB_CD',this)";

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
    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {

            PJOB_CD = txt_PJOB_CD.Text;
            LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            WS_CD = ddl_WS.SelectedValue;

            getGridView("PJOB_CD");
        }
        catch (Exception)
        {

            throw;
        }
    }
}