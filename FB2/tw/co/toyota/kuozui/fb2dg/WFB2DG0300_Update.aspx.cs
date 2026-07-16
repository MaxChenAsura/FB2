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
public partial class WebContent_fb2dg_WFB2DG0300_Update : BasePage
{
    //Service 物件
    private CFB2DG030BO service = new CFB2DG030BO();
    string emp_id = string.Empty;
    string fun_name = "WFB2DG030";
    string ID = string.Empty;
    string MODE_ID = string.Empty;
    string FUNC_ID = string.Empty;
    
    string TableName = string.Empty;
    string TextColumn = string.Empty;
    string ValueColumn = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數

        
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
        {
            ID = Convert.ToString(Request.QueryString["id"]);
            emp_id = Convert.ToString(Request.QueryString["id"]);
        }
        if (!IsPostBack)
        {
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            txt_EMP_ID.Text = emp_id;
            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("UPDATE_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("UPDATE_DT", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            TableName = "tb";
            TextColumn = "CLOCK";
            ValueColumn = "CLOCK_NO";

            //lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString(); 
            getPLANT_CD();
            getPARKING_CD();
            getCAR_PARK_NO();
            getCAR_BRAND();
            //getCAR_TYPE();
            getData();
            checkNEED_SELECT();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void checkNEED_SELECT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.checkNEED_SELECT(ID);
            if (dt.Rows.Count > 0)
            {
                HID_NEED_SELECT.Value = dt.Rows[0]["NEEDSELECT"].ToString();
            }

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
            string cBrand = "", cType = "";
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            
            //找到該員工car brand
            DataTable cb = service.getEMP_CAR_BRAND(emp_id);
            if (cb.Rows.Count > 0)
	        {
                cBrand = cb.Rows[0]["CAR_BRAND"].ToString();
                cType = cb.Rows[0]["CAR_TYPE"].ToString();
	        }
            fb2dg.CAR_TYPE = cBrand;
            DataTable dt = new DataTable();
            dt = service.getCAR_TYPE(fb2dg);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_TYPE.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                    if (cBrand == "1")
                    {
                        if (dt.Rows[i]["SUB_CD"].ToString() == cType)
                        {
                            ddl_CAR_TYPE.Items[i].Selected = true;
                        }
                    }
                    if (cBrand == "2")
                    {
                        ddl_CAR_TYPE.Items[i].Selected = true;
                    }
                }
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
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
           
            //DataTable dt = new DataTable();
            //基本資料
            dt = service.getData(emp_id);

            if (dt.Rows.Count > 0)
            {
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                HidPJOB_CD.Value = dt.Rows[0]["PJOB_CD"].ToString();
                ddl_PLANT_CD.SelectedValue = dt.Rows[0]["PARKING_PLANT_CD"].ToString();
                HidPLANT_CD.Value = dt.Rows[0]["PARKING_PLANT_CD"].ToString();
                txt_LINE_CD.Text = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                Hid_work_shift_cd.Value = dt.Rows[0]["wsc"].ToString();
                HidIFLOW_NO.Value = dt.Rows[0]["IFLOW_NO"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                HidDEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_CAR_NO.Text = dt.Rows[0]["CAR_NO"].ToString();
                HidCAR_NO.Value = dt.Rows[0]["CAR_NO"].ToString();
                ddl_PARKING_CD.SelectedValue = dt.Rows[0]["PARKING_TOOL"].ToString();
                HidPARKING_CD.Value = dt.Rows[0]["PARKING_TOOL"].ToString();
                ddl_CAR_BRAND.SelectedValue = dt.Rows[0]["CAR_BRAND"].ToString() == "" ? "2" : dt.Rows[0]["CAR_BRAND"].ToString();
                HidCAR_BRAND.Value = dt.Rows[0]["CAR_BRAND"].ToString();
                ddl_CAR_PARK_NO.SelectedValue = dt.Rows[0]["CAR_PARK_NO"].ToString();
                HidCAR_PARK_NO.Value = dt.Rows[0]["CAR_PARK_NO"].ToString();
                CAR_BRAND_Changed(ddl_CAR_BRAND.SelectedValue);
                ddl_CAR_TYPE.SelectedValue = dt.Rows[0]["CAR_TYPE"].ToString() == "" ? "4" : dt.Rows[0]["CAR_TYPE"].ToString();
                HidCAR_TYPE.Value = dt.Rows[0]["CAR_TYPE"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_CD"].ToString();
                DataTable dt2 = new DataTable();
                fb2dg.CAR_PARK_NO = ddl_CAR_PARK_NO.SelectedValue;
                dt2 = service.getREMAINDER_PARKING_SPOT(fb2dg);
                txt_REMAINDER_PARKING_SPOT.Text = Convert.ToString(dt2.Rows[0]["REMAINDER_PARKING_SPOT"].ToString());

                fb2dg.PARKING_CD = ddl_PARKING_CD.SelectedValue;
                fb2dg.EMP_ID = ID;
                Multi_Select multi = new Multi_Select();
                multi.TableNmae = TableName;
                multi.TextColumn = TextColumn;
                multi.ValueColumn = ValueColumn;

                dt = service.getModeData(fb2dg);
                lb_unselect.DataSource = dt;
                lb_unselect.DataTextField = TextColumn;
                lb_unselect.DataValueField = ValueColumn;
                lb_unselect.DataBind();

                dt1 = service.getModeData2(ID);
                lb_select.DataSource = dt1;
                lb_select.DataTextField = "CLOCK";
                //lb_select.DataValueField = "ITEM_SEQ";
                lb_select.DataBind();

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
                    ddl_PLANT_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()),dt.Rows[i]["SUB_CD"].ToString()));
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
                    ddl_PARKING_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()),dt.Rows[i]["SUB_CD"].ToString()));
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
            string CAR_PARK_NO = ""; 
            string PARKING_PLANT_CD = "";
            DataTable dt = new DataTable();
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            DataTable dt1 = service.getCarParkNo(emp_id);
            if (dt1.Rows.Count > 0)
            {
                CAR_PARK_NO = dt1.Rows[0]["CAR_PARK_NO"].ToString();//default value
                PARKING_PLANT_CD = dt1.Rows[0]["PARKING_PLANT_CD"].ToString();
            }

            fb2dg.PLANT_CD = PARKING_PLANT_CD;
            dt = service.getCAR_PARK_NO(fb2dg);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_PARK_NO.Items.Add(new ListItem(string.Format(dt.Rows[i]["CAR_PARK_NO"].ToString() + "-" + dt.Rows[i]["PARKING_NAME"].ToString()),dt.Rows[i]["CAR_PARK_NO"].ToString()));
                }
            }
          
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
            CFB2DG030DAO fb2dg2 = new CFB2DG030DAO();
            CFB2DG030BO service = new CFB2DG030BO();
            string msg = "";
            Control KeyinRow = null;
            ViewState["Queryble"] = false;

            //fb2dg.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2dg.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增

            //更新歷史檔案資料
                fb2dg2.EMP_ID = txt_EMP_ID.Text;
                fb2dg2.EMP_NAME = txt_EMP_NAME.Text;
                fb2dg2.PARKING_PLANT_CD = HidPLANT_CD.Value;
                fb2dg2.DEPT_NO = txt_DEPT_NO.Text;
                fb2dg2.DEPT_NAME = HidDEPT_NAME.Value;
                fb2dg2.LEVEL_CD = txt_LEVEL_CD.Text;
                fb2dg2.PJOB_CD = HidPJOB_CD.Value;
                fb2dg2.PJOB_NAME = txt_PJOB_DESC.Text;
                //fb2dg2.WORK_SHIFT = txt_LINE_CD.Text;
                fb2dg2.WORK_SHIFT = Hid_work_shift_cd.Value;
                fb2dg2.CAR_NO = HidCAR_NO.Value;
                fb2dg2.CAR_BRAND = HidCAR_BRAND.Value;
                fb2dg2.CAR_TYPE = HidCAR_TYPE.Value;
                fb2dg2.PARKING_TOOL = HidPARKING_CD.Value;
                fb2dg2.CAR_PARK_NO = HidCAR_PARK_NO.Value;
                fb2dg2.IFLOW_NO = HidIFLOW_NO.Value;
                fb2dg2.CREATED_BY = SessionHandle.Current.emp_id;
                fb2dg2.UPDATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData_2_1(fb2dg2);
             //更新歷史檔案資料


                string Message = string.Empty;
                fb2dg.EMP_ID = txt_EMP_ID.Text;
                fb2dg.EMP_NAME = txt_EMP_NAME.Text;
                fb2dg.PARKING_PLANT_CD = ddl_PLANT_CD.SelectedValue;
                fb2dg.DEPT_NO = txt_DEPT_NO.Text;
                fb2dg.CAR_NO = txt_CAR_NO.Text;
                fb2dg.CAR_BRAND = ddl_CAR_BRAND.SelectedValue;
                fb2dg.CAR_TYPE = ddl_CAR_TYPE.SelectedValue;
                fb2dg.CAR_PARK_NO = ddl_CAR_PARK_NO.SelectedValue;
                fb2dg.PARKING_CD = ddl_PARKING_CD.SelectedValue;
                fb2dg.CREATED_BY = SessionHandle.Current.emp_id;
                DataTable dt = new DataTable();

                msg = service.updateData(fb2dg);

                string CAR_PARK = string.Empty;
                CAR_PARK = ddl_CAR_PARK_NO.SelectedValue;


                dt = service.addData_2(CAR_PARK);
                if (dt.Rows.Count > 0)
                {
                    string CAR_PARK_NO = dt.Rows[0]["total_record"].ToString();
                    fb2dg.CAR_PARK_NO_N = CAR_PARK_NO;
                    fb2dg.REMAINDER_PARKING_SPOT = service.getREMAINDER_PARKING_SPOT_2(fb2dg);
                    msg = service.addData_3(fb2dg);                   
                }
                int X = 0;
                if (lb_select.Items.Count > 0)
                {
                    foreach (ListItem item in lb_select.Items)
                    {
                        fb2dg.X = X;
                        fb2dg.CLOCK2 = item.Value;
                        fb2dg.EMP_ID = txt_EMP_ID.Text;


                        msg = service.CLOCK(fb2dg);
                        X = X + 1;
                    }
                }
                else
                {
                    //P1  P3機車停車場 不需選卡鐘
                    msg = service.delCLOCK(fb2dg);
                }
                



                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2DG030Save, this.GetType(), "success", "history.back(-4);", true);
                    Session["DG030_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2DG030Save, this.GetType(), "success", "location.href='WFB2DG0300_Qry.aspx';", true);
                }
                else
                {
                    showMessage("modFailMessage", msg);
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
            fb2dg.EMP_ID = ID;

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

                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                    ((DropDownList)e.Row.FindControl("ddl_IS_VALID_Add")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);
                else
                {
                    Label lbl_USER_UPD = ((Label)e.Row.FindControl("lbl_IS_VALID"));

                    if (Convert.ToString(DataRow["IS_VALID"]) == "Y")
                        lbl_USER_UPD.Text = "Y";
                    else if (Convert.ToString(DataRow["IS_VALID"]) == "N")
                        lbl_USER_UPD.Text = "N";
                    else
                        lbl_USER_UPD.Text = "";
                }

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

    protected void CAR_BRAND_Changed(string CAR_BRAND)
    {

        DataTable dt = new DataTable();
        if (CAR_BRAND != "-1")
        {
            ViewState["Queryble"] = false;
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            fb2dg.CAR_TYPE = CAR_BRAND;
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