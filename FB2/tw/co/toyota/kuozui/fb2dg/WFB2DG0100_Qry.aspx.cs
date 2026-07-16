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
public partial class WebContent_fb2dg_WFB2DG0100_Qry : BasePage
{
    //Service 物件
    private CFB2DG010BO service = new CFB2DG010BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //系統分類代號下拉式選單            
            getSYS_ID();
            //getData();
            ViewState["NewPageIndex"] = 0;
            //重算各停車場的剩餘數
            calREMAINDER_PARKING_SPOT();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void calREMAINDER_PARKING_SPOT()
    {
        try
        {
            string CAR_PARK = string.Empty;
            //CAR_PARK = ddl_CAR_PARK_NO.SelectedItem.Text;
            CFB2DG010DAO fb2dg = new CFB2DG010DAO();
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
    private void getSYS_ID()
    {
        try
        {
            //ddl
            DataTable dt = new DataTable();
            CFB2DG010DAO fb2dg = new CFB2DG010DAO();
            fb2dg.SYSCODE = Syscode();
            dt = service.getSYS_ID(fb2dg);
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(string.Format("{0}-{1}",dt.Rows[i]["SUB_CD"].ToString(),dt.Rows[i]["SUB_DESC"].ToString()),dt.Rows[i]["SUB_CD"].ToString()));
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
        CFB2DG010DAO fb2dg = new CFB2DG010DAO();
        return fb2dg.get_SYS_ID_Data();
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
                getSortDirection("CAR_PARK_NO");    //排序方式(BasePage.cs)

            //gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CAR_PARK_NO" }; //設定GridView Key
            gv_result.DataBind();


            HID_PageRow.Value = ""; //GridView有分頁此段必加

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DG010Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "CAR_PARK_NO" }; //設定GridView Key
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
            //給下拉選單預設值
            string st1 = ((HiddenField)e.Row.FindControl("hid_NEEDSELECT")).Value;
            ((DropDownList)e.Row.FindControl("ddl_NEEDSELECT_Add")).SelectedValue = st1;
        }

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_PLANT_CD_Add");
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                CFB2DG010DAO fb2dg = new CFB2DG010DAO();
                fb2dg.SYSCODE = Syscode();
                dt = service.getSYS_ID(fb2dg);
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()));
                    }
                    
                }
                
                    
            }
            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_PLANT_CD_Add")).SelectedIndex = Convert.ToInt32(DataRow["PLANT_CD"]);
                string x = Convert.ToString(DataRow["PLANT_CD"]);

               
            }
                    
                        
                   
               
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_PARKING_TYPE_Add");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl2 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getPARKING_TYPE();
                ddl2.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }
            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {


                ((DropDownList)e.Row.FindControl("ddl_PARKING_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["SUBPARKING_TYPE"]);
            }
            
        }

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView DataRow = (DataRowView)e.Row.DataItem;

        //    //Add CSS class on normal row.
        //    if (e.Row.RowState == DataControlRowState.Normal)
        //        e.Row.CssClass = "normal";

        //    //Add CSS class on alternate row.
        //    if (e.Row.RowState == DataControlRowState.Alternate ||
        //                       e.Row.RowState == DataControlRowState.Selected)
        //        e.Row.CssClass = "alternate";

        //}

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
        try
        {//設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

                //系統代號
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_PLANT_CD_Add");
               
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    CFB2DG010DAO fb2dg = new CFB2DG010DAO();
                    fb2dg.SYSCODE = Syscode();
                    dt = service.getSYS_ID(fb2dg);
                    ddl1.Items.Add(new ListItem("", "-1"));
                   
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(string.Format("{0}-{1}",dt.Rows[i]["SUB_CD"].ToString(),dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                           
                        }
                    }

                }
                DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_PARKING_TYPE_Add");

                if (ddl2 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getPARKING_TYPE();
                    ddl2.Items.Add(new ListItem("", "-1"));

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl2.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));

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
                t.HorizontalAlign = HorizontalAlign.Left;

                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                //ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
            if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
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
            //OnePage.Visible = false;
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
        gv_result.DataKeyNames = new string[] { "CAR_PARK_NO" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2DG010Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("SYS_ID", 0, 10);
            //end

            ////GridView有分頁此段必加 begin
            //if (Convert.ToString(ViewState["PerPageRow"]) != "")
            //{
            //    this.Page.FindControl("ddlPerPageRow");
            //    getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            //}
            //else
            //{
            //    getGridView("SYS_ID", 0, 10);
            //}
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }



            if (gv_result.Rows.Count > 0)
            {
                WFB2DG010Add.Visible = true;
                WFB2DG010Edit.Visible = true;
                WFB2DG010Delete.Visible = true;
                //WFB2DG010Detail.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2DG010Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2DG010Add_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;

            WFB2DG010Search.Enabled = false;
            WFB2DG010Clear.Enabled = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("SYS_ID", 0, 10);

            WFB2DG010Save.Visible = true;
            WFB2DG010Cancel.Visible = true;

            WFB2DG010Add.Visible = false;
            WFB2DG010Edit.Visible = false;
            WFB2DG010Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;


            //int oldPageIndex = this.gv_result.PageIndex;

            //if (this.gv_result.PageIndex > 0)
            //    getGridView("SYS_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            //else
            //{
            //    this.gv_result.Visible = true;
            //    getGridView("SYS_ID", 0, 10);
            //}
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2DG010Delete_Click(object sender, EventArgs e)
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
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DG010Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2DG010Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            }
            //getSYS_ID();
            //createSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DG010Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2DG010Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;

            //disable查詢清除按鈕
            WFB2DG010Search.Enabled = false;
            WFB2DG010Clear.Enabled = false;
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
                ScriptManager.RegisterClientScriptBlock(WFB2DG010Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DG010Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }
            WFB2DG010Save.Visible = true;
            WFB2DG010Cancel.Visible = true;

            WFB2DG010Add.Visible = false;
            WFB2DG010Edit.Visible = false;
            WFB2DG010Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DG010Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2DG010Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DG010DAO fb2dg = new CFB2DG010DAO();
            CFB2DG010BO service = new CFB2DG010BO();
            string msg = "";
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

            //fb2dg.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;
            
            fb2dg.UPDATED_BY = SessionHandle.Current.emp_name;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;                
                fb2dg.CAR_PARK_NO = ((TextBox)KeyinRow.FindControl("txt_CAR_PARK_NO_Add")).Text;        //停車場代號
                fb2dg.PLANT_CD = ((DropDownList)KeyinRow.FindControl("ddl_PLANT_CD_Add")).Text;         //工廠區分
                fb2dg.PARKING_NAME = ((TextBox)KeyinRow.FindControl("txt_PARKING_NAME_Add")).Text;      //停車場名稱
                fb2dg.PARKING_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_PARKING_TYPE_Add")).Text; //停車場類別
                fb2dg.PARKING_SPOT = ((TextBox)KeyinRow.FindControl("txt_PARKING_SPOT_Add")).Text;      //車位數
                fb2dg.USING_PARKING_SPOT = service.getUSING_PARKING_SPOT(fb2dg.CAR_PARK_NO);            //已使用數
                fb2dg.OVERLAP = ((TextBox)KeyinRow.FindControl("txt_OVERLAP_Add")).Text;                //重疉率
                fb2dg.NEEDSELECT = ((DropDownList)KeyinRow.FindControl("ddl_NEEDSELECT_Add")).Text; //是否必須選取
                //fb2dg.REMAINDER_PARKING_SPOT = service.getREMAINDER_PARKING_SPOT(fb2dg.CAR_PARK_NO, fb2dg.PARKING_SPOT, fb2dg.OVERLAP);
                fb2dg.FUNC_ID = "FB2DG010";
                
                fb2dg.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData(fb2dg);

                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2DG010Save, this.GetType(), "success", "history.back(-4);", true);
                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "EMP_ID" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false;

                    //enable查詢清除按鈕
                    WFB2DG010Search.Enabled = true;
                    WFB2DG010Clear.Enabled = true;

                    WFB2DG010Save.Visible = false;
                    WFB2DG010Cancel.Visible = false;
                    WFB2DG010Add.Visible = true;
                    WFB2DG010Edit.Visible = true;
                    WFB2DG010Delete.Visible = true;

                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    //ScriptManager.RegisterClientScriptBlock(WFB2DG010Save, this.GetType(), "init", "initForm();", true);
                    return;
                }
            }
            else
            {
                fb2dg.CAR_PARK_NO = ((TextBox)KeyinRow.FindControl("txt_CAR_PARK_NO_Add")).Text;
                fb2dg.PLANT_CD = ((DropDownList)KeyinRow.FindControl("ddl_PLANT_CD_Add")).Text;
                fb2dg.PARKING_NAME = ((TextBox)KeyinRow.FindControl("txt_PARKING_NAME_Add")).Text;
                fb2dg.PARKING_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_PARKING_TYPE_Add")).Text;
                fb2dg.PARKING_SPOT = ((TextBox)KeyinRow.FindControl("txt_PARKING_SPOT_Add")).Text;
                fb2dg.OVERLAP = ((TextBox)KeyinRow.FindControl("txt_OVERLAP_Add")).Text;
                fb2dg.NEEDSELECT = ((DropDownList)KeyinRow.FindControl("ddl_NEEDSELECT_Add")).Text; //是否必須選取
               
                msg = service.updateData(fb2dg);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2DG010Save, this.GetType(), "success", "history.back(-4);", true);
                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "EMP_ID" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false;

                    //enable查詢清除按鈕
                    WFB2DG010Search.Enabled = true;
                    WFB2DG010Clear.Enabled = true;

                    WFB2DG010Save.Visible = false;
                    WFB2DG010Cancel.Visible = false;
                    WFB2DG010Add.Visible = true;
                    WFB2DG010Edit.Visible = true;
                    WFB2DG010Delete.Visible = true;

                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    return;
                }
            }


            ////createSYS_ID();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DG010Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2DG010Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            //WFB2DG010Search.Enabled = true;
            //WFB2DG010Clear.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2DG010Edit.Visible = true;
                WFB2DG010Delete.Visible = true;
            }

            WFB2DG010Save.Visible = false;
            WFB2DG010Cancel.Visible = false;
            WFB2DG010Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DG010Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        //WFB2DG010Search.Enabled = true;
        //WFB2DG010Clear.Visible = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DG010Edit.Visible = true;
            WFB2DG010Delete.Visible = true;
        }

        WFB2DG010Save.Visible = false;
        WFB2DG010Cancel.Visible = false;
        WFB2DG010Add.Visible = true;
        WFB2DG010Search.Enabled = true;
        WFB2DG010Clear.Enabled = true;
    }

    protected void ddl_CAR_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
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




    protected void WFB2DG010Detail_Click(object sender, EventArgs e)
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
                string re = string.Format("WFB2DG010_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2DG010_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    protected string Syscode()
    {
        //權限:依登入者的權限設定，顯示符合該登入者的功能鍵																														
        //依照所取得的小分類，來顯示底下下拉選單- 廠別的內容																											
        string syscode = string.Empty;
        string derolecd = string.Empty;
        string dept = string.Empty;
        string departments = string.Empty;
        string SysCode = string.Empty;
        string st = string.Empty;
        ACESLib.ACES aces = new ACESLib.ACES();
        List<string> syscodelist = new List<string>();
        List<string> Codelist = new List<string>();
        string a = aces.GetRoles();
        foreach (string dbRoleCD in aces.GetRoles().Split(','))
        {
            derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
            ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
            //derolecd = "FB2DBOWNER";
            dept = deptbean.IsDEPT;
            departments = deptbean.Departments;
            SysCode = deptbean.SysCode;

            foreach (string code in SysCode.Split(','))
            {
                if (code.Trim().Equals("PLANT_CD"))
                {
                    string syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());

                    foreach (string  item in syscodeatt.Split(','))
                    {
                        switch (item.Trim())
                        {
                            case "CL":
                                st = "1";
                                break;
                            case "KN":
                                st = "2";
                                break;
                            default:
                                st = item.Trim();
                               break;
                        }
                        st = string.Format("'{0}'", st);
                        if (!Codelist.Contains(st))
                        {
                            Codelist.Add(st);
                        }
                    }


                    //Codelist = syscodeatt.Split(',').tol;

                    //syscodeatt = syscodeatt.Trim();
                    //if (st.IndexOf(syscodeatt, 0) == -1)
                    //{
                    //    st = st + syscodeatt + ",";
                    //}
                }
            }
            //st = st.Substring(0, st.Length - 1);

        }

        return string.Join(",", Codelist.ToArray());
    }
}


