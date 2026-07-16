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
public partial class WebContent_fb2dg_WFB2DG0300_ADD : BasePage
{
    string fun_name = "WFB2DG030";
    string ID = string.Empty;
    string MODE_ID = string.Empty;
    string FUNC_ID = string.Empty;
    string emp_id = string.Empty;
    string TableName = string.Empty;
    string TextColumn = string.Empty;
    string ValueColumn = string.Empty;
    //Service 物件
    private CFB2DG030BO service = new CFB2DG030BO();
    string event_target = string.Empty;
    string event_argu = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        TableName = "tb";
        TextColumn = "CLOCK";
        ValueColumn = "CLOCK_NO";
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
        {
            ID = Convert.ToString(Request.QueryString["id"]);
        }
        if (!IsPostBack)
        {

            //lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString(); 
            getPLANT_CD();
            getPARKING_CD();
            getCAR_PARK_NO();
            getCAR_BRAND();
            getCAR_TYPE();
            getData();            
            
        }

        event_target = Request.Form.Get("__EVENTTARGET");
        event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "getHistoryGrid")
        {
            // call function
            getHistoryGrid();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    private void getHistoryGrid()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            COMMGEOBO service = new COMMGEOBO();
            DataTable dt = service.getEMPFile(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_PLANT_CD.Text = string.Format("{0}-{1}", dt.Rows[0]["PLANT_CD"].ToString(), dt.Rows[0]["PLANT_NAME"].ToString());
                txt_DEPT_NO.Text = string.Format("{0}-{1}", dt.Rows[0]["DEPT_NO"].ToString(), dt.Rows[0]["DEPT_NAME"].ToString());
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_LINE_CD.Text = string.Format("{0}-{1}", dt.Rows[0]["LINE_CD"].ToString(), dt.Rows[0]["LINE_NAME"].ToString());
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getData()
    {
        try
        {
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.PARKING_CD = ddl_PARKING_CD.SelectedValue;
            //將代碼繫結至listbox


            Multi_Select multi = new Multi_Select();
            multi.TableNmae = TableName;
            multi.TextColumn = TextColumn;
            multi.ValueColumn = ValueColumn;
            DataTable dt = new DataTable();
            dt = service.getaddData(fb2dg);
            string a = Convert.ToString(dt.Rows.Count);
            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = "CLOCK_NAME";
            lb_unselect.DataValueField = ValueColumn;
            lb_unselect.DataBind();
            DataTable dt1 = new DataTable();
            dt1 = service.getModeData2(ID);
            lb_select.DataSource = dt1;
            lb_select.DataTextField = "CLOCK";
            //lb_select.DataValueField = "ITEM_SEQ";
            lb_select.DataBind();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCAR_TYPE()
    {
        try
        {
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.CAR_TYPE = ddl_CAR_BRAND.SelectedValue;
            DataTable dt = new DataTable();
            dt = service.getCAR_TYPE(fb2dg);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_TYPE.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()),dt.Rows[i]["SUB_CD"].ToString()));
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
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getPARKING_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getPARKING_CD();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PARKING_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getCAR_PARK_NO()
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dt = service.getCAR_PARK_NO(fb2dg);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_PARK_NO.Items.Add(new ListItem(string.Format(dt.Rows[i]["CAR_PARK_NO"].ToString() + "-" + dt.Rows[i]["PARKING_NAME"].ToString()), dt.Rows[i]["CAR_PARK_NO"].ToString()));
                }
            }


            fb2dg.CAR_PARK_NO = ddl_CAR_PARK_NO.SelectedValue;
            DataTable dt1 = new DataTable();
            dt1 = service.getREMAINDER_PARKING_SPOT(fb2dg);
            txt_REMAINDER_PARKING_SPOT.Text = Convert.ToString(dt1.Rows[0]["REMAINDER_PARKING_SPOT"].ToString());

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCAR_BRAND()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCAR_BRAND();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_BRAND.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()),dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    private DataTable get_SYS_ID_Data()
    {
        CFB2IB0100DAO fb2dg = new CFB2IB0100DAO();
        return fb2dg.get_SYS_ID_Data();
    }
    private void createSYS_ID()
    {
        try
        {
            DataTable dt = get_SYS_ID_Data();
            ddl_PLANT_CD.Items.Clear();
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


    protected void WFB2DG030Save_Click(object sender, EventArgs e)
    {
        try
        {
           
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
           
            CFB2DG030BO service = new CFB2DG030BO();
            string msg = "";
            Control KeyinRow = null;
            

            //fb2dg.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2dg.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            
                string Message = string.Empty;
                fb2dg.EMP_ID = txt_EMP_ID.Text;
                fb2dg.EMP_NAME = txt_EMP_NAME.Text;
                fb2dg.PARKING_PLANT_CD = ddl_PLANT_CD.SelectedValue;
                fb2dg.DEPT_NO = HidDEPT_NO.Value;
                fb2dg.CAR_NO = txt_CAR_NO.Text;
                fb2dg.CAR_BRAND = ddl_CAR_BRAND.SelectedValue;
                fb2dg.CAR_TYPE = ddl_CAR_TYPE.SelectedValue;
                fb2dg.CAR_PARK_NO = ddl_CAR_PARK_NO.SelectedValue;
                fb2dg.PARKING_CD = ddl_PARKING_CD.SelectedValue;




                fb2dg.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData_1(fb2dg);//TB_D_M_PARKING_EMP_MAIN
                string CAR_PARK = string.Empty;
                CAR_PARK = ddl_CAR_PARK_NO.SelectedValue;

                DataTable dt = new DataTable();
                //剩餘數
                DataTable dt_remainder = service.getREMAINDER_PARKING(fb2dg.CAR_PARK_NO);
                if (dt_remainder.Rows.Count > 0)
                {
                    fb2dg.REMAINDER_PARKING_SPOT = dt_remainder.Rows[0]["REMAINDER_PARKING_SPOT"].ToString();
                }
                
                dt = service.addData_2(CAR_PARK);
                if (dt.Rows.Count > 0)
                {
                    string CAR_PARK_NO = dt.Rows[0]["total_record"].ToString();
                    fb2dg.CAR_PARK_NO_N = CAR_PARK_NO;


                    msg = service.addData_3(fb2dg);
                }
                int X = 0;
                foreach (ListItem item in lb_select.Items)
                {
                    fb2dg.X = X;
                    fb2dg.CLOCK2 = item.Value;
                    fb2dg.EMP_ID = txt_EMP_ID.Text;


                    msg = service.CLOCK(fb2dg);
                    X = X + 1;
                }

                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    Session["DG030_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2DG030Save, this.GetType(), "success", "location.href='WFB2DG0300_Qry.aspx';", true);
                }
                else
                {
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2DG030Save, this.GetType(), "init", "initForm();", true);
                }
             
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DG030Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            
        }
    }
    protected void WFB2DG030Clear_Click(object sender, EventArgs e)
    {
        Session["DG030_Is_Search"] = "Y";
        Response.Redirect("WFB2DG0300_Qry.aspx");

    }
    protected void btn_select_Click(object sender, EventArgs e)
    {

        foreach (ListItem item in lb_unselect.Items)
        {
            if (item.Selected == true)
            {
                if (!lb_select.Items.Contains(item))
                {
                    lb_select.Items.Add(new ListItem(item.Text, item.Value));
                }

            }
        }

        foreach (ListItem item in lb_select.Items)
        {
            lb_unselect.Items.Remove(item);
        }

    }
    protected void btn_unselect_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in lb_select.Items)
        {
            if (item.Selected == true)
            {
                if (!lb_unselect.Items.Contains(item))
                {
                    lb_unselect.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
        }

        foreach (ListItem item in lb_unselect.Items)
        {
            lb_select.Items.Remove(item);
        }

    }
    protected void ddl_PLANT_CD_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        if (ddl_PLANT_CD.SelectedValue != "-1")
        {
            ViewState["Queryble"] = false;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dt = service.getCAR_PARK_NO(fb2dg);
            ddl_CAR_PARK_NO.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_PARK_NO.Items.Add(new ListItem(string.Format(dt.Rows[i]["CAR_PARK_NO"].ToString() + "-" + dt.Rows[i]["PARKING_NAME"].ToString()), dt.Rows[i]["CAR_PARK_NO"].ToString()));
                }
            }

        }
        else
        {
            getCAR_PARK_NO();
        }
    }
    protected void ddl_CAR_PARK_NO_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        if (ddl_CAR_PARK_NO.SelectedValue != "-1")
        {
            ViewState["Queryble"] = false;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.CAR_PARK_NO = ddl_CAR_PARK_NO.SelectedValue;
            dt = service.getREMAINDER_PARKING_SPOT(fb2dg);
            txt_REMAINDER_PARKING_SPOT.Text = Convert.ToString(dt.Rows[0]["REMAINDER_PARKING_SPOT"].ToString());
            HID_NEED_SELECT.Value = Convert.ToString(dt.Rows[0]["NEEDSELECT"].ToString());

        }
        else
        {
            getCAR_PARK_NO();
        }
    }
    protected void ddl_PARKING_CD_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        if (ddl_PARKING_CD.SelectedValue != "-1")
        {
            ViewState["Queryble"] = false;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.PARKING_CD = ddl_PARKING_CD.SelectedValue;


            Multi_Select multi = new Multi_Select();
            multi.TableNmae = TableName;
            multi.TextColumn = TextColumn;
            multi.ValueColumn = ValueColumn;

            dt = service.getModeData(fb2dg);
            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = "CLOCK";
            lb_unselect.DataValueField = "CLOCK_NO";
            lb_unselect.DataBind();
            DataTable dt1 = new DataTable();
            dt1 = service.getModeData2(ID);
            lb_select.DataSource = dt1;
            lb_select.DataTextField = "CLOCK";
            //lb_select.DataValueField = "ITEM_SEQ";
            lb_select.DataBind();

        }
        else
        {
            getCAR_PARK_NO();
        }
    }

    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
     
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        if (ViewState["SortExpression"] == null)
            getSortDirection("UPDATE_DT", "DESC");   //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "UPDATE_DT" };
        //gv_result.DataBind();
        if (gv_result.Rows.Count == 0)
        {
            //gv_result.Visible = false;           
        }
        HID_PageRow.Value = "";
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
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "UPDATE_DT" }; //設定GridView Key
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "UPDATE_DT" }; //設定GridView Key
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
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }  
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (lb_select.SelectedIndex > 0);
    }


    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            COMMGEOBO service = new COMMGEOBO();
            DataTable dt = service.getEMPFile(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_PLANT_CD.Text = string.Format("{0}-{1}",dt.Rows[0]["PLANT_CD"].ToString(),dt.Rows[0]["PLANT_NAME"].ToString());
                txt_DEPT_NO.Text = string.Format("{0}-{1}",dt.Rows[0]["DEPT_NO"].ToString(),dt.Rows[0]["DEPT_NAME"].ToString());
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_LINE_CD.Text = string.Format("{0}-{1}",dt.Rows[0]["LINE_CD"].ToString(),dt.Rows[0]["LINE_NAME"].ToString());
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2DG030Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ddl_BRAND_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        if (ddl_CAR_BRAND.SelectedValue != "-1")
        {
            ViewState["Queryble"] = false;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.CAR_TYPE = ddl_CAR_BRAND.SelectedValue;
            dt = service.getCAR_TYPE(fb2dg);
            ddl_CAR_TYPE.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_TYPE.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }


        }
        else
        {
            getCAR_TYPE();
        }
    }

}