using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SA1600_AllUpd : BasePage
{
    CFB2SA1600BO sa160BO = new CFB2SA1600BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            ViewState["Queryble"] = false;
            gv_result.PagerSettings.Visible = true;

            if (!IsPostBack)
            {
                initialValue();
                WFB2SA1600AllSave_Search_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }

    }
    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
            sa160DAO.PJOB_CD = hashtable_get("SA1600_ALLUPD_PJOB_CD").ToString();
            sa160DAO.SALARY_ID = hashtable_get("SA1600_ALLUPD_SALARY_ID").ToString();
            sa160DAO.HIRE_TYPE = hashtable_get("SA1600_ALLUPD_HIRE_TYPE").ToString();
            sa160DAO.START_DT = Convert.ToDateTime(hashtable_get("SA1600_ALLUPD_START_DT").ToString()).ToString("yyyy/MM/dd");


            DataTable dt = new DataTable();
            //基本資料
            dt = sa160BO.getUpdData(sa160DAO);

            if (dt.Rows.Count > 0)
            {
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_SALARY_ID.Text = dt.Rows[0]["SALARY_ID_DESC"].ToString();
                txt_HIRE_TYPE.Text = dt.Rows[0]["HIRE_TYPE_DESC"].ToString();
                txt_PAY.Text = Convert.ToInt32(dt.Rows[0]["PAY"].ToString()).ToString("N0");
                hid_PJOB_CD.Value = dt.Rows[0]["PJOB_CD"].ToString();
                hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
                hid_HIRE_TYPE.Value = dt.Rows[0]["HIRE_TYPE"].ToString();
                hid_GEN_EMP_ID.Value = SessionHandle.Current.emp_id;
            }

        
           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    #region GridView的必要function
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
                getSortDirection("EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PJOB_CD", "SALARY_ID", "HIRE_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "SALARY_ID", "HIRE_TYPE"  }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

        }

        //設定Css begin
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
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
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
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "SALARY_ID", "HIRE_TYPE"  }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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



    #endregion


    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            hashtable_set("SA1600_Is_Search", "Y");
            Response.Redirect("WFB2SA1600_Qry.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SA1600AllSave_Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10000);

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //提出核可
    protected void WFB2SA1600AllSave_Click(object sender, EventArgs e)
    {
        try
        {
            
            CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
            sa160DAO.PJOB_CD = hid_PJOB_CD.Value.ToUpper(); ;
            sa160DAO.PAY = txt_PAY.Text.Replace(",","");
            sa160DAO.SALARY_ID = hid_SALARY_ID.Value;
            sa160DAO.HIRE_TYPE = hid_HIRE_TYPE.Value;
            sa160DAO.START_DT = txt_All_START_DT.Text;
            sa160DAO.REMARK = txt_REMARK.Text;
            sa160DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.FUNC_ID = "FB2SA160";

            string msg = "0";

            msg = sa160BO.execSP_S_SA160_SEND(sa160DAO);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "送出失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SA1600_Is_Search", "Y");
                showMessage("releaseSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SA1600_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }
             

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


}