using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0600_Add_batch : BasePage
{
    //Service 物件
    CFB2DC0600BO service = new CFB2DC0600BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生刷卡時間
            createAbnormalTime();
            //取得工廠區分
            getPLANT_CD();
            //異常刷卡原因
            getABNORMAL_REASON_CD();
            //異常刷卡類型
            getABNORMAL_TYPE();
            //交通車路線
            getLINE_CD();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    private void createAbnormalTime()
    {
        ddl_MINUTE.Items.Clear();
        ddl_MINUTE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
        string content = "";
        for (int i = 0; i < 60; i++)
        {
            content = i < 10 ? "0" + i : i.ToString();
            ddl_MINUTE.Items.Add(new ListItem(content, content));
        }
    }

    private void getLINE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DD", "LINE_CD", "", "");
            ddl_LINE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LINE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getABNORMAL_REASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_REASON_CD", "", "");
            ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getABNORMAL_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_TYPE", "", "");
            ddl_ABNORMAL_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("PLANT_CD,DEPT_NO,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {



        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lb = (Label)e.Row.FindControl("lb_CARD_STATUS");
            Label end_dt = (Label)e.Row.FindControl("lb_END_DT");
            if (lb != null && end_dt != null)
            {
                if (end_dt.Text != "")
                {
                    if (DateTime.Parse(end_dt.Text) < DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        lb.Text = "註銷";
                    }
                }
            }
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
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
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }
    protected void WFB2DC0600Search_Click(object sender, EventArgs e)
    {
        try
        {
            //已查詢過
            hid_SearchFlag.Value = "Y";            

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10000);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0600Delete.Visible = true;
                if (Convert.ToInt32(ViewState["TotalCount"]) > 150)
                {
                    hid_SearchFlag.Value = "N";
                    //GridView基本設定
                    gv_result.PageIndex = 0;
                    gv_result.DataSourceID = null;
                    gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
                    gv_result.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢人員不可大於150筆！');", true);
                }
            }
            else
            {
                hid_SearchFlag.Value = "N";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無資料！');", true);
            }
           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0600Add_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2DC0600Search.Enabled = false;
            btn_clear.Disabled = true;
            WFB2DC0600Add2.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10000);

            if (Convert.ToInt32(ViewState["TotalCount"]) > 150)
            {
                WFB2DC0600Search.Enabled = true;
                btn_clear.Disabled = false;
                WFB2DC0600Add2.Visible = true;
                gv_result.PageIndex = 0;
                gv_result.DataSourceID = null;
                gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
                gv_result.DataBind();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢人員不可大於150筆！');", true);
                return;
            }
            WFB2DC0600Save.Visible = true;
            WFB2DC0600Cancel.Visible = true;

            WFB2DC0600Add.Visible = false;
            WFB2DC0600Delete.Visible = false;            
            gv_result.Visible = true;
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2DC0600Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());

                }
            }
            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                List<string> arrAddEmp = hid_AddEMP.Value.Split(',').ToList();
                hid_AddEMP.Value = "";
                foreach (var item in emp_data)
                {
                    hid_DeleteEMP.Value += "," + item;
                    arrAddEmp.Remove(item);
                }

                foreach (var item in arrAddEmp)
                {
                    hid_AddEMP.Value += "," + item;
                }



            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2DC0600Save_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox txt_EMP_ID = new TextBox();
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {

                txt_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID");

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    txt_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_ID");

                }

            }

            hid_AddEMP.Value += "," + txt_EMP_ID.Text;

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10000;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2DC0600Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2DC0600Save.Visible = false;
            WFB2DC0600Cancel.Visible = false;
            WFB2DC0600Add.Visible = true;
            WFB2DC0600Delete.Visible = true;
            WFB2DC0600Add2.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0600Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2DC0600Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC0600Delete.Visible = true;
        }

        WFB2DC0600Save.Visible = false;
        WFB2DC0600Cancel.Visible = false;
        WFB2DC0600Add.Visible = true;
        WFB2DC0600Add2.Visible = true;
    }

    //產生
    protected void WFB2DC0600Add2_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //一括申請, 產生資料時, 應不用勾選指定, 即依查詢明細結果所有工號產生! 
                //if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                //{
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());

                //}
            }
            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('產生請選擇一筆資料')", true);
                return;
            }
            else
            {
                CFB2DC0600DAO dao = new CFB2DC0600DAO();

                dao.ABNORMAL_TYPE = ddl_ABNORMAL_TYPE.SelectedValue;
                dao.ABNORMAL_REASON_CD = ddl_ABNORMAL_REASON_CD.SelectedValue;
                dao.CALENDAR_DT = txt_CALENDAR_DT.Text;
                dao.ABNORMAL_DT = txt_ABNORMAL_DT.Text;
                dao.HOUR = ddl_HOUR.SelectedValue;
                dao.MINUTE = ddl_MINUTE.SelectedValue;
                dao.ABNORMAL_SOURCE_CD = "2";
                dao.IS_RE_MAKE = rbl_IS_RE_MAKE.SelectedValue;
                dao.IS_CONFIRM = ddl_IS_CONFIRM.SelectedValue;
                dao.REMARK = txt_REMARK.Text;

                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2DC060";

                string msg = service.batchABNORMAL_APPLY(dao, emp_data);
                if (msg != "0")
                {
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
                }
                else
                {
                    showMessage("addSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        string dept_no = txt_DEPT_NO.Text;
        if (!string.IsNullOrEmpty(dept_no))
        {
            DataTable dt = dao.getDEPT_NAME(dept_no);
            if (dt.Rows.Count == 1)
            {
                txt_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
            }
            else
            {
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DEPT_NOerror", "alert('部門代號輸入錯誤');", true);
            }
        }
        else
        {
            txt_DEPT_NAME.Text = "";
        }
    }
    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        string work_shift_cd = txt_WORK_SHIFT_CD.Text;
        if (!string.IsNullOrEmpty(work_shift_cd))
        {
            DataTable dt = dao.getWORK_SHIFT_DESC(work_shift_cd);
            if (dt.Rows.Count == 1)
            {
                txt_WORK_SHIFT_DESC.Text = Convert.ToString(dt.Rows[0]["WORK_SHIFT_DESC"]);
            }
            else
            {
                txt_WORK_SHIFT_CD.Text = "";
                txt_WORK_SHIFT_DESC.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "WORK_SHIFT_CDerror", "alert('輪值表代號輸入錯誤');", true);
            }
        }
        else
        {
            txt_WORK_SHIFT_DESC.Text = "";
        }
    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        TextBox emp_id = null;
        TextBox emp_desc = null;
        Label plant_desc = null;
        Label dept_full_name = null;
        Label work_shift_cd = null;
        if (gv_result.Rows.Count > 0)
        {
            emp_id = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_ID");
            emp_desc = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_NAME");
            plant_desc = (Label)gv_result.FooterRow.FindControl("lb_NEW_PLANT_CD");
            dept_full_name = (Label)gv_result.FooterRow.FindControl("lb_NEW_DEPT_NAME");
            work_shift_cd = (Label)gv_result.FooterRow.FindControl("lb_NEW_WORK_SHIFT_CD");
        }


        else
        {
            emp_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID");
            emp_desc = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_NAME");
            plant_desc = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_NEW_PLANT_CD");
            dept_full_name = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_NEW_DEPT_NAME");
            work_shift_cd = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_NEW_WORK_SHIFT_CD");
        }

        if (!string.IsNullOrEmpty(emp_id.Text))
        {
            DataTable dt = dao.getEmp_Name_add(emp_id.Text);
            if (dt.Rows.Count == 1)
            {
                emp_desc.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                plant_desc.Text = Convert.ToString(dt.Rows[0]["PLANT_NAME"]);
                dept_full_name.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                work_shift_cd.Text = Convert.ToString(dt.Rows[0]["WORK_SHIFT_DESC"]);
            }
            else
            {
                emp_id.Text = "";
                emp_desc.Text = "";
                plant_desc.Text = "";
                dept_full_name.Text = "";
                work_shift_cd.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            emp_desc.Text = "";
            plant_desc.Text = "";
            dept_full_name.Text = "";
            work_shift_cd.Text = "";
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DC0600_Is_Search"] = "Y";
        Response.Redirect("WFB2DC0600_Qry.aspx");
    }
}