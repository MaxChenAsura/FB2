using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2SJ0150_Qry : BasePage 
{
    //Service 物件
    private CFB2SJ0150BO service = new CFB2SJ0150BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            DataTable dt = new DataTable();
            //
            //考核類型
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

            //查詢條件及自動查詢
            getQryField();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    

   
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

           
            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("ASSESS_YEAR", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD" }; //設定GridView Key

            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0130_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SJ0150Search_Click(object sender, EventArgs e)
    {
        if (txt_ASSESS_YEAR.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核年度!');", true);
            return;
        }
        if (ddl_ASSESS_TYPE.SelectedValue == "-1")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核類別!');", true);
            return;
        }
        try
        {
            //保留查詢條件
            setQryField(true);

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            //GridView有分頁此段必加 begin

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0150Add.Visible = true;
                WFB2SJ0150Edit.Visible = true;
                WFB2SJ0150Dtl.Visible = true;
                WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                WFB2SJ0150Edit.Visible = false;
                WFB2SJ0150Dtl.Visible = false;
                WFB2SJ0150Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2SJ0150Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2SJ0150Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD", 0, 10);

            WFB2SJ0150Save.Visible = true;
            WFB2SJ0150Cancel.Visible = true;

            WFB2SJ0150Add.Visible = false;
            WFB2SJ0150Edit.Visible = false;
            WFB2SJ0150Dtl.Visible = false;
            WFB2SJ0150Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;

            gv_result.Visible = true; 
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除按鈕事件
    protected void WFB2SJ0150Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string>> target_type =
                new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    target_type.Add(
                        new Tuple<string, string, string>(
                            gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString(),
                            gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString(),
                            gv_result.DataKeys[i].Values["GRP_CD"].ToString()));

                }
            }

            string msg = service.Delete(target_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改按鈕事件
    protected void WFB2SJ0150Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2SJ0150Search.Visible = false;
            btn_clear.Visible = false;

            WFB2SJ0150Save.Visible = true;
            WFB2SJ0150Cancel.Visible = true;

            WFB2SJ0150Add.Visible = false;
            WFB2SJ0150Edit.Visible = false;
            WFB2SJ0150Dtl.Visible = false;
            WFB2SJ0150Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2SJ0150Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                // 儲存 換頁條件
                hashtable_set("SJ0150_UPD_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0150_UPD_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0150_UPD_GRP_CD", gv_result.DataKeys[editindex[0]].Values["GRP_CD"].ToString());
                //hashtable_set("SA1600_UPD_SALARY_ID", gv_result.DataKeys[editindex[0]].Values["SALARY_ID"].ToString());
                //hashtable_set("SA1600_UPD_HIRE_TYPE", gv_result.DataKeys[editindex[0]].Values["HIRE_TYPE"].ToString());
                //hashtable_set("SA1600_UPD_START_DT", gv_result.DataKeys[editindex[0]].Values["START_DT"].ToString());
                Response.Redirect("WFB2SJ0150_Dtl.aspx?");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //確認按鈕
    protected void WFB2SJ0150Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox ASSESS_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ASSESS_YEAR");
                DropDownList ASSESS_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_ASSESS_TYPE");
                DropDownList WS_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_WS_CD");
                DropDownList REDEPLOY_YN = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_REDEPLOY_YN");
                DropDownList REPORT_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_REPORT_TYPE");
                DropDownList IS_CTL = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_IS_CTL");
                TextBox GRP_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_GRP_CD");
                TextBox GRP_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_GRP_NAME");

               
                CFB2SJ0150DAO wfb2sj = new CFB2SJ0150DAO();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ASSESS_YEAR.Text +";"+ASSESS_TYPE.SelectedValue +";"+ WS_CD.SelectedValue +";"+GRP_CD.Text +";"+GRP_NAME.Text +";"+ "');", true);
                wfb2sj.ASSESS_YEAR = ASSESS_YEAR.Text;
                wfb2sj.ASSESS_TYPE = ASSESS_TYPE.SelectedValue;
                wfb2sj.WS_CD = WS_CD.SelectedValue;
                wfb2sj.GRP_CD = GRP_CD.Text;
                wfb2sj.GRP_NAME = GRP_NAME.Text;
                wfb2sj.REDEPLOY_YN = REDEPLOY_YN.SelectedValue;
                wfb2sj.REPORT_TYPE = REPORT_TYPE.SelectedValue;
                wfb2sj.IS_CTL = IS_CTL.SelectedValue;
                wfb2sj.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2sj.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2sj.FUNC_ID = "FB2SJ015";
               
                string msg = service.Add(wfb2sj);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox ASSESS_YEAR = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ASSESS_YEAR");
                    DropDownList ASSESS_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_ASSESS_TYPE");
                    DropDownList WS_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_WS_CD");
                    DropDownList REDEPLOY_YN = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_REDEPLOY_YN");
                    DropDownList REPORT_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_REPORT_TYPE");
                    DropDownList IS_CTL = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_IS_CTL");
               
                    TextBox GRP_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_GRP_CD");
                    TextBox GRP_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_GRP_NAME");

                    CFB2SJ0150DAO wfb2sj = new CFB2SJ0150DAO();
                    wfb2sj.ASSESS_YEAR = ASSESS_YEAR.Text;
                    wfb2sj.ASSESS_TYPE = ASSESS_TYPE.SelectedValue;
                    wfb2sj.WS_CD = WS_CD.SelectedValue;
                    wfb2sj.GRP_CD = GRP_CD.Text;
                    wfb2sj.GRP_CD = GRP_CD.Text;
                    wfb2sj.GRP_NAME = GRP_NAME.Text;
                    wfb2sj.REDEPLOY_YN = REDEPLOY_YN.SelectedValue;
                    wfb2sj.REPORT_TYPE = REPORT_TYPE.SelectedValue;
                    wfb2sj.IS_CTL = IS_CTL.SelectedValue;
                    wfb2sj.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2sj.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2sj.FUNC_ID = "FB2SJ015";



                    string msg = service.Add(wfb2sj);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    Control KeyinRow = null;
                    if (gv_result.Rows.Count == 0)
                        KeyinRow = gv_result.Controls[0].Controls[0];
                    else
                    {
                        if (gv_result.EditIndex == -1)
                            KeyinRow = gv_result.FooterRow;
                        else
                            KeyinRow = gv_result.Rows[gv_result.EditIndex];
                    }
                    //更新
                    DropDownList WS_CD = (DropDownList)KeyinRow.FindControl("ddl_WS_CD");
                    DropDownList REDEPLOY_YN = (DropDownList)KeyinRow.FindControl("ddl_REDEPLOY_YN");
                    DropDownList REPORT_TYPE = (DropDownList)KeyinRow.FindControl("ddl_REPORT_TYPE");
                    DropDownList IS_CTL = (DropDownList)KeyinRow.FindControl("ddl_IS_CTL");
                    TextBox GRP_NAME = (TextBox)KeyinRow.FindControl("txt_GRP_NAME");
                    CFB2SJ0150DAO wfb2sj = new CFB2SJ0150DAO();
                    wfb2sj.ASSESS_YEAR = gv_result.DataKeys[gv_result.EditIndex].Values["ASSESS_YEAR"].ToString();
                    wfb2sj.ASSESS_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["ASSESS_TYPE"].ToString();
                    wfb2sj.GRP_CD = gv_result.DataKeys[gv_result.EditIndex].Values["GRP_CD"].ToString();
                    wfb2sj.WS_CD = WS_CD.SelectedValue;
                    wfb2sj.GRP_NAME = GRP_NAME.Text;
                    wfb2sj.REDEPLOY_YN = REDEPLOY_YN.SelectedValue;
                    wfb2sj.REPORT_TYPE = REPORT_TYPE.SelectedValue;
                    wfb2sj.IS_CTL = IS_CTL.SelectedValue;
                    wfb2sj.FUNC_ID = "FB2SJ015";
                    wfb2sj.UPDATED_BY = SessionHandle.Current.emp_id;
                    string msg = service.Update(wfb2sj);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2SJ0150Search.Visible = true;
            btn_clear.Visible = true;

            WFB2SJ0150Save.Visible = false;
            WFB2SJ0150Cancel.Visible = false;
            WFB2SJ0150Add.Visible = true;
            WFB2SJ0150Edit.Visible = true;
            WFB2SJ0150Dtl.Visible = true;
            WFB2SJ0150Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    

    //取消按鈕
    protected void WFB2SJ0150Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2SJ0150Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SJ0150Edit.Visible = true;
            WFB2SJ0150Dtl.Visible = true;
            WFB2SJ0150Delete.Visible = true;
        }

        WFB2SJ0150Save.Visible = false;
        WFB2SJ0150Cancel.Visible = false;
        WFB2SJ0150Add.Visible = true;
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_ASSESS_TYPE");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_WS_CD");
            if (ddl != null)
            {
                dt = utilities.getCommCode("HB", "WS_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
            
            ddl = (DropDownList)e.Row.FindControl("ddl_WS_CD");
            if (ddl != null)
            {
                dt = utilities.getCommCode("HB", "WS_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_REDEPLOY_YN");
            ddl.Items.Add(new ListItem("", "-1"));
            ddl.Items.Add(new ListItem("Y", "Y"));
            ddl.Items.Add(new ListItem("N", "N"));

            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_IS_CTL");
            ddl.Items.Add(new ListItem("", "-1"));
            ddl.Items.Add(new ListItem("Y", "Y"));
            ddl.Items.Add(new ListItem("N", "N"));

            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_REPORT_TYPE");
            if (ddl != null)
            {
                dt = utilities.getCommCode("SJ", "REPORT_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            //gv_result.ShowFooter = false;

        }

    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_WS_CD");
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_REDEPLOY_YN");
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_REPORT_TYPE");
            DropDownList ddl4 = (DropDownList)e.Row.FindControl("ddl_IS_CTL");

            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            DataTable dt = new DataTable();
           if (ddl1 != null)
            {
                //txt.Enabled = false;
                
                dt = utilities.getCommCode("HB", "WS_CD", "", "");
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }
           if (ddl2 != null)
           {
               //txt.Enabled = false;
               dt = new DataTable();
               ddl2.Items.Add(new ListItem("", "-1"));
               ddl2.Items.Add(new ListItem("Y", "Y"));
               ddl2.Items.Add(new ListItem("N", "N"));
               //if (hid != null)
               //    ddl.SelectedValue = hid.Value;
           }
           if (ddl3 != null)
           {
               //txt.Enabled = false;

               dt = utilities.getCommCode("SJ", "REPORT_TYPE", "", "");
               ddl3.Items.Add(new ListItem("", "-1"));
               if (dt.Rows.Count > 0)
               {
                   for (int i = 0; i < dt.Rows.Count; i++)
                   {
                       ddl3.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                   }
               }
               if (ddl4 != null)
               {
                   //txt.Enabled = false;
                  // dt = new DataTable();
                   ddl4.Items.Add(new ListItem("", "-1"));
                   ddl4.Items.Add(new ListItem("Y", "Y"));
                   ddl4.Items.Add(new ListItem("N", "N"));
                   //if (hid != null)
                   //    ddl.SelectedValue = hid.Value;
               }
               //if (hid != null)
               //    ddl.SelectedValue = hid.Value;
           }
            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_WS_CD")).SelectedValue = Convert.ToString(DataRow["WS_CD"]);
                ((DropDownList)e.Row.FindControl("ddl_REDEPLOY_YN")).SelectedValue = Convert.ToString(DataRow["REDEPLOY_YN"]);
                ((DropDownList)e.Row.FindControl("ddl_REPORT_TYPE")).SelectedValue = Convert.ToString(DataRow["REPORT_TYPE"]);
                ((DropDownList)e.Row.FindControl("ddl_IS_CTL")).SelectedValue = Convert.ToString(DataRow["IS_CTL"]);

            }

        }

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

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0150_Is_Search").ToString() == "Y")
            {

                txt_ASSESS_YEAR.Text = hashtable_get("SJ0150_txt_ASSESS_YEAR").ToString();

                ViewState["PerPageRow"] = hashtable_get("SJ0150_ddlPerPageRow").ToString();
                WFB2SJ0150Search_Click(null, null);
                setQryField(false);
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
            //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("SJ0150_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
        }
        else
        {
            hashtable_set("SJ0150_Is_Search", "N");
        }
    }




    #endregion
}