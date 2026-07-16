using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2se_WFB2SE0100_Qry : BasePage
{
    CFB2SE0100BO service = new CFB2SE0100BO();
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
            CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
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
                getSortDirection("GRADE_CD desc,LEVEL_CD");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //WFB2SE0100Edit.Visible = false;
                //WFB2SE0101Edit.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";
            Session["SE0100_ddlPerPageRow"] = ViewState["PerPageRow"];
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


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SE0100_EFFECT_YM"] = txt_EFFECT_YM.Text;
            Session["SE0100_LEVEL_CD"] = ddl_LEVEL_CD.SelectedValue;
            //Session["SE0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["SE0100_EFFECT_YM"] = null;
            //Session["SE0100_LEVEL_CD"] = null;
            Session["SE0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SE0100_Is_Search"] == "Y")
            {
                txt_EFFECT_YM.Text = Session["SE0100_EFFECT_YM"].ToString();
                ddl_LEVEL_CD.SelectedValue = Session["SE0100_LEVEL_CD"].ToString();
                ViewState["PerPageRow"] = Session["SE0100_ddlPerPageRow"].ToString();
                WFB2SE0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

    /* 資格下載 改用匯入
    protected void WFB2SE0100LoadAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            gv_result.Visible = false;
            WFB2SE0100Edit.Visible = false;
            WFB2SE0101Edit.Visible = false;
            OnePage.Visible = false;
            CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
            fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            int count = fb2se.GetCount_TB_S_M_SALARYSET_H();
            if (count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + hid_wfb2se_Add_AlreadyExistMessage.Value + "');", true);
            }
            else
            {
                gv_result2.Visible = true;
                gv_result2.PageIndex = 0;
                gv_result2.PageSize = 10000;
                gv_result2.DataSourceID = "ods2";
                if (ViewState["SortExpression"] != "LEVEL_CD" || ViewState["SortExpression"] != "GRADE_CD")
                    getSortDirection("ORDER_SEQ,LEVEL_CD,GRADE_CD");

                //if (ViewState["SortExpression"] == null)
                //    getSortDirection("ORDER_SEQ,LEVEL_CD,GRADE_CD");
                gv_result2.DataKeyNames = new string[] { "LEVEL_CD" };
                gv_result2.DataBind();

                if (gv_result2.Rows.Count > 0)
                {
                    WFB2SE0100Edit.Visible = false;
                    WFB2SE0101Edit.Visible = false;
                    WFB2SE0100Search.Enabled = false;
                    btn_clear.Enabled = false;
                    WFB2SE0100LoadAdd.Enabled = false;

                    WFB2SE0100OK.Visible = true;
                    btn_cancel.Visible = true;
                }
                else
                {
                    WFB2SE0100Edit.Visible = false;
                    WFB2SE0101Edit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
     * 
     */
 
    protected void WFB2SE0100Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("RowNumber", 0, 10);


            gv_result.EditIndex = -1;
            //gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                //WFB2SE0100Edit.Visible = true;
                //WFB2SE0101Edit.Visible = true;
            }
            else
            {
                //WFB2SE0100Edit.Visible = false;
                //WFB2SE0101Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SE0101Edit_Click(object sender, EventArgs e)
    {
        string release_dt = "";
        CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
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
        else
        {
            Response.Redirect("WFB2SE0100_Edit.aspx?effect_ym=" + fb2se.EFFECT_YM);
        }




        //string EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");

    }
    protected void WFB2SE0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            string release_dt = "";
            CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
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
            WFB2SE0100Search.Enabled = false;
            btn_clear.Enabled = false;
            //WFB2SE0100LoadAdd.Enabled = false;
            //WFB2SE0100OK.Visible = true;
            //btn_cancel.Visible = true;
            //WFB2SE0100Edit.Visible = false;
            //WFB2SE0101Edit.Visible = false;

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
    protected void WFB2SE0100OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
            DataTable dtSend = new DataTable();
            dtSend.Columns.Add("EFFECT_YM");
            dtSend.Columns.Add("LEVEL_CD");
            dtSend.Columns.Add("GRADE_CD");
            dtSend.Columns.Add("EXAMINE_A");
            dtSend.Columns.Add("EXAMINE_B");
            dtSend.Columns.Add("EXAMINE_C");
            dtSend.Columns.Add("EXAMINE_D");
            dtSend.Columns.Add("EXAMINE_E");
            dtSend.Columns.Add("ABILITY_ADJ");
            dtSend.Columns.Add("LEVEL_ADJ");
            dtSend.Columns.Add("LEVEL_PAY_LOW");
            dtSend.Columns.Add("LEVEL_PAY_AVG");
            dtSend.Columns.Add("LEVEL_PAY_UP");
            dtSend.Columns.Add("ORDER_SEQ");
            //新增
            if (gv_result2.Visible == true && gv_result2.Rows.Count > 0)
            {
                for (int i = 0; i < gv_result2.Rows.Count; i++)
                {
                    Label lb_LEVEL_CD = (Label)gv_result2.Rows[i].FindControl("lb_LEVEL_CD_gv2");
                    Label lb_GRADE_CD = (Label)gv_result2.Rows[i].FindControl("lb_GRADE_CD");
                    TextBox txt_NEW_EXAMINE_A = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_A");
                    TextBox txt_NEW_EXAMINE_B = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_B");
                    TextBox txt_NEW_EXAMINE_C = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_C");
                    TextBox txt_NEW_EXAMINE_D = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_D");
                    TextBox txt_NEW_EXAMINE_E = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_EXAMINE_E");
                    TextBox txt_NEW_ABILITY_ADJ = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_ABILITY_ADJ");
                    TextBox txt_NEW_LEVEL_ADJ = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_LEVEL_ADJ");
                    TextBox txt_NEW_LEVEL_PAY_LOW = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_LEVEL_PAY_LOW");
                    TextBox txt_NEW_LEVEL_PAY_AVG = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_LEVEL_PAY_AVG");
                    TextBox txt_NEW_LEVEL_PAY_UP = (TextBox)gv_result2.Rows[i].FindControl("txt_NEW_LEVEL_PAY_UP");
                    HiddenField HID_ORDER_SEQ = (HiddenField)gv_result2.Rows[i].FindControl("HID_ORDER_SEQ");

                    DataRow row = dtSend.NewRow();
                    row["EFFECT_YM"] = txt_EFFECT_YM.Text.Replace("/", "");
                    row["LEVEL_CD"] = lb_LEVEL_CD.Text;
                    row["GRADE_CD"] = lb_GRADE_CD.Text;
                    row["EXAMINE_A"] = txt_NEW_EXAMINE_A.Text;
                    row["EXAMINE_B"] = txt_NEW_EXAMINE_B.Text;
                    row["EXAMINE_C"] = txt_NEW_EXAMINE_C.Text;
                    row["EXAMINE_D"] = txt_NEW_EXAMINE_D.Text;
                    row["EXAMINE_E"] = txt_NEW_EXAMINE_E.Text;
                    row["ABILITY_ADJ"] = txt_NEW_ABILITY_ADJ.Text;
                    row["LEVEL_ADJ"] = txt_NEW_LEVEL_ADJ.Text;
                    row["LEVEL_PAY_LOW"] = txt_NEW_LEVEL_PAY_LOW.Text;
                    row["LEVEL_PAY_AVG"] = txt_NEW_LEVEL_PAY_AVG.Text;
                    row["LEVEL_PAY_UP"] = txt_NEW_LEVEL_PAY_UP.Text;
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
                Label lb_GRADE_CD = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_GRADE_CD");
                TextBox txt_EDIT_EXAMINE_A = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_A");
                TextBox txt_EDIT_EXAMINE_B = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_B");
                TextBox txt_EDIT_EXAMINE_C = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_C");
                TextBox txt_EDIT_EXAMINE_D = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_D");
                TextBox txt_EDIT_EXAMINE_E = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_EXAMINE_E");
                TextBox txt_EDIT_ABILITY_ADJ = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_ABILITY_ADJ");
                TextBox txt_EDIT_LEVEL_ADJ = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_LEVEL_ADJ");
                TextBox txt_EDIT_LEVEL_PAY_LOW = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_LEVEL_PAY_LOW");
                TextBox txt_EDIT_LEVEL_PAY_AVG = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_LEVEL_PAY_AVG");
                TextBox txt_EDIT_LEVEL_PAY_UP = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_LEVEL_PAY_UP");
                fb2se.data_key = gv_result.DataKeys[gv_result.EditIndex].Value.ToString();
                fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
                fb2se.LEVEL_CD = lb_LEVEL_CD.Text;
                fb2se.GRADE_CD = lb_GRADE_CD.Text;
                fb2se.EXAMINE_A = txt_EDIT_EXAMINE_A.Text;
                fb2se.EXAMINE_B = txt_EDIT_EXAMINE_B.Text;
                fb2se.EXAMINE_C = txt_EDIT_EXAMINE_C.Text;
                fb2se.EXAMINE_D = txt_EDIT_EXAMINE_D.Text;
                fb2se.EXAMINE_E = txt_EDIT_EXAMINE_E.Text;
                fb2se.ABILITY_ADJ = txt_EDIT_ABILITY_ADJ.Text;
                fb2se.LEVEL_ADJ = txt_EDIT_LEVEL_ADJ.Text;
                fb2se.LEVEL_PAY_LOW = txt_EDIT_LEVEL_PAY_LOW.Text;
                fb2se.LEVEL_PAY_AVG = txt_EDIT_LEVEL_PAY_AVG.Text;
                fb2se.LEVEL_PAY_UP = txt_EDIT_LEVEL_PAY_UP.Text;
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
                    //WFB2SE0100Edit.Visible = true;
                    //WFB2SE0101Edit.Visible = true;
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
            WFB2SE0100Search.Enabled = true;
            btn_clear.Enabled = true;
            //WFB2SE0100LoadAdd.Enabled = true;
            //WFB2SE0100OK.Visible = false;
            //btn_cancel.Visible = false;
            gv_result2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        WFB2SE0100Search.Enabled = true;
        btn_clear.Enabled = true;
        //WFB2SE0100LoadAdd.Enabled = true;
        //WFB2SE0100OK.Visible = false;
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
            //WFB2SE0100Edit.Visible = false;
            //WFB2SE0101Edit.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
        }
        else
        {
            gv_result.Visible = true;
            gv_result2.Visible = false;
            //WFB2SE0100Edit.Visible = true;
            //WFB2SE0101Edit.Visible = true;
        }


    }
    //進入匯入頁面
    protected void WFB2SE0100Import_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2SE0100_Upload.aspx");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }


}
    
