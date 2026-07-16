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
public partial class WebContent_fb2dg_WFB2DG0300_Qry : BasePage
{
    //Service 物件
    private CFB2DG030BO service = new CFB2DG030BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //getREMAINDER_PARKING_SPOT();
            //系統分類代號下拉式選單
            getPLANT_CD();
            string a = SessionHandle.Current.emp_name;

            ViewState["NewPageIndex"] = 0;
            realeaseConditions();

            //重算各停車場的剩餘數
            calREMAINDER_PARKING_SPOT();
           
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    //private void getREMAINDER_PARKING_SPOT()
    //{
    //    try
    //    {
    //        string CAR_PARK = string.Empty;
    //        //CAR_PARK = ddl_CAR_PARK_NO.SelectedItem.Text;
    //        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
    //        DataTable dt = new DataTable();
    //        dt = service.getREMAINDER_PARKING_SPOT_1();
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                string CAR_PARK_NO = dt.Rows[i]["CAR_PARK_NO"].ToString();
    //                string PARKING_SPOT = dt.Rows[i]["PARKING_SPOT"].ToString();
    //                string USING_PARKING_SPOT = dt.Rows[i]["USING_PARKING_SPOT"].ToString();
    //                fb2dg.CAR_PARK_NO = CAR_PARK_NO;
    //                fb2dg.PARKING_SPOT1 = PARKING_SPOT;
    //                fb2dg.USING_PARKING_SPOT1 = USING_PARKING_SPOT;

    //                service.REMAINDER_PARKING_SPOT_2(fb2dg);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }

    //}

    //private void setQryField()
    //{
    //    Session["DG030_EMP_ID"] = txt_EMP_ID.Text;
    //    Session["DG030_EMP_NAME"] = txt_EMP_NAME.Text;
    //    Session["DG030_DEPT_NO"] = txt_DEPT_NO.Text;
    //    Session["DG030_DEPT_NAME"] = txt_DEPT_NAME.Text;
    //    Session["DG030_PLANT_CD"] = ddl_PLANT_CD.SelectedValue;
    //    Session["DG030_CAR_PARK_NO"] = txt_CAR_PARK_NO.Text;
    //    Session["DG030_CAR_NO"] = txt_CAR_NO.Text;

    //    Session["DG030_Is_Search"] = "Y";
    //}

    private void calREMAINDER_PARKING_SPOT()
    {
        try
        {
            string CAR_PARK = string.Empty;
            //CAR_PARK = ddl_CAR_PARK_NO.SelectedItem.Text;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            DataTable dt = new DataTable();
            dt = service.getREMAINDER_PARKING_SPOT_1();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    service.re_Cal_REMainder(dt.Rows[i]["CAR_PARK_NO"].ToString());
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
            dt = service.getPLANT_CD();
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //ddl_PLANT_CD.Items[1].Selected = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createSYS_ID()
    {
        try
        {
            DataTable dt = get_SYS_ID_Data();
            ddl_PLANT_CD.Items.Clear();
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_PLANT_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_SYS_ID_Data()
    {
        CFB2IB0100DAO fb2sb = new CFB2IB0100DAO();
        return fb2sb.get_SYS_ID_Data();
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }


            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DG030_ddlPerPageRow"] = ViewState["PerPageRow"];

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2DG030Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_EMP_CD");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getPLANT_CD();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }

        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

                //系統代號
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_EMP_CD");

                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getPLANT_CD();
                    ddl1.Items.Add(new ListItem("", "-1"));

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));

                        }
                    }

                }
            }


            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
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
            if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
            {
                gv_result.ShowFooter = true;
                int m = e.Row.Cells.Count;

                for (int i = m - 1; i >= 1; i += -1)
                {
                    e.Row.Cells.RemoveAt(i);

                }
                e.Row.Cells[0].ColumnSpan = m;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                TableCell tc = new TableCell();
                //tc.Attributes["align"] = "left";
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //tc.Attributes["style"] = "width:150px";
                Table t = new Table();
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

                TableRow tr = new TableRow();
                tr.HorizontalAlign = HorizontalAlign.Right;
                //tr.Attributes["style"] = "width:980px";
                tr.Cells.Add(tc);
                tr.Cells.AddAt(0, tc2);

                t.Rows.Add(tr);
                e.Row.Cells[0].Controls.Add(t);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                OnePage.Visible = true;
            }
            else
            {
                OnePage.Visible = false;
            }
            OnePage.Visible = false;
            if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
                gv_result.Visible = true;
            else
                gv_result.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2DG030Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            ViewState["Queryble"] = true;
            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("SYS_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DG030Add.Visible = true;
                WFB2DG030Edit.Visible = true;
                WFB2DG030Delete.Visible = true;
                //WFB2DG030Detail.Visible = true;
            }
            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2DG030Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2DG030Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DG0300_Add.aspx");
    }
    //刪除按鈕事件
    protected void WFB2DG030Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString() + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_EMP_NAME")).Text + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidPARKING_PLANT_CD")).Value + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_DEPT_NO")).Text + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_DEPT_NAME")).Text + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidLEVEL_CD")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidPJOB_CD")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidPJOB_DESC")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidWORK_SHIFT_DESC")).Value + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_CAR_NO")).Text + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidCAR_BRAND")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidCAR_TYPE")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidPARKING_TOOL")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidCAR_PARK_NO")).Value + "|" + ((HiddenField)gv_result.Rows[i].FindControl("HidIFLOW_NO")).Value + "|" + SessionHandle.Current.emp_id);
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DG030Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {

                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2DG030Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                CFB2DG030DAO fb2dg = new CFB2DG030DAO();

            }
            //getSYS_ID();
            //createSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DG030Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2DG030Edit_Click(object sender, EventArgs e)
    {
        int selectrow = -1;
        List<string> sys_id = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {

                sys_id.Add(gv_result.DataKeys[i].Value.ToString());
                selectrow = i;
            }
        }
        string re = string.Format("WFB2DG0300_Update.aspx?id={0}", gv_result.DataKeys[selectrow].Value.ToString());
        Response.Redirect(re);
    }
    //儲存按鈕事件
    protected void WFB2DG030Save_Click(object sender, EventArgs e)
    {

    }
    //取消按鈕事件
    protected void WFB2DG030Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            //WFB2DG030Search.Enabled = true;
            //WFB2DG030Clear.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2DG030Edit.Visible = true;
                WFB2DG030Delete.Visible = true;
            }

            WFB2DG030Save.Visible = false;
            WFB2DG030Cancel.Visible = false;
            WFB2DG030Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DG030Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        //WFB2DG030Search.Enabled = true;
        //WFB2DG030Clear.Visible = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DG030Edit.Visible = true;
            WFB2DG030Delete.Visible = true;
        }

        WFB2DG030Save.Visible = false;
        WFB2DG030Cancel.Visible = false;
        WFB2DG030Add.Visible = true;
    }

    protected void ddl_CAR_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        DropDownList ddl1 = new DropDownList();
        DropDownList ddl2 = new DropDownList();
        //取得該列的DropDownList在將值填入
        if (gv_result.Rows.Count == 0)
        {
            //完全沒值(一開始新增的時候)
            ddl1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SYS_NAME_Add");
        }
        else
        {
            ddl1 = (DropDownList)gv_result.FooterRow.FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.FooterRow.FindControl("ddl_SYS_NAME_Add");
        }
        ddl2.Items.Clear();
        if (ddl != null && ddl2 != null)
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID(ddl1.SelectedValue);
            ddl2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()));
                }
            }

        }
    }




    protected void WFB2DG030Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> sys_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sys_id.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (sys_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (sys_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                string re = string.Format("WFB2DG0300_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2DG030_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DG030_EMP_ID"] = txt_EMP_ID.Text;
            Session["DG030_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DG030_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DG030_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DG030_PLANT_CD"] = ddl_PLANT_CD.SelectedValue;
            Session["DG030_CAR_PARK_NO"] = txt_CAR_PARK_NO.Text;
            Session["DG030_CAR_NO"] = txt_CAR_NO.Text;

            //Session["DG030_Is_Search"] = "Y";
        }
        else
        {
            //Session["DG030_EMP_ID"] = null;
            //Session["DG030_EMP_NAME"] = null;
            //Session["DG030_DEPT_NO"] = null;
            //Session["DG030_DEPT_NAME"] = null;
            //Session["DG030_PLANT_CD"] = null;
            //Session["DG030_CAR_PARK_NO"] = null;
            //Session["DG030_CAR_NO"] = null;
            Session["DG030_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DG030_Is_Search"] == "Y")
            {
                txt_EMP_ID.Text = Session["DG030_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DG030_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DG030_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DG030_DEPT_NAME"].ToString();
                ddl_PLANT_CD.SelectedValue = Session["DG030_PLANT_CD"].ToString();
                txt_CAR_PARK_NO.Text = Session["DG030_CAR_PARK_NO"].ToString();
                txt_CAR_NO.Text = Session["DG030_CAR_NO"].ToString();
                ViewState["PerPageRow"] = Session["DG030_ddlPerPageRow"].ToString();

                WFB2DG030Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion
}


