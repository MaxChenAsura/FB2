using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2HB_WFB2HB0100_Qry : BasePage
{
    //Service 物件
    private CFB2HB0100BO service = new CFB2HB0100BO();


    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //員工區分
            getEMP_CD();
            //職種
            getWS_CD();
            //聘用單位
            getCOMPANY_CD();
            //工廠區分
            getPLANT_CD();
            //資格代號
            getLEVEL_CD();
            //工數區分
            getWORK_CD();
            //日籍會社
            getJPN_CD();

            createddl_EMP_CHG_CD();

            if (Session["HB0100_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }

        if (HID_PageRow.Value != "")
        {

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void createddl_EMP_CHG_CD()
    {
        //在職區分
        DataTable dt = utilities.getCommCode("HB", "EMP_CHG_CD", "", "");
        ddl_EMP_CHG_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
            //ddl_EMP_CHG_CD.SelectedValue = "11";
        }
    }

    private void getEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWORK_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getJPN_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("JPN_CD", "", "");
            ddl_JPN_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_JPN_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getLEVEL_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLEVEL_CD();
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

    private void getCOMPANY_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCOMPANY_CD();
            ddl_COMPANY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["COMPANY_CD"].ToString() + "-" + dt.Rows[i]["COMPANY_SNAME"].ToString(), dt.Rows[i]["COMPANY_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWS_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_EMP_ID.Text = Session["HB0100_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["HB0100_EMP_NAME"].ToString();
            ddl_EMP_CD.SelectedValue = Session["HB0100_EMP_CD"].ToString();
            ddl_EMP_CHG_CD.SelectedValue = Session["HB0100_EMP_CHG_CD"].ToString();
            txt_LICENSE_ID.Text = Session["HB0100_LICENSE_ID"].ToString();
            txt_DEPT_NO.Text = Session["HB0100_DEPT_NO"].ToString();
            txt_DEPT_NAME.Text = Session["HB0100_DEPT_NAME"].ToString();
            ddl_WS_CD.SelectedValue = Session["HB0100_WS_CD"].ToString();
            ddl_COMPANY_CD.SelectedValue = Session["HB0100_COMPANY_CD"].ToString();
            ddl_PLANT_CD.SelectedValue = Session["HB0100_PLANT_CD"].ToString();
            ddl_LEVEL_CD.SelectedValue = Session["HB0100_LEVEL_CD"].ToString();
            txt_PJOB_CD.Text = Session["HB0100_PJOB_CD"].ToString();
            txt_PJOB_DESC.Text = Session["HB0100_PJOB_DESC"].ToString();
            ddl_WORK_CD.SelectedValue = Session["HB0100_WORK_CD"].ToString();
            ddl_JPN_CD.SelectedValue = Session["HB0100_JPN_CD"].ToString();
            txt_WORK_SHIFT_CD.Text = Session["HB0100_WORK_SHIFT_CD"].ToString();
            txt_WORK_SHIFT_DESC.Text = Session["HB0100_WORK_SHIFT_DESC"].ToString();
            txt_JOIN_DT_S.Text = Session["HB0100_JOIN_DT_S"].ToString();
            txt_JOIN_DT_E.Text = Session["HB0100_JOIN_DT_E"].ToString();
            txt_LEAVE_DT_S.Text = Session["HB0100_LEAVE_DT_S"].ToString();
            txt_LEAVE_DT_E.Text = Session["HB0100_LEAVE_DT_E"].ToString();
            txt_BE_CONTRACT_DT_S.Text = Session["HB0100_BE_CONTRACT_DT_S"].ToString();
            txt_BE_CONTRACT_DT_E.Text = Session["HB0100_BE_CONTRACT_DT_E"].ToString();
            txt_BE_EMP_DT_S.Text = Session["HB0100_BE_EMP_DT_S"].ToString();
            txt_BE_EMP_DT_E.Text = Session["HB0100_BE_EMP_DT_E"].ToString();
            cb_IS_DUTY_CHECK.Checked = Convert.ToBoolean(Session["HB0100_IS_DUTY_CHECK"]);
            cb_OVERTIME_CTL_CD.Checked = Convert.ToBoolean(Session["HB0100_OVERTIME_CTL_CD"]);
            cb_MODEL.Checked = Convert.ToBoolean(Session["HB0100_MODEL"]);
            cb_IS_UPD_HEAD.Checked = Convert.ToBoolean(Session["HB0100_IS_UPD_HEAD"]);
            cb_UNION_PJOB_CD.Checked = Convert.ToBoolean(Session["HB0100_UNION_PJOB_CD"]);
            ViewState["PerPageRow"] = Session["HB0100_ddlPerPageRow"].ToString();
            WFB2HB0100Search_Click(null, null);
            Session["HB0100_Is_Search"] = "N";
        }
        catch
        {
        }
    }
    private void setQryField()
    {
        Session["HB0100_EMP_ID"] = txt_EMP_ID.Text;
        Session["HB0100_EMP_NAME"] = txt_EMP_NAME.Text;
        Session["HB0100_EMP_CD"] = ddl_EMP_CD.SelectedValue;
        Session["HB0100_LICENSE_ID"] = txt_LICENSE_ID.Text;
        Session["HB0100_DEPT_NO"] = txt_DEPT_NO.Text;
        Session["HB0100_DEPT_NAME"] = txt_DEPT_NAME.Text;
        Session["HB0100_EMP_CHG_CD"] = ddl_EMP_CHG_CD.SelectedValue;
        Session["HB0100_WS_CD"] = ddl_WS_CD.SelectedValue;
        Session["HB0100_COMPANY_CD"] = ddl_COMPANY_CD.SelectedValue;
        Session["HB0100_PLANT_CD"] = ddl_PLANT_CD.SelectedValue;
        Session["HB0100_LEVEL_CD"] = ddl_LEVEL_CD.SelectedValue;
        Session["HB0100_PJOB_CD"] = txt_PJOB_CD.Text;
        Session["HB0100_PJOB_DESC"] = txt_PJOB_DESC.Text;

        Session["HB0100_WORK_CD"] = ddl_WORK_CD.SelectedValue;
        Session["HB0100_JPN_CD"] = ddl_JPN_CD.SelectedValue;
        Session["HB0100_WORK_SHIFT_CD"] = txt_WORK_SHIFT_CD.Text;
        Session["HB0100_WORK_SHIFT_DESC"] = txt_WORK_SHIFT_DESC.Text;
        Session["HB0100_JOIN_DT_S"] = txt_JOIN_DT_S.Text;
        Session["HB0100_JOIN_DT_E"] = txt_JOIN_DT_E.Text;

        Session["HB0100_LEAVE_DT_S"] = txt_LEAVE_DT_S.Text;
        Session["HB0100_LEAVE_DT_E"] = txt_LEAVE_DT_E.Text;
        Session["HB0100_BE_CONTRACT_DT_S"] = txt_BE_CONTRACT_DT_S.Text;
        Session["HB0100_BE_CONTRACT_DT_E"] = txt_BE_CONTRACT_DT_E.Text;
        Session["HB0100_BE_EMP_DT_S"] = txt_BE_EMP_DT_S.Text;
        Session["HB0100_BE_EMP_DT_E"] = txt_BE_EMP_DT_E.Text;

        Session["HB0100_IS_DUTY_CHECK"] = cb_IS_DUTY_CHECK.Checked;
        Session["HB0100_OVERTIME_CTL_CD"] = cb_OVERTIME_CTL_CD.Checked;
        Session["HB0100_MODEL"] = cb_MODEL.Checked;
        Session["HB0100_IS_UPD_HEAD"] = cb_IS_UPD_HEAD.Checked;
        Session["HB0100_UNION_PJOB_CD"] = cb_UNION_PJOB_CD.Checked;
    }
    #endregion

    protected void WFB2HB0100Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HB0100_Add.aspx");
    }
    protected void WFB2HB0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            if (gv_result.Rows.Count > 0)
            {
                WFB2HB0100Add.Visible = true;
                WFB2HB0100Edit.Visible = true;
                WFB2HB0100Detail.Visible = true;
                gv_result.ShowFooter = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料')", true);

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");

            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.DataBind();


            HID_PageRow.Value = "";
            Session["HB0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
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

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
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
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }
    protected void WFB2HB0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2HB0100_Mod.aspx?mod=mod&emp_id=" + emp_id[0]);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HB0100UpdHead_Click(object sender, EventArgs e)
    {
        try
        {
            //主管更新
            string msg = service.callSP();
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0100Upload_Click(object sender, EventArgs e)
    {

    }
    protected void WFB2HB0100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2HB0100_Dtl.aspx?emp_id=" + emp_id[0]);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_DEPT_NO.Text.Trim() != "")
            {
                DataTable dt = service.getDEPT_DATA(txt_DEPT_NO.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void txt_PJOB_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_PJOB_CD.Text.Trim() != "")
            {
                DataTable dt = service.getPJOB_DATA(txt_PJOB_CD.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_WORK_SHIFT_CD.Text.Trim() != "")
            {
                DataTable dt = service.getWorkShift(txt_WORK_SHIFT_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_WORK_SHIFT_DESC.Text = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
 
}