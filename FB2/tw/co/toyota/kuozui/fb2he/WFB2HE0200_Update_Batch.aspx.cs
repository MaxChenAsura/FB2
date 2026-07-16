using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2he_WFB2HE0200_Update_Batch : BasePage
{
    CFB2HE0200BO service = new CFB2HE0200BO();
  
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        ViewState["Queryble"] = false;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getEMP_CD();
            getPLANT_CD();
            getLEVEL_CD();
            getGRADE_CD();
            getWORK_CD();
            getADOPT_RESULT1();

            getINTERVIEW_PROCESS_STATUS();
            getINTERVIEW_RESULT();
            getADOPT_RESULT();
            getAPPROVE_STATUS();

            //this.exportExcel();  

        }        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {            
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getEMP_CD()
    {
        try
        {
            ddl_EMP_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getPLANT_CD()
    {
        try
        {
            ddl_PLANT_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "PLANT_CD", "", "");
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
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getLEVEL_CD()
    {
        try
        {
            ddl_LEVEL_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getLEVEL_CD();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tt = dt.Rows[i]["LEVEL_CD"].ToString();
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }

            }
            ddl_LEVEL_CD.SelectedValue = "5A";//預設5A
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getGRADE_CD()
    {
        try
        {
            ddl_GRADE_CD.Items.Clear();
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            DataTable dt = new DataTable();
            dt = service.getGRADE_CD(dao);
            ddl_GRADE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GRADE_CD.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                }

            }
            ddl_GRADE_CD.SelectedValue = "2";//預設2
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getWORK_CD()
    {
        try
        {
            ddl_WORK_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    private void getINTERVIEW_PROCESS_STATUS()
    {
        try
        {
            ddl_INTERVIEW_PROCESS_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "INTERVIEW_PROCESS_STATUS", "", "");
            ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getINTERVIEW_RESULT()
    {
        try
        {
            ddl_INTERVIEW_RESULT.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "INTERVIEW_RESULT", "", "");
            ddl_INTERVIEW_RESULT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INTERVIEW_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_RESULT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getADOPT_RESULT()
    {
        try
        {
            ddl_ADOPT_RESULT.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "ADOPT_RESULT", "", "");
            ddl_ADOPT_RESULT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ADOPT_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ADOPT_RESULT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getADOPT_RESULT1()
    {
        try
        {
            ddl_ADOPT_RESULT1.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "ADOPT_RESULT", "", "");
            ddl_ADOPT_RESULT1.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ADOPT_RESULT1.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ADOPT_RESULT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getAPPROVE_STATUS()
    {
        try
        {
            ddl_APPROVE_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SA", "APPROVE_STATUS", "", "");
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_APPROVE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_APPROVE_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //LEVEL_CD選擇後查詢GRADE_CD
    protected void ddl_LEVEL_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_GRADE_CD.Items.Clear();
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;

            DataTable dt = new DataTable();

            dt = service.getGRADE_CD(dao);
            ddl_GRADE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GRADE_CD.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                }

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region Grid事件
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
                getSortDirection("LICENSE_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HE0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[29].Visible = false;
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
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
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }
    #endregion

    protected void WFB2HE0201SEARCH_Click(object sender, EventArgs e)
    {        
        try
        {
            ViewState["Queryble"] = true;           
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("LICENSE_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("LICENSE_ID", 0, 10);
            //end

            if (gv_result.Rows.Count > 0)
            {
                
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
               
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0201SEARCH, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }   

    //儲存
    protected void WFB2HE0201SAVE_Click(object sender, EventArgs e)
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

            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請至少選擇一筆資料!')", true);
                return;
            }
            else
            {
                CFB2HE0200DAO dao =  new CFB2HE0200DAO();
                ArrayList datas = new ArrayList();
                List<string> LICENSE_ID = new List<string>();
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        //gv_result.Rows[i].Cells[29].Text;
                        datas.Add(new string[] {gv_result.Rows[i].Cells[6].Text
                                             ,gv_result.Rows[i].Cells[2].Text
                                             , gv_result.Rows[i].Cells[17].Text                                           
                                        });
                    }
                }

                //畫面參數
                dao.EMP_CD = ddl_EMP_CD.SelectedValue;
                dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
                dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
                dao.GRADE_CD = ddl_GRADE_CD.SelectedValue;
                dao.WORK_CD = ddl_WORK_CD.SelectedValue;
                dao.DEPT_NO = txt_DEPT_NO.Text;
                dao.JOIN_DT = txt_JOIN_DT.Text;
                dao.PLAN_DESPATCH_DT = txt_PLAN_DESPATCH_DT.Text;
                dao.EXAM_EXPIRE_DT = txt_EXAM_EXPIRE_DT.Text;
                dao.ADOPT_RESULT1 = ddl_ADOPT_RESULT1.SelectedValue;
                dao.ADOPT_BY1 = txt_ADOPT_BY1.Text;
                dao.ADOPT_DT1 = txt_ADOPT_DT1.Text;


                string msg = service.updateEmp(datas, dao);

                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "執行錯誤：" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;
                }
                else
                {
                    showMessage("executeSuccessMessage");
                    WFB2HE0201SEARCH_Click(null, null);
                }

            }



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0201SAVE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HE0200Back_Click(object sender, EventArgs e)
    {
        Session["HE0200_Is_Search"] = "Y";
        Response.Redirect("WFB2HE0200_Qry.aspx");
    }


}