using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2SJ0150_Dtl : BasePage 
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

            initialValue();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ0150DAO sj0150DAO = new CFB2SJ0150DAO();
            sj0150DAO.ASSESS_YEAR = hashtable_get("SJ0150_UPD_ASSESS_YEAR").ToString();
            sj0150DAO.ASSESS_TYPE = hashtable_get("SJ0150_UPD_ASSESS_TYPE").ToString();
            sj0150DAO.GRP_CD = hashtable_get("SJ0150_UPD_GRP_CD").ToString();

            DataTable dt = new DataTable();
         
            //基本資料
            dt = service.getUpdData(sj0150DAO);

            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                hid_ASSESS_YEAR.Value = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                txt_GRP_CD.Text = dt.Rows[0]["GRP_CD"].ToString();
                hid_GRP_CD.Value = dt.Rows[0]["GRP_CD"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_GRP_NAME.Text = dt.Rows[0]["GRP_NAME"].ToString();
                txt_REDEPLOY_YN.Text = dt.Rows[0]["REDEPLOY_YN"].ToString();
                if (dt.Rows[0]["REDEPLOY_YN"].ToString() == "N")
                {
                    txt_IS_CTL.Text = "N";
                }
                else
                {
                    txt_IS_CTL.Text = "Y";
                }
            }
            this.WFB2SJ0150Search_Click(null, null);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD", "LEVEL_CD" }; //設定GridView Key

            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0150_ddlPerPageRow", ViewState["PerPageRow"]);
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
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            //GridView有分頁此段必加 begin

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0150Add.Visible = true;
                //WFB2SJ0150Edit.Visible = true;
                WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                //WFB2SJ0150Edit.Visible = false;
                WFB2SJ0150Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
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
            //WFB2SJ0150Search.Visible = false;
            //btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ", 0, 10);

            WFB2SJ0150Save.Visible = true;
            WFB2SJ0150Cancel.Visible = true;

            WFB2SJ0150Add.Visible = false;
            //WFB2SJ0150Edit.Visible = false;
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
            List<Tuple<string, string, string, string>> target_type =
                new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    target_type.Add(
                        new Tuple<string, string, string, string>(
                            gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString(),
                            gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString(),
                            gv_result.DataKeys[i].Values["GRP_CD"].ToString(),
                            gv_result.DataKeys[i].Values["LEVEL_CD"].ToString()));
                   
                }
            }

            string msg = service.DeleteDtl(target_type);
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

   
   
    //確認按鈕
    protected void WFB2SJ0150Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                
                DropDownList LEVEL_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_LEVEL_CD");

               
                CFB2SJ0150DAO wfb2sj = new CFB2SJ0150DAO();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ASSESS_YEAR.Text +";"+ASSESS_TYPE.SelectedValue +";"+ WS_CD.SelectedValue +";"+GRP_CD.Text +";"+GRP_NAME.Text +";"+ "');", true);
                wfb2sj.ASSESS_YEAR = hid_ASSESS_YEAR.Value.ToUpper();
                wfb2sj.ASSESS_TYPE = hid_ASSESS_TYPE.Value.ToUpper();
                wfb2sj.GRP_CD = hid_GRP_CD.Value;
                wfb2sj.WS_CD = txt_WS_CD.Text;
                wfb2sj.LEVEL_CD =  LEVEL_CD.SelectedValue;
                wfb2sj.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2sj.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2sj.FUNC_ID = "FB2SJ015";
             
                string msg = service.Add_Dtl(wfb2sj);
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
                    DropDownList LEVEL_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_LEVEL_CD");

               
                    CFB2SJ0150DAO wfb2sj = new CFB2SJ0150DAO();
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ASSESS_YEAR.Text +";"+ASSESS_TYPE.SelectedValue +";"+ WS_CD.SelectedValue +";"+GRP_CD.Text +";"+GRP_NAME.Text +";"+ "');", true);
                    wfb2sj.ASSESS_YEAR = hid_ASSESS_YEAR.Value.ToUpper();
                    wfb2sj.ASSESS_TYPE = hid_ASSESS_TYPE.Value.ToUpper();
                    wfb2sj.GRP_CD = hid_GRP_CD.Value;
                    wfb2sj.WS_CD = txt_WS_CD.Text;
                    wfb2sj.LEVEL_CD =  LEVEL_CD.SelectedValue;
                    wfb2sj.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2sj.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2sj.FUNC_ID = "FB2SJ015";


                    string msg = service.Add_Dtl(wfb2sj);
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
               
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD", "LEVEL_CD" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            //WFB2SJ0150Search.Visible = true;
            //btn_clear.Visible = true;

            WFB2SJ0150Save.Visible = false;
            WFB2SJ0150Cancel.Visible = false;
            WFB2SJ0150Add.Visible = true;
            //WFB2SJ0150Edit.Visible = true;
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
        //WFB2SJ0150Search.Visible = true;
        btn_cancel.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            //WFB2SJ0150Edit.Visible = true;
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD", "LEVEL_CD" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_CD");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getLevelData();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
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
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "GRP_CD", "LEVEL_CD" };
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
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0150_Is_Search", "Y");
        Response.Redirect("WFB2SJ0150_Qry.aspx");
    }
}