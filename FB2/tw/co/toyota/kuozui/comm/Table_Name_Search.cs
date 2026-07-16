using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tw_co_toyota_kuozui_web_comm_Table_Name_Search : BasePage
{
    string SYS_kind;
    protected void Page_Load(object sender, EventArgs e)
    {
        //取得mode:dept 只顯示部門 all 全部顯示
        SYS_kind = Request.QueryString["SYS_kind"];
        if (!Page.IsPostBack)
        {
            Table_Name_Search dao = new Table_Name_Search();
            DataTable dt = dao.getTABLE_NAME(SYS_kind);
            gv_result.DataSource = dt;
            gv_result.DataBind();
        }

    }

    protected void btn_confirm_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((RadioButton)gv_result.Rows[i].FindControl("rbl_TABLE_NAME")).Checked)
                {
                    OpenWindowRtnJson json = new OpenWindowRtnJson();
                    json.CD = gv_result.Rows[i].Cells[1].Text;
                    string strJson = JsonConvert.SerializeObject(json, Formatting.None);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "return", "ReturnValue('" + strJson + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_TABLE_NAME");

            string script = "SelectOne('gv_result.*rblg_TABLE_NAME',this)";

            rdo.Attributes.Add("onclick", script);
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