using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2IA1300_Qry : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            if (Session["IA1300_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }
        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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
                getSortDirection("COMPANY_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "COMPANY_CD", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["IA1300_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "COMPANY_CD" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            #region 設定header多列

            GridViewRow gvHeaderRow = e.Row;
            GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gvHeaderRowCopy.CssClass = "header";
            this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

            int headerCellCount = gvHeaderRow.Cells.Count;
            int cellIndex = 0;

            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 9 && i <= 14)
                {
                    cellIndex++;
                }
                else
                {
                    TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                    tcHeader.RowSpan = 2;
                    gvHeaderRowCopy.Cells.Add(tcHeader);
                }
            }


            TableCell tcMergeProduct = new TableCell();
            tcMergeProduct.Text = "勞保";
            tcMergeProduct.ColumnSpan = 2;
            gvHeaderRowCopy.Cells.AddAt(9, tcMergeProduct);

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = "勞退";
            tcMergeProduct.ColumnSpan = 2;
            gvHeaderRowCopy.Cells.AddAt(10, tcMergeProduct);

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = "健保";
            tcMergeProduct.ColumnSpan = 2;
            gvHeaderRowCopy.Cells.AddAt(11, tcMergeProduct);


            #endregion
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            #region 有多筆頁籤資料時

            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            gv_result.ShowFooter = false;
            #endregion
        }

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            #region 只有一筆頁籤資料時

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
            tc.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
            #endregion
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
        gv_result.DataKeyNames = new string[] { "COMPANY_CD" };
        getSortDirection(e.SortExpression);
    }

    //查詢按鈕事件
    protected void WFB2IA1300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("COMPANY_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("COMPANY_CD", 0, 10);
            //end
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                tbEdit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true); 
            }
            else
            {
                gv_result.Visible = true;
                tbEdit.Visible = true;
            }
            

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //薪調試算
    protected void WFB2IA1300Detail_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2IA1300_Add.aspx");
    }

    //薪調確定
    //protected void WFB2IA1300Update_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //檢查勾選項目
    //        List<int> selectindex = new List<int>();
    //        List<int> validindex = new List<int>();
    //        for (int i = 0; i < this.gv_result.Rows.Count; i++)
    //        {
    //            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
    //            {
    //                selectindex.Add(i);
    //                if (((Label)gv_result.Rows[i].Cells[15].Controls[1]).Text != "")
    //                    validindex.Add(i+1);
    //            }
    //        }

    //        if (selectindex.Count() == 0)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請點選指定生效日期資料!')", true);
    //            return;
    //        }

    //        if (validindex.Count() > 0)
    //        {
    //            string tmp = "";
    //            for (int i = 0; i < validindex.Count; i++)
    //            {
    //                if (tmp == "") tmp = validindex[i].ToString();
    //                else tmp += "," + validindex[i].ToString();
    //            }
    //            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('第" + tmp + "筆資料生效日期不為空白,不允許勾選!')", true);
    //            return;
    //        }
    //        else
    //        {
    //            bool successed = true;
    //            CFB2IA1300BO bo = new CFB2IA1300BO();
    //            CFB2IA1300DAO fbsIA = new CFB2IA1300DAO();
    //            for (int i = 0; i < selectindex.Count; i++)
    //            {
    //                fbsIA.COMPANY_CD = ((HiddenField)gv_result.Rows[selectindex[i]].Cells[2].FindControl("hid_COMPANY_CD")).Value;//公司別
    //                fbsIA.EMP_ID = ((Label)gv_result.Rows[selectindex[i]].Cells[4].FindControl("lb_EMP_ID")).Text;//工號
    //                fbsIA.SALARY_SYM = ((HiddenField)gv_result.Rows[selectindex[i]].Cells[9].FindControl("hid_SALARY_SYM")).Value;//指定薪調年月起
    //                fbsIA.LICENSE_ID = ((Label)gv_result.Rows[selectindex[i]].Cells[8].FindControl("lb_LICENSE_ID")).Text;//身份證/居留證
    //                fbsIA.AVG_SALARY = ((Label)gv_result.Rows[selectindex[i]].Cells[9].FindControl("lb_AVG_SALARY")).Text.Replace(",", "");//平均薪資
    //                fbsIA.A_OLD_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[10].FindControl("lb_A_OLD_INSAMT")).Text.Replace(",","");//勞保-原投保金額
    //                fbsIA.A_NEW_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[11].FindControl("lb_A_NEW_INSAMT")).Text.Replace(",", "");//勞保-新投保金額
    //                fbsIA.C_OLD_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[12].FindControl("lb_C_OLD_INSAMT")).Text.Replace(",", "");//勞退-原提繳工資
    //                fbsIA.C_NEW_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[13].FindControl("lb_C_NEW_INSAMT")).Text.Replace(",", "");//勞退-新提繳工資
    //                fbsIA.B_OLD_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[14].FindControl("lb_B_OLD_INSAMT")).Text.Replace(",", "");//健保-原投保金額
    //                fbsIA.B_NEW_INSAMT = ((Label)gv_result.Rows[selectindex[i]].Cells[15].FindControl("lb_B_NEW_INSAMT")).Text.Replace(",", "");//健保-新投保金額
    //                fbsIA.CREATED_BY = SessionHandle.Current.emp_id;
    //                fbsIA.UPDATED_BY = SessionHandle.Current.emp_id;
    //                fbsIA.FUNC_ID = "FB2IA130";

    //                fbsIA.EFFECT_DT = txt_DEF_EFFECT_DT.Text;
    //                successed = bo.Confirm_SALARY_ADJUSTMENT(fbsIA);
    //            }

    //            if (successed)
    //                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('薪調確定更新成功');", true);
    //            else
    //                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調確定更新失敗!');", true);

    //            WFB2IA1300Search_Click(this, new EventArgs());

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
    //    }
    //}

    //薪調確定 20150918 改由畫面查詢條件整批作薪調  
    protected void WFB2IA1300Update_Click(object sender, EventArgs e)
    {
        try
        {
            bool successed = true;
            CFB2IA1300BO bo = new CFB2IA1300BO();
            CFB2IA1300DAO dao = new CFB2IA1300DAO();
            dao.EFFECT_DT = txt_DEF_EFFECT_DT.Text;//指定生效日期
            dao.COMPANY_CD = (txt_COMPANY_CD.Text).ToUpper();
            dao.SALARY_SYM = txt_SALARY_SYM.Text.Replace("/", "");
            dao.SALARY_EYM = txt_SALARY_EYM.Text.Replace("/", "");
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EFFECT_DT_S = txt_EFFECT_DT.Text;//查詢條件的生效日期
            dao.LICENSE_ID = (txt_LICENSE_ID.Text).ToUpper();
            dao.is_EFFECTED = rb_is_EFFECTED.SelectedValue;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2IA130";
            
            //生效日期是否已經生效
            if (dao.is_EFFECTED == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調確定時，薪調對象的生效否不可選擇是,請先選擇否後，再做一次查詢動作!')", true);
                return;
            }

            string st = "";
            DataTable dt = dao.checkLeaveData();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count ; i++)
                {
                    st = st + dt.Rows[i]["EMP_ID"].ToString() + "\\n";
                }
            }
            string err = "";
            if (st != "")
            {
                err = "此次薪調對象有已離社人員,請先刪除!已離社名單如下:\\n" + st;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + err + "')", true);
                return;
            }


            successed = bo.Confirm_SALARY_ADJUSTMENT(dao);
            if (successed)
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('薪調確定更新成功');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪調確定更新失敗!');", true);

            WFB2IA1300Search_Click(this, new EventArgs());    

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //刪除明細
    protected void WFB2IA1300Del_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> selectindex = new List<int>();
            List<int> validindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    selectindex.Add(i);
                    if (gv_result.Rows[i].Cells[15].Text != "")
                        validindex.Add(i+1);
                }
            }

            if (selectindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            if (validindex.Count() > 0)
            {
                string tmp = "";
                for (int i = 0; i < validindex.Count; i++)
                {
                    if (tmp == "") tmp = validindex[i].ToString();
                    else tmp += "," + validindex[i].ToString();
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('第" + tmp + "筆資料生效日期不為空白,不允許勾選!')", true);
                return;
            }
            else
            {
                bool successed = true;
                CFB2IA1300BO bo = new CFB2IA1300BO();
                CFB2IA1300DAO fbsIA = new CFB2IA1300DAO();
                for (int i = 0; i < selectindex.Count; i++)
                {

                    fbsIA.COMPANY_CD = ((HiddenField)gv_result.Rows[selectindex[i]].Cells[2].FindControl("hid_COMPANY_CD")).Value;//公司別
                    fbsIA.EMP_ID = ((Label)gv_result.Rows[selectindex[i]].Cells[3].Controls[1]).Text.Trim();//工號
                    fbsIA.SALARY_SYM = ((HiddenField)gv_result.Rows[selectindex[i]].Cells[9].FindControl("hid_SALARY_SYM")).Value;//指定薪調年月起
                    successed = successed && bo.Delete_TB_I_M_LEVEL_CHG(fbsIA);
                }

                if (successed)
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('明細刪除成功');", true);

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

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_COMPANY_CD.Text = Session["IA1300_COMPANY_CD"].ToString();
            txt_COMPANY_SNAME.Text = Session["IA1300_COMPANY_SNAME"].ToString();
            txt_SALARY_SYM.Text = Session["IA1300_SALARY_SYM"].ToString();
            txt_SALARY_EYM.Text = Session["IA1300_SALARY_EYM"].ToString();
            txt_EMP_ID.Text = Session["IA1300_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["IA1300_EMP_NAME"].ToString();
            txt_EFFECT_DT.Text = Session["IA1300_EFFECT_DT"].ToString();
            txt_LICENSE_ID.Text = Session["IA1300_LICENSE_ID"].ToString();
            ViewState["PerPageRow"] = Session["IA1300_ddlPerPageRow"].ToString();

            WFB2IA1300Search_Click(null, null);
            Session["IA1300_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["IA1300_COMPANY_CD"] = txt_COMPANY_CD.Text;
        Session["IA1300_COMPANY_SNAME"] = txt_COMPANY_SNAME.Text;
        Session["IA1300_SALARY_SYM"] = txt_SALARY_SYM.Text;
        Session["IA1300_SALARY_EYM"] = txt_SALARY_EYM.Text;
        Session["IA1300_EMP_ID"] = txt_EMP_ID.Text;
        Session["IA1300_EMP_NAME"] = txt_EMP_NAME.Text;
        Session["IA1300_EFFECT_DT"] = txt_EFFECT_DT.Text;
        Session["IA1300_LICENSE_ID"] = txt_LICENSE_ID.Text;
    }
    #endregion
}