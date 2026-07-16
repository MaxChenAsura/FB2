using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb299_WFB2990400_Dtl : BasePage
{
    string fun_name = "wfb2990400";
    string ID = string.Empty;
    string MODE_ID = string.Empty;
    string FUNC_ID = string.Empty;
    string emp_id = string.Empty;
    string TableName = string.Empty;
    string TextColumn = string.Empty;
    string ValueColumn = string.Empty;
    //Service 物件
    private CFB2990400BO service = new CFB2990400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //取得table 顯示欄位 值
        TableName = "tb";
        TextColumn = "FUNC_NAME";
        ValueColumn = "FUNC_ID";

        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
        {
            ID = Convert.ToString(Request.QueryString["id"]);
        }

        //emp_id = Request.QueryString["emp_id"].ToString();
 
        
        if (!Page.IsPostBack)
        {

            DataTable dt = new DataTable();
            dt = service.getFUNC_ID(ID);

            if (dt.Rows.Count > 0) {
                HID_FUNC_ID.Value = Convert.ToString(dt.Rows[0]["FUNC_ID"]);
                HID_MODE_ID.Value = Convert.ToString(dt.Rows[0]["MODE_ID"]);


                lit_FUNC_ID.Text = Convert.ToString(dt.Rows[0]["MODE_ID"]);
                lit_FUNC_NAME.Text = Convert.ToString(dt.Rows[0]["MODE_NAME"]);

            }


            //取得multi_select
            getData();
            
            //下方gridview
            getGridView("FUNC_ID", 0, 10);
            gv_result.ShowFooter = false;
        }

    }

    private void getData()
    {
        try
        {


            //將代碼繫結至listbox
            Multi_Select multi = new Multi_Select();
            multi.TableNmae = TableName;
            multi.TextColumn = TextColumn;
            multi.ValueColumn = ValueColumn;
           
            //DataTable dt = new DataTable();
            //dt = multi.getSelectData(fun_name, HID_FUNC_ID.Value);
            //lb_unselect.DataSource = dt;
            //lb_unselect.DataTextField = TextColumn;
            //lb_unselect.DataValueField = ValueColumn;
            //lb_unselect.DataBind();
            //DataTable dt1 = new DataTable();
            //dt1 = service.getModeData(ID);
            //lb_select.DataSource = dt1;
            //lb_select.DataTextField = "FUNC_NAME";
            //lb_select.DataValueField = "FUNCTION_ID";
            //lb_select.DataBind();

            DataTable dt1 = new DataTable();
            dt1 = service.getModeData(ID);
            lb_select.DataSource = dt1;
            lb_select.DataTextField = "FUNC_NAME";
            lb_select.DataValueField = "FUNCTION_ID";
            lb_select.DataBind();

            //所有已選擇的function id
            DataTable allDT = multi.getAllFunc();

            //string funcs = "";
            //for (int i = 0; i < allDT.Rows.Count; i++)
            //{                
            //    funcs = funcs + "'" + allDT.Rows[i]["FUNCTION_ID"].ToString() + "'" + ",";
            //}           
            //funcs = funcs.Substring(1, funcs.Length-3); 
            
            //未選擇的function id
            DataTable dt = new DataTable();
            dt = service.getFuncData(allDT);
            //int tt  = dt.Rows.Count;
            //dt = multi.getSelectData(fun_name, HID_FUNC_ID.Value);
            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = TextColumn;
            lb_unselect.DataValueField = ValueColumn;
            lb_unselect.DataBind();
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("FUNC_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "id" }; //設定GridView Key
            //gv_result.DataBind();

            gv_result.Visible = true;
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
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "id" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
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
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "id" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }

    //確認按鈕
    protected void WFB2990401Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2990400DAO wfb299 = new CFB2990400DAO();
            wfb299.MODE_ID = HID_MODE_ID.Value;
            wfb299.CREATED_BY = SessionHandle.Current.emp_id;
            wfb299.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb299.FUNC_ID = "FB2990400";




            if (lb_select.Items.Count == 0 && lb_unselect.Items.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('無選擇一筆資料來儲存');", true);
                return;
            }
            service.deleteFUNC(HID_MODE_ID.Value);

            string msg = "0";
            foreach (ListItem item in lb_select.Items)
            {
                wfb299.FUNCTION_ID = item.Value;
                wfb299.FUNCTION_NAME = item.Text.Replace(item.Value + ":", "");
                msg = service.add_SYS_D_Data(wfb299);
            }

            //wfb299.INS_TYPE = "A";
            //wfb299.EMP_ID = txt_EMP_ID.Text;
            //wfb299.IDENTITY_KIND = "1";
            //wfb299.LICENSE_ID = txt_LICENSE_ID.Text;
            //wfb299.COMPANY_CD = COMPANY_CD.Text;
            //wfb299.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
            //wfb299.INS_AMT = INS_AMT.Text.Replace(",", "");
            //wfb299.EFFECT_SDT = EFFECT_SDT.Text;
            //wfb299.EFFECT_EDT = EFFECT_EDT.Text;
            //wfb299.REMARK = REMARK.Text;

            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('儲存成功');", true);

            WFB2990401Save.Visible = true;
            WFB2990400Cancel.Visible = true;
            HID_isAdd.Value = "";
            //getLABOR_Data();


            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FUNC_ID", 0, 10);    
                //getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            
            
            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "id" };
            //gv_result.EditIndex = -1;
            //gv_result.ShowFooter = false; 

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕
    protected void WFB2990400Cancel_Click(object sender, EventArgs e)
    {
        HID_isAdd.Value = "";
        //getLABOR_Data();

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        Session["99040_Is_Search"] = "Y";
        string re = string.Format("WFB2990400_Qry.aspx");
        Response.Redirect(re);
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
}