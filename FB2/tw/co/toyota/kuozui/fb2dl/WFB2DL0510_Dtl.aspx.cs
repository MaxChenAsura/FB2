using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2dl_WFB2DL0510_Dtl : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2DL0510BO service = new CFB2DL0510BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        string hr_chg_cd = "";
        string dl_gen_Cd = "";
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["hr_chg_cd"]))) { hr_chg_cd = Request.QueryString["hr_chg_cd"].ToString(); }
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dl_gen_Cd"]))) { dl_gen_Cd = Request.QueryString["dl_gen_Cd"].ToString(); }
        
        gv_result.PagerSettings.Visible = true;
       
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //設定hidden值
            hid_hr_chg_cd.Value = hr_chg_cd;
            hid_dl_gen_Cd.Value = dl_gen_Cd;


            //產生header資料
            getDtlHeader(hr_chg_cd, dl_gen_Cd);
            //取得GRID
            getDtlData();

            
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //取得修改資料
    private void getDtlHeader(string hr_chg_cd,string dl_gen_Cd)
    { 
        try
        {
            DataTable dt = new DataTable();
            dt = service.getData(hr_chg_cd,dl_gen_Cd);
            
            if (dt.Rows.Count > 0)
            {

                txt_HR_CHG_DESC.Text = dt.Rows[0]["HR_CHG_CD_DESC"].ToString();
                txt_IS_BIND_PJOB.Text = dt.Rows[0]["IS_BIND_PJOB"].ToString();
                txt_DL_GEN_CD.Text = dt.Rows[0]["DL_GEN_CD"].ToString();
                txt_SALARY_SETTLE_CD_DESC.Text = dt.Rows[0]["SALARY_SETTLE_CD_DESC"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                txt_PROC_DESC.Text = dt.Rows[0]["PROC_CD_DESC"].ToString();
                txt_LOGI_DESC.Text = dt.Rows[0]["LOGI_CD_DESC"].ToString();
                txt_SDT_DESC.Text = dt.Rows[0]["SDT_CD_DESC"].ToString();
                txt_EDT_DESC.Text = dt.Rows[0]["EDT_CD_DESC"].ToString();
                txt_DL_GENDT_DESC.Text = dt.Rows[0]["DL_GENDT_CD_DESC"].ToString();
                txt_IS_D01_SAME_DESC.Text = dt.Rows[0]["IS_D01_SAME_DESC"].ToString();
            }
            
            hid_USER_UPD.Value = txt_IS_BIND_PJOB.Text;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void getDtlData()
    {
        try
        {
            getGridView("PJOB_CD", 0, 10);

            EditOrAddMode(UIMode.Init, -1);           
            if (gv_result.Rows.Count == 0 &&　txt_IS_BIND_PJOB.Text == "Y")
            {
                showMessage("QryNotFoundMessage");
            }
            if (gv_result.Rows.Count > 0)
            {
                EditOrAddMode(UIMode.Query, -1);
            }
          
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        if (ViewState["SortExpression"] == null)
            getSortDirection("PJOB_CD");    //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD" };
        gv_result.DataBind();
        HID_PageRow.Value = "";
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void ods1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PJOB_CD" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView DataRow = (DataRowView)e.Row.DataItem;
 
                //Add CSS class on normal row.
                if (e.Row.RowState == DataControlRowState.Normal)
                    e.Row.CssClass = "normal";

                //Add CSS class on alternate row.
                if (e.Row.RowState == DataControlRowState.Alternate ||
                                   e.Row.RowState == DataControlRowState.Selected)
                    e.Row.CssClass = "alternate";
               
            }

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
        catch (Exception ex)
        {
        }
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (hid_USER_UPD.Value == "N")
            {
                gv_result.Columns[0].Visible = false;
            }
            else
            {
                gv_result.Columns[0].Visible = true;
            }

            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    #endregion

    #region "button event"
    //新增按鈕事件
    protected void WFB2DL0511Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("PJOB_CD", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("PJOB_CD", 0, 10);
            }
            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2DL0511Del_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteDtlList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteDtlList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            string msg = "";
            msg = service.deleteDtlData(deleteDtlList, hid_hr_chg_cd.Value, hid_dl_gen_Cd.Value);

            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            if (gv_result.Rows.Count == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0511Del, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //儲存按鈕事件
    protected void WFB2DL0511Save_Click(object sender, EventArgs e)
    {
        try
        {
            
            string msg = string.Empty;
            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();

            Control KeyinRow = null;

            
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0]; //無筆數新增
            else
            {
               
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;  //有筆數新增
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];  //修改
            }

            dl051DAO.HR_CHG_CD = hid_hr_chg_cd.Value;
            dl051DAO.DL_GEN_CD = hid_dl_gen_Cd.Value;

           

            //新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                dl051DAO.PJOB_CD = ((TextBox)KeyinRow.FindControl("txt_NEW_PJOB_CD")).Text.Trim().ToUpper();
                dl051DAO.CREATED_BY = SessionHandle.Current.emp_id;
                dl051DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dl051DAO.FUNC_ID = "FB2DL051";


                msg = service.addDtlData(dl051DAO);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
            }
            else
            {
                //修改
            }
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            if (gv_result.Rows.Count == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        if (gv_result.Rows.Count == 0)
        {
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    //回上頁按鈕事件
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0510_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0510_Qry.aspx");
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DL0511Add.Visible = false;
                WFB2DL0511Del.Visible = false;
                btn_back.Visible = false;
                WFB2DL0511Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DL0511Add.Visible = false;
                WFB2DL0511Del.Visible = false;
                btn_back.Visible = false;
                WFB2DL0511Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                if (hid_USER_UPD.Value == "Y")
                {
                    WFB2DL0511Add.Visible = true;
                    WFB2DL0511Del.Visible = true;
                }
                else
                {
                    WFB2DL0511Add.Visible = false;
                    WFB2DL0511Del.Visible = false;
                }
                btn_back.Visible = true;
                gv_result.Visible = true;
                WFB2DL0511Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                if (hid_USER_UPD.Value == "Y")
                {
                    WFB2DL0511Add.Visible = true;
                    WFB2DL0511Del.Visible = false;
                }
                else
                {
                    WFB2DL0511Add.Visible = false;
                    WFB2DL0511Del.Visible = false;
                }
                btn_back.Visible = true;
                WFB2DL0511Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion
}

