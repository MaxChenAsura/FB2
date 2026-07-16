using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2se_WFB2SE0200_Qry : BasePage
{
    CFB2SE0200BO service = new CFB2SE0200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            createLEVEL_CD();
            realeaseConditions();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void createLEVEL_CD()
    {
        try
        {
            ddl_LEVEL_CD.Items.Clear();
            CFB2SE0200DAO fb2se = new CFB2SE0200DAO();
            DataTable dt = fb2se.getDDL();
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
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("LEVEL_CD");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //WFB2SE0200Edit.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

            HID_PageRow.Value = "";
            Session["SE0200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";


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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1 && gv_result.Visible == true)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2SE0200Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("LEVEL_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("LEVEL_CD", 0, 10);


            gv_result.EditIndex = -1;
            //gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                //WFB2SE0200Edit.Visible = true;
            }
            else
            {
                //WFB2SE0200Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //進入匯入頁面
    protected void WFB2SE0200Import_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2SE0200_Upload.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SE0200_EFFECT_YM"] = txt_EFFECT_YM.Text;
            Session["SE0200_LEVEL_CD"] = ddl_LEVEL_CD.SelectedValue;
            //Session["SE0100_Is_Search"] = "Y";
        }
        else
        {
            Session["SE0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SE0200_Is_Search"] == "Y")
            {
                txt_EFFECT_YM.Text = Session["SE0200_EFFECT_YM"].ToString();
                ddl_LEVEL_CD.SelectedValue = Session["SE0200_LEVEL_CD"].ToString();
                ViewState["PerPageRow"] = Session["SE0200_ddlPerPageRow"].ToString();
                WFB2SE0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion



    #region 棄用改用匯入
    /*
    protected void WFB2SE0200LoadAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            gv_result.Visible = false;
            //WFB2SE0200Edit.Visible = false;
            OnePage.Visible = false;
            CFB2SE0200DAO fb2se = new CFB2SE0200DAO();
            fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            int count = fb2se.GetCount_TB_S_M_2BSALARY_SET_H();
            if (count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + hid_wfb2se_Add_AlreadyExistMessage2.Value + "');", true);

            }
            else
            {
                gv_result2.Visible = true;
                gv_result2.PageIndex = 0;
                gv_result2.PageSize = 10000;
                gv_result2.DataSourceID = "ods2";
                if (ViewState["SortExpression"] != "LEVEL_CD")
                    getSortDirection("ORDER_SEQ,LEVEL_CD");
                gv_result2.DataKeyNames = new string[] { "LEVEL_CD" };
                gv_result2.DataBind();

                if (gv_result2.Rows.Count > 0)
                {
                    //WFB2SE0200Edit.Visible = false;
                    WFB2SE0200Search.Enabled = false;
                    btn_clear.Enabled = false;
                    //WFB2SE0200LoadAdd.Enabled = false;

                    //WFB2SE0200OK.Visible = true;
                    //btn_cancel.Visible = true;
                }
                else
                {
                    //WFB2SE0200Edit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SE0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            string release_dt = "";
            CFB2SE0200DAO fb2se = new CFB2SE0200DAO();
            fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            DataTable dt = fb2se.Get_H_RELEASE_DT();
            if (dt.Rows.Count > 0)
            {
                release_dt = Convert.ToString(dt.Rows[0]["RELEASE_DT"]);
            }
            if (release_dt != "")
            {
                //release_dt = Convert.ToDateTime(release_dt).ToString("yyyy/MM/dd");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + txt_EFFECT_YM.Text + "資料,已提出核可申請,不允修改。');", true);
                return;
            }
            //disable查詢清除按鈕
            WFB2SE0200Search.Enabled = false;
            btn_clear.Enabled = false;
            //WFB2SE0200LoadAdd.Enabled = false;
            //WFB2SE0200OK.Visible = true;
            btn_cancel.Visible = true;
            //WFB2SE0200Edit.Visible = false;

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.EditIndex = editindex[0];
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2SE0200OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SE0200DAO fb2se = new CFB2SE0200DAO();
            DataTable dtSend = new DataTable();
            dtSend.Columns.Add("EFFECT_YM");
            dtSend.Columns.Add("LEVEL_CD");
            dtSend.Columns.Add("PJOB_TYPE");
            dtSend.Columns.Add("EXAMINE_A");
            dtSend.Columns.Add("EXAMINE_B");
            dtSend.Columns.Add("EXAMINE_C1");
            dtSend.Columns.Add("EXAMINE_C2");
            dtSend.Columns.Add("EXAMINE_D");
            dtSend.Columns.Add("EXAMINE_E");
            dtSend.Columns.Add("ORDER_SEQ");
            //新增
            if (gv_result2.Visible == true && gv_result2.Rows.Count > 0)
            {
                for (int i = 0; i < gv_result2.Rows.Count; i++)
                {
                    Label lb_LEVEL_CD = (Label)gv_result2.Rows[i].FindControl("lb_LEVEL_CD_gv2");
                    HiddenField hid_PJOB_TYPE = (HiddenField)gv_result2.Rows[i].FindControl("hid_PJOB_TYPE");
                    TextBox txt_NEW_EXAMINE_A = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_A");
                    TextBox txt_NEW_EXAMINE_B = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_B");
                    TextBox txt_NEW_EXAMINE_C1 = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_C1");
                    TextBox txt_NEW_EXAMINE_C2 = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_C2");
                    TextBox txt_NEW_EXAMINE_D = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_D");
                    TextBox txt_NEW_EXAMINE_E = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_E");
                    HiddenField HID_ORDER_SEQ = (HiddenField)gv_result2.Rows[i].FindControl("HID_ORDER_SEQ");

                    DataRow row = dtSend.NewRow();
                    row["EFFECT_YM"] = txt_EFFECT_YM.Text.Replace("/", "");
                    row["LEVEL_CD"] = lb_LEVEL_CD.Text;
                    row["PJOB_TYPE"] = hid_PJOB_TYPE.Value;
                    row["EXAMINE_A"] = txt_NEW_EXAMINE_A.Text;
                    row["EXAMINE_B"] = txt_NEW_EXAMINE_B.Text;
                    row["EXAMINE_C1"] = txt_NEW_EXAMINE_C1.Text;
                    row["EXAMINE_C2"] = txt_NEW_EXAMINE_C2.Text;
                    row["EXAMINE_D"] = txt_NEW_EXAMINE_D.Text;
                    row["EXAMINE_E"] = txt_NEW_EXAMINE_E.Text;
                    row["ORDER_SEQ"] = HID_ORDER_SEQ.Value;
                    dtSend.Rows.Add(row);



                }
                string msg = service.Add(fb2se, dtSend, txt_EFFECT_YM.Text.Replace("/", ""));
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            //修改
            if (gv_result.Visible == true && gv_result.EditIndex != -1)
            {

                Label lb_LEVEL_CD = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_LEVEL_CD_gv1");
                HiddenField hid_PJOB_TYPE = (HiddenField)gv_result.Rows[gv_result.EditIndex].FindControl("hid_PJOB_TYPE_gv1");
                TextBox txt_EDIT_EXAMINE_A = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_A");
                TextBox txt_EDIT_EXAMINE_B = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_B");
                TextBox txt_EDIT_EXAMINE_C1 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_C1");
                TextBox txt_EDIT_EXAMINE_C2 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_C2");
                TextBox txt_EDIT_EXAMINE_D = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_D");
                TextBox txt_EDIT_EXAMINE_E = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_E");
                fb2se.data_key = gv_result.DataKeys[gv_result.EditIndex].Value.ToString();
                fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
                fb2se.LEVEL_CD = lb_LEVEL_CD.Text;
                fb2se.PJOB_TYPE = hid_PJOB_TYPE.Value;
                fb2se.EXAMINE_A = txt_EDIT_EXAMINE_A.Text;
                fb2se.EXAMINE_B = txt_EDIT_EXAMINE_B.Text;
                fb2se.EXAMINE_C1 = txt_EDIT_EXAMINE_C1.Text;
                fb2se.EXAMINE_C2 = txt_EDIT_EXAMINE_C2.Text;
                fb2se.EXAMINE_D = txt_EDIT_EXAMINE_D.Text;
                fb2se.EXAMINE_E = txt_EDIT_EXAMINE_E.Text;
                string msg = service.Update(fb2se);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("modSuccessMessage");
                    //WFB2SE0200Edit.Visible = true;
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.EditIndex = -1;
            //enable查詢清除按鈕
            WFB2SE0200Search.Enabled = true;
            btn_clear.Enabled = true;
            //WFB2SE0200LoadAdd.Enabled = true;
            //WFB2SE0200OK.Visible = false;
            //btn_cancel.Visible = false;
            //gv_result.Visible = false;
            gv_result2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
  
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        WFB2SE0200Search.Enabled = true;
        btn_clear.Enabled = true;
        //WFB2SE0200LoadAdd.Enabled = true;
        //WFB2SE0200OK.Visible = false;
        //btn_cancel.Visible = false;

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
        else
            GetGridView("RowNumber", 0, 10);
        gv_result.EditIndex = -1;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
            gv_result2.Visible = false;
            //WFB2SE0200Edit.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
        }
        else
        {
            gv_result.Visible = true;
            gv_result2.Visible = false;
            //WFB2SE0200Edit.Visible = true;
        }


    }
    */
    #endregion

}