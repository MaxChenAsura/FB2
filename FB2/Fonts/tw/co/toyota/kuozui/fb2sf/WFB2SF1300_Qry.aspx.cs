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
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;
public partial class WebContent_fb2sf_WFB2SF1300_Qry : BasePage
{
    //Service 物件
    private CFB2SF1300BO service = new CFB2SF1300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            getSALARY_TYPE();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getSALARY_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSALARY_TYPE();
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
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
            ddl_SALARY_TYPE.Items.Clear();
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_SYS_ID_Data()
    {
        CFB2SF1300DAO fb2ib = new CFB2SF1300DAO();
        return fb2ib.get_SYS_ID_Data();
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        if (ViewState["SortExpression"] == null)
            getSortDirection("EMP_ID");   //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.DataBind();
        if (gv_result.Rows.Count == 0)
        {
            //gv_result.Visible = false;           
        }
        HID_PageRow.Value = "";
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
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getSALARY_TYPE();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
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

            //控制GridView裡面的值
            //string err = service.getLogFlag(dao);
            //CheckBox ck1 = (CheckBox)e.Row.FindControl("cb_check");
            Label lbl = (Label)e.Row.FindControl("lbl_ACCT_ID");
            Label lb2 = (Label)e.Row.FindControl("lbl_DEPT_ACCT_ID");
            Label lb3 = (Label)e.Row.FindControl("lbl_PAYMONEY_TYPE");
            Label lb4 = (Label)e.Row.FindControl("lb_Lno");
            Label lb_SALARY_DT = (Label)e.Row.FindControl("lb_SALARY_DT");
            

            TextBox tx1 = (TextBox)e.Row.FindControl("txt_HOPE_PAT_DT");
            TextBox tx2 = (TextBox)e.Row.FindControl("txt_S_DT");
            TextBox tx3 = (TextBox)e.Row.FindControl("txt_E_DT");
            RadioButtonList rdo1 = (RadioButtonList)e.Row.FindControl("rdo_PAYMONEY_TYPE");
            HiddenField hid_PAY_TARGET = (HiddenField)e.Row.FindControl("hid_PAY_TARGET");

            //20200923 依薪資類型給予預設值
            HiddenField hid_SALARY_TYPE = (HiddenField)e.Row.FindControl("hid_SALARY_TYPE");
            if (hid_SALARY_TYPE.Value == "A")
            {
                //薪資日期前1月月初及月底
                tx2.Text = Convert.ToDateTime(lb_SALARY_DT.Text).AddMonths(-1).ToString("yyyy/MM/01");
                tx3.Text = Convert.ToDateTime(Convert.ToDateTime(lb_SALARY_DT.Text).ToString("yyyy/MM/01"))
                    .AddDays(-1).ToString("yyyy/MM/dd");
                    ;
            }
            else {
                tx2.Text = lb_SALARY_DT.Text;
                tx3.Text = lb_SALARY_DT.Text;
            }
            /*
            //20200923 
            if ((lb3.Text).Substring(0, 1) == "A")
            {   
                //A.月薪  發薪日期前1月的起迄                
                tx2.Text = "9999/12/31";
                tx3.Text = "";
            }
            else { 
                //其它    發薪日期
                tx2.Text = lb_SALARY_DT.Text;
                tx3.Text = lb_SALARY_DT.Text;
            }*/


            //匯款方式
            if (lbl.Text == "")
            {
                rdo1.Visible = true;
                lb3.Visible = false;
                DataTable dt = new DataTable();
                dt = service.getPAYMONEY_TYPE();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rdo1.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                    }

                    if (lb3.Text != "")
                    {
                        if ((lb3.Text).Substring(0,1) == "A")
                        {
                            rdo1.Items[0].Selected = true;                          
                        }
                        else if ((lb3.Text).Substring(0, 1) == "B")
                        {
                            rdo1.Items[1].Selected = true;
                        }
                        else if ((lb3.Text).Substring(0, 1) == "C")
                        {
                            rdo1.Items[2].Selected = true;
                        }
                    }
                    else
                    {
                        //匯款方式的預設值 A->支票1, B->匯款0,C->匯款0,D->匯款0,E->個人2
                        if (hid_PAY_TARGET.Value == "A")
                        {
                            rdo1.Items[1].Selected = true;
                        }
                        else if (hid_PAY_TARGET.Value == "B" || hid_PAY_TARGET.Value == "C" || hid_PAY_TARGET.Value == "D")
                        {
                            rdo1.Items[0].Selected = true;
                        }
                        else if (hid_PAY_TARGET.Value == "E")
                        {
                            rdo1.Items[2].Selected = true;
                        }
                        else
                        {
                            rdo1.Items[0].Selected = true;
                        }
                    }                    
                   
                }
            }
            else
            {
                rdo1.Visible = false;
                lb3.Visible = true;
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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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

            gv_result.ShowFooter = false;
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
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (ddl_ACCT_ID.SelectedValue == "Y")
            {
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    ((TextBox)gv_result.Rows[i].FindControl("txt_HOPE_PAT_DT")).ReadOnly = true;
                    ((TextBox)gv_result.Rows[i].FindControl("txt_HOPE_PAT_DT")).CssClass = "number2";
                    ((TextBox)gv_result.Rows[i].FindControl("txt_HOPE_PAT_DT")).BorderWidth = 0;

                    ((TextBox)gv_result.Rows[i].FindControl("txt_E_DT")).ReadOnly = true;
                    ((TextBox)gv_result.Rows[i].FindControl("txt_E_DT")).CssClass = "number2";
                    ((TextBox)gv_result.Rows[i].FindControl("txt_E_DT")).BorderWidth = 0;
                    ((TextBox)gv_result.Rows[i].FindControl("txt_S_DT")).ReadOnly = true;
                    ((TextBox)gv_result.Rows[i].FindControl("txt_S_DT")).CssClass = "number2";
                    ((TextBox)gv_result.Rows[i].FindControl("txt_S_DT")).BorderWidth = 0;

                }

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
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void getData()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                //this.Page.FindControl("ddlPerPageRow");
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10000);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SF130Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SF130Add_Click(object sender, EventArgs e)
    {
        try
        {

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;

            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_ID", 0, 10000);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
                    ddl2.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

        }
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


    protected void WFB2SF130Detail_Click(object sender, EventArgs e)
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
                string re = string.Format("WFB2SF130_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }
    protected void WFB2SF130Search_Click(object sender, EventArgs e)
    {

        ViewState["Queryble"] = true;
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
            getGridView("EMP_ID", 0, 10000);
        }
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;

        if (gv_result.Rows.Count > 0)
        {
            WFB2SF1300Execute.Visible = true;
            WFB2SF1300Print.Visible = true;

        }
        else
        {
            WFB2SF1300Execute.Visible = false;
            WFB2SF1300Print.Visible = false;


        }
        if (gv_result.Rows.Count == 0)
        {
            showMessage("QryNotFoundMessage");
        }
    }
    protected void WFB2SF1300Delete_Click(object sender, EventArgs e)
    {
      
        ViewState["Queryble"] = false;
        string st = "";

        //if (txt_DEPT_ACCT_ID.Text == "")
        if (txt_ACCT_ID.Text == "" && txt_Lno.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行刪除傳票時,傳票號碼與批號不允同時空白.');", true);
            return;

        }
        else
        {
            string msg = "";
            CFB2SF1300DAO dao = new CFB2SF1300DAO();
            DataTable dt = new DataTable();
            dao.TblId = "H15060FFDA1";
            dao.ACCT_ID = txt_ACCT_ID.Text.Trim();
            dao.Lno = txt_Lno.Text.Trim();
            if (dao.Lno == "")
            {
                dao.TMP_LNO = service.getLno(dao);
                if (dao.TMP_LNO =="")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('傳票號碼錯誤');", true);
                    return;
                }
            }
            else
            {
                dao.TMP_LNO = dao.Lno;
            }
            //檢核:該傳票或批號是否能刪除
            msg = service.getLogFlag(dao.TMP_LNO, dao.TblId);
            if (msg == "N")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('傳票已進入財務系統，不能再重新計算.');", true);
                return;
            }                     
            
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SF130";
            
            msg = service.execDel(dao);
            if (msg == "0")
            {
                showMessage("deleteAS400Message");
            }
            else
            {

                showMessage("deleteAS400FailMessage", msg);
                //ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('查無資料');", true);
            }
        }


    }

    //法扣轉傳票
    protected void WFB2SF1300Execute_Click(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        CFB2SF1300DAO dao = new CFB2SF1300DAO();
        string msg = string.Empty;
        string DOC_NO = string.Empty;
        string EMP_ID = string.Empty;
        string EMP_NAME = string.Empty;
        string AMOUNT = string.Empty;
        string SALARY_NAME = string.Empty;
        string VENDOR_ID = string.Empty;
        string PAY_TARGET = string.Empty;
        string HOPE_PAT_DT = string.Empty;
        string S_DT = string.Empty;
        string E_DT = string.Empty;
        string PAYMONEY_TYPE = string.Empty;
        string PAYMONEY_NAME = string.Empty;

        string DEPT_ACCT_ID = string.Empty;
        string ACCT_ID = string.Empty;
        string SEQ = string.Empty;
        string SALARY_DT = string.Empty;
        string SALARY_TYPE = string.Empty;
        string PAY_KIND = string.Empty;
        string chkmsg = string.Empty;
        string isTransfer = "";
        isTransfer = ddl_ACCT_ID.SelectedValue;
        if (isTransfer == "Y")
        {
            chkmsg = chkmsg + "已轉過部門傳票,不允再執行轉傳票!\\n";
            ScriptManager.RegisterClientScriptBlock(WFB2SF1300Execute, this.GetType(), "alert", "alert('" + chkmsg + "');", true);
            return;
        }

        List<string> sys_id = new List<string>();
        int row = 0;

        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                HOPE_PAT_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_HOPE_PAT_DT")).Text;
                S_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_S_DT")).Text;
                E_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_E_DT")).Text;
                VENDOR_ID = ((Label)gv_result.Rows[i].FindControl("lbl_VENDOR_ID")).Text;
                row = i + 1;

                if (string.IsNullOrEmpty(HOPE_PAT_DT))
                {
                    chkmsg = chkmsg + "第" + row + "列希望匯款日不可為空!\\n";
                }
                else
                {
                    if (!DateValid(HOPE_PAT_DT))
                    {
                        chkmsg = chkmsg + "第" + row + "列希望匯款日日期格式錯誤!\\n";
                    }
                }

                if (string.IsNullOrEmpty(S_DT))
                {
                    chkmsg = chkmsg + "第" + row + "列發生期間起不可為空!\\n";
                }
                else
                {
                    if (!DateValid(S_DT))
                    {
                        chkmsg = chkmsg + "第" + row + "列發生期間起日期格式錯誤!\\n";
                    }
                }

                if (string.IsNullOrEmpty(E_DT))
                {
                    chkmsg = chkmsg + "第" + row + "列發生期間迄不可為空!\\n";
                }
                else
                {
                    if (!DateValid(E_DT))
                    {
                        chkmsg = chkmsg + "第" + row + "列發生期間迄日期格式錯誤!\\n";
                        //chkk = chkk + 1;
                    }
                }
                if (string.IsNullOrEmpty(VENDOR_ID))
                {
                    chkmsg = chkmsg + "第" + row + "列廠商code不可為空!\\n";
                    //chke = chke + 1; 
                }
            }
        }

        if (!string.IsNullOrEmpty(chkmsg))
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SF1300Execute, this.GetType(), "alert", "alert('" + chkmsg + "');", true);
            return;
        }


        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        dt.Columns.Add(new DataColumn("DOC_NO", typeof(string)));
        dt.Columns.Add(new DataColumn("EMP_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("EMP_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
        dt.Columns.Add(new DataColumn("SALARY_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("VENDOR_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("HOPE_PAT_DT", typeof(string)));
        dt.Columns.Add(new DataColumn("S_DT", typeof(string)));
        dt.Columns.Add(new DataColumn("E_DT", typeof(string)));
        dt.Columns.Add(new DataColumn("PAYMONEY_TYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("PAYMONEY_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("DEPT_ACCT_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("ACCT_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("SEQ", typeof(string)));
        dt.Columns.Add(new DataColumn("SALARY_DT", typeof(string)));
        dt.Columns.Add(new DataColumn("SALARY_TYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("PAY_KIND", typeof(string)));
        dt.Columns.Add(new DataColumn("PAY_TARGET", typeof(string))); //BY EVA ADD 2015/06/23

        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                DOC_NO = ((Label)gv_result.Rows[i].FindControl("lbl_DOC_NO")).Text;
                EMP_ID = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text;
                EMP_NAME = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_NAME")).Text;
                AMOUNT =(((Label)gv_result.Rows[i].FindControl("lbl_AMOUNT")).Text).Replace(",","");
                SALARY_NAME = ((Label)gv_result.Rows[i].FindControl("lbl_SALARY_NAME")).Text;
                //VENDOR_ID = txt_VENDOR_ID.Text;
                VENDOR_ID = ((Label)gv_result.Rows[i].FindControl("lbl_VENDOR_ID")).Text;
                HOPE_PAT_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_HOPE_PAT_DT")).Text;
                S_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_S_DT")).Text;
                E_DT = ((TextBox)gv_result.Rows[i].FindControl("txt_E_DT")).Text;
                ACCT_ID = ((Label)gv_result.Rows[i].FindControl("lbl_ACCT_ID")).Text;
                DEPT_ACCT_ID = ((Label)gv_result.Rows[i].FindControl("lbl_DEPT_ACCT_ID")).Text;
                PAY_TARGET = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_TARGET")).Value;
                             
                if (!string.IsNullOrEmpty(DEPT_ACCT_ID))
                {
                    PAYMONEY_TYPE = ((Label)gv_result.Rows[i].FindControl("lbl_PAYMONEY_TYPE")).Text;
                }
                else
                {
                    PAYMONEY_TYPE = ((RadioButtonList)gv_result.Rows[i].FindControl("rdo_PAYMONEY_TYPE")).SelectedValue;
                    PAYMONEY_NAME = ((RadioButtonList)gv_result.Rows[i].FindControl("rdo_PAYMONEY_TYPE")).SelectedItem.Text;
                }
                SEQ = ((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ")).Value;
                SALARY_DT = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_DT")).Value;
                SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;

                dt.Rows.Add(DOC_NO, EMP_ID, EMP_NAME, AMOUNT, SALARY_NAME, VENDOR_ID, HOPE_PAT_DT, S_DT, E_DT, PAYMONEY_TYPE, PAYMONEY_NAME, DEPT_ACCT_ID, ACCT_ID, SEQ, SALARY_DT, SALARY_TYPE, PAY_KIND, PAY_TARGET);
            }
        }

        string EMP_ID_LIST = string.Join(",", sys_id.ToArray());

        //DataTable欄位內容排序

        dt.DefaultView.Sort = "PAYMONEY_TYPE asc";
        string PAYMONEY_TYPE_tmp = string.Empty;
        int b = 0;
        foreach (DataRow r in dt.Rows)
        {
            if (!string.IsNullOrEmpty(r["DEPT_ACCT_ID"].ToString()))
            {
                showMessage("modFailMessage", "已轉過部門傳票,不允再執行轉傳票");
                ScriptManager.RegisterClientScriptBlock(WFB2SF1300Delete, this.GetType(), "init", "initForm();", true);
                return;
            }
        }
      
        string tmpNO = string.Empty;
        DataTable dttmpNO = new DataTable();

        //20161005 入帳日期
        dao.IaDat = txt_IaDat.Text;

        msg = service.transferToACC(dao, dt);


        if (msg == "0")
        {
            showMessage("SF110ExecuteSuccessMessage");
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            showMessage("SF110ExecuteFailMessage", msg);
            return;
        }

        ViewState["NewPageIndex"] = gv_result.PageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;

    }

    //列印明細
    protected void WFB2SF1300Print_Click(object sender, EventArgs e)
    {

        ViewState["Queryble"] = false;
        if (txt_ACCT_ID.Text == "" && txt_Lno.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行支付明細(PDF)時,傳票號碼與批號不允同時空白.');", true);
            return;
        }
       
        else
        {

            DataTable dt = new DataTable();

            dt = service.get_PDF_Data(txt_ACCT_ID.Text, txt_Lno.Text);
            //Int32 STD;
            //STD = Convert.ToInt32(txt_SALARY_DT.Text.Replace("-", "")) - 191100;
            // 建立報表參數陣列變數
            ReportParameter[] para = new ReportParameter[1];
            para[0] = new ReportParameter("datatime", DateTime.Now.ToString("yyyy/MM/dd"), true);
            //para[1] = new ReportParameter("SALARY_DT_Y", STD.ToString().Substring(0, 3), true);
            //para[2] = new ReportParameter("SALARY_DT_M", STD.ToString().Substring(3), true);
            //para[2] = new ReportParameter("DEF_SYM", txt_DEF_SYM.Text.Replace("/", ""), true);
            //para[3] = new ReportParameter("DEF_EYM", txt_DEF_EYM.Text.Replace("/", ""), true);
            //para[4] = new ReportParameter("HEALTH_ORG_ID", txt_HEALTH_ORG_ID.Text, true);
            //para[5] = new ReportParameter("CLASSQTY", txt_CLASSQTY.Text, true);

            ReportViewer reportviewer1 = new ReportViewer();
            //將ReportViewer1的DataSources集合清除
            reportviewer1.LocalReport.DataSources.Clear();
            //將ReportViewer1重置為初始狀態           
            reportviewer1.Reset();
            reportviewer1.LocalReport.Refresh();
            // 給 ReportViewer1 新的設定
            reportviewer1.LocalReport.ReportPath = "report/WFB2SF130PDF.rdlc";
            // 設定 ReportViewer1 的參數, 把值傳過去
            reportviewer1.LocalReport.SetParameters(para);
            // 設定 ReportViewer1 的 DataSources
            reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dssf130", dt));

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            byte[] bytes = reportviewer1.LocalReport.Render(
                                      "PDF", null, out mimeType, out encoding, out filenameExtension,
                                      out streamids, out warnings);

            //將Byte內容寫到Client
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={1}.{0}", filenameExtension, HttpUtility.UrlEncode("FB2SF130_1", System.Text.Encoding.UTF8)));
            //Response.BinaryWrite(bytes);
            Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();

        }
    }

    public bool DateValid(string ymd)
    {
        bool b = false;
        ymd = ymd.Trim();
        string Year = "", day = "", Month = "";
        try
        {
            if (ymd.Length != 10)
            {
                return b;
            }
            Year = ymd.Substring(0, 4);
            Month = ymd.Substring(5, 2);
            day = ymd.Substring(8, 2);

            if (ymd == "")
            {
                return b;
            }

            if (!IsNumeric(Year))
            {
                return b;
            }
            else if (Convert.ToInt32(Year) < 1910)
            {
                return b;
            }
            else if (Convert.ToInt32(Month) > 12 || Convert.ToInt32(Month) < 1)
            {
                return b;
            }
            else if ((Convert.ToInt32(Month) == 1 || Convert.ToInt32(Month) == 3 || Convert.ToInt32(Month) == 5 || Convert.ToInt32(Month) == 7 ||
                     Convert.ToInt32(Month) == 8 || Convert.ToInt32(Month) == 10 || Convert.ToInt32(Month) == 12) && (Convert.ToInt32(day) > 31 ||
                     Convert.ToInt32(day) < 1))
            {
                return b;
            }
            else if ((Convert.ToInt32(Month) == 4 || Convert.ToInt32(Month) == 6 || Convert.ToInt32(Month) == 9 || Convert.ToInt32(Month) == 11)
                && (Convert.ToInt32(day) > 30 || Convert.ToInt32(day) < 1))
            {
                return b;
            }
            else if (Convert.ToInt32(Month) == 2)
            {

                if (Convert.ToInt32(day) < 1)
                {
                    return b;
                }
                if (LeapYear(Convert.ToInt32(Year)) == true)
                {
                    if (Convert.ToInt32(day) > 29)
                    {
                        return b;
                    }
                }
                else
                {
                    if (Convert.ToInt32(day) > 28)
                    {
                        return b;
                    }
                }
            }
            b = true;
            return b;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return false;
        }
    }

    public bool LeapYear(int year)
    {
        try
        {
            if (year % 100 == 0)
            {
                if (year % 400 == 0) { return true; }
            }
            else if ((year % 4) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return false;
        }
    }

    public bool IsNumeric(String strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
    }

}


