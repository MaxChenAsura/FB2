using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HC0100_Qry : BasePage
{
    //Service 物件
    private CFB2HC0100BO hc010BO = new CFB2HC0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        this.btn_EMP_ID.Attributes.Add("onclick", "OpenEmpSearch('txt_EMP_ID_search','txt_EMP_NAME_search','N');return false;");
        //this.btn_HR_CHG_CD.Attributes.Add("onclick", "OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD_search', 'txt_HR_CHG_CD_DESC_search', 'N');return false;");

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
            //依權限 取得人事異動代碼
            getddl_HR_CHG_CD();
            hid_wfb2hc_CheckBox_NotChoiceMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
            hid_wfb2hc_Delete_Confirm_Message.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;

            string qdatakeys = (Request.QueryString["qdatakey"] == null) ? "" : Request.QueryString["qdatakey"];
            string[] qdatakey = qdatakeys.Split(',');
            if (qdatakey.Length == 5)
            {
                txt_EMP_ID_search.Text = qdatakey[0];
                txt_EMP_NAME_search.Text = hc010BO.getEMP_NAME(qdatakey[0]);
                txt_START_SDT_search.Text = qdatakey[1];
                txt_START_EDT_search.Text = qdatakey[2];
                ddl_HR_CHG_CD.SelectedValue = qdatakey[3];
                //txt_HR_CHG_CD_DESC_search.Text = service.getHR_CHG_DESC(qdatakey[3]);
                rb_HR_CHG_PROC_STATUS_Y_search.Checked = false;
                rb_HR_CHG_PROC_STATUS_N_search.Checked = false;
                rb_HR_CHG_PROC_STATUS_E_search.Checked = false;
                rb_HR_CHG_PROC_STATUS_A_search.Checked = false;
                switch (qdatakey[4])
                {
                    case "Y": rb_HR_CHG_PROC_STATUS_Y_search.Checked = true;
                        break;
                    case "N": rb_HR_CHG_PROC_STATUS_N_search.Checked = true;
                        break;
                    case "E": rb_HR_CHG_PROC_STATUS_E_search.Checked = true;
                        break;
                    case "A": rb_HR_CHG_PROC_STATUS_A_search.Checked = true;
                        break;
                    default: rb_HR_CHG_PROC_STATUS_A_search.Checked = true;
                        break;

                }
                if (Session["HC0100_Is_Search"] == "Y")
                {
                    ViewState["PerPageRow"] = Session["HC0100_ddlPerPageRow"] != null ? Session["HC0100_ddlPerPageRow"].ToString() : "";

                    btn_search_Click(null, null);
                    Session["HC0100_Is_Search"] = "N";
                }
            }

        }
        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getddl_HR_CHG_CD()
    {
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        DataTable dt = dao.getddl_HR_CHG_CD("","FB2HC010_ADD","","");
        ddl_HR_CHG_CD.Items.Clear();
        ddl_HR_CHG_CD.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_HR_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["HR_CHG_CD"].ToString() + "-" + dt.Rows[i]["HR_CHG_DESC"].ToString(), dt.Rows[i]["HR_CHG_CD"].ToString()));
            }
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
                getSortDirection("HR_CHG_NO");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HC0100_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" };
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

            //第幾列到第幾列需要雙層式Header
            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 7 && i <= 16)
                {
                    cellIndex++;
                }
                else
                {
                    TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                    tcHeader.RowSpan = 2;//合併幾層
                    gvHeaderRowCopy.Cells.Add(tcHeader);
                }
            }

            TableCell tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2hc_hd_CHG_CONTENT;  //雙層Header的名稱
            tcMergeProduct.ColumnSpan = 10;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(7, tcMergeProduct);//第個欄位開始

            #endregion
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            #region 有多筆頁籤資料時

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
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
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

        //尚未處理改 為紅色字
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox hid_HR_CHG_PROC_STATUS = (TextBox)e.Row.FindControl("hid_HR_CHG_PROC_STATUS");
            if (hid_HR_CHG_PROC_STATUS.Text != "Y")
            {
                e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
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

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" };
        getSortDirection(e.SortExpression);
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            hid_EMP_ID_search.Value = txt_EMP_ID_search.Text;
            hid_START_SDT_search.Value = txt_START_SDT_search.Text;
            hid_START_EDT_search.Value = txt_START_EDT_search.Text;
            hid_HR_CHG_CD_search.Value = ddl_HR_CHG_CD.SelectedValue;
            if (rb_HR_CHG_PROC_STATUS_Y_search.Checked)
                hid_HR_CHG_PROC_STATUS_search.Value = "Y";
            else if (rb_HR_CHG_PROC_STATUS_N_search.Checked)
                hid_HR_CHG_PROC_STATUS_search.Value = "N";
            else if (rb_HR_CHG_PROC_STATUS_E_search.Checked)
                hid_HR_CHG_PROC_STATUS_search.Value = "E";
            else
                hid_HR_CHG_PROC_STATUS_search.Value = "";

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("HR_CHG_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("HR_CHG_NO", 0, 10);
            //end
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //showMessage("QryNotFoundMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_no_permission_to_emp + "');", true);
            }
            else
            {
                gv_result.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setinit_grid", "setinit_grid(true);", true);
            }
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

    //刪除明細
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            ArrayList datas = new ArrayList();
            string errMag = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    datas.Add(new String[] { gv_result.Rows[i].Cells[17].Text, gv_result.Rows[i].Cells[4].Text });
                    string hid_HR_CHG_PROC_STATUS_val = ((TextBox)gv_result.Rows[i].FindControl("hid_HR_CHG_PROC_STATUS")).Text;
                    string hid_INS_CHG_PROC_STATUS_val = ((TextBox)gv_result.Rows[i].FindControl("hid_INS_CHG_PROC_STATUS")).Text;
                    if (hid_HR_CHG_PROC_STATUS_val == "Y" || hid_INS_CHG_PROC_STATUS_val == "Y")
                    {
                        string HR_CHG_NO = gv_result.Rows[i].Cells[17].Text;
                        if (errMag == "")
                            errMag += String.Format(Resources.Resource.wfb2hc_Edit_Check_CHG_PROC_STATUS_IS_Y_Message, HR_CHG_NO);
                        else
                            errMag += "\\n" + String.Format(Resources.Resource.wfb2hc_Edit_Check_CHG_PROC_STATUS_IS_Y_Message, HR_CHG_NO);
                    }
                }
            }
            if (errMag != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errMag + "');$.unblockUI();", true);
                return;
            }

            CFB2HC0100BO bo = new CFB2HC0100BO();
            bo.Delete(datas);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "successed", "alert('" + Resources.Resource.wfb2hc_Delete_ok + "');$.unblockUI();", true);
            btn_search_Click(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
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

    protected void btn_add_Click(object sender, EventArgs e)
    {
        string qdatakey = "";
        qdatakey += hid_EMP_ID_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_SDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_EDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_CD_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;

        Response.Redirect("WFB2HC0100_Add.aspx?qdatakey=" + qdatakey);
    }

    protected void btn_edit_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        try
        {
            List<int> selectindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    selectindex.Add(i);
                }
            }

            if (selectindex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage + "')", true);
                return;
            }

            string hid_HR_CHG_PROC_STATUS_val = ((TextBox)gv_result.Rows[selectindex[0]].FindControl("hid_HR_CHG_PROC_STATUS")).Text;
            string hid_INS_CHG_PROC_STATUS_val = ((TextBox)gv_result.Rows[selectindex[0]].FindControl("hid_INS_CHG_PROC_STATUS")).Text;
            string HR_CHG_NO = gv_result.Rows[selectindex[0]].Cells[17].Text;
            string EMP_ID = gv_result.Rows[selectindex[0]].Cells[4].Text;
            if (hid_HR_CHG_PROC_STATUS_val == "Y" || hid_INS_CHG_PROC_STATUS_val == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + String.Format(Resources.Resource.wfb2hc_Edit_Check_CHG_PROC_STATUS_IS_Y_Message, HR_CHG_NO) + "')", true);
                return;
            }

            string qdatakey = "";
            qdatakey += hid_EMP_ID_search.Value;
            qdatakey += ",";
            qdatakey += hid_START_SDT_search.Value;
            qdatakey += ",";
            qdatakey += hid_START_EDT_search.Value;
            qdatakey += ",";
            qdatakey += hid_HR_CHG_CD_search.Value;
            qdatakey += ",";
            qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;
            qdatakey += ",";
            qdatakey += HR_CHG_NO;
            qdatakey += ",";
            qdatakey += EMP_ID;
            Response.Redirect("WFB2HC0100_Update.aspx?qdatakey=" + qdatakey);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢明細
    protected void WFB2HC0100Detail_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        try
        {
            List<int> selectindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    selectindex.Add(i);
                }
            }

            if (selectindex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage + "')", true);
                return;
            }

            //string HR_CHG_NO = gv_result.Rows[selectindex[0]].Cells[17].Text;
            //string EMP_ID = gv_result.Rows[selectindex[0]].Cells[4].Text;
            string HR_CHG_NO = gv_result.Rows[selectindex[0]].Cells[17].Text;
            string EMP_ID = gv_result.Rows[selectindex[0]].Cells[4].Text;
            string qdatakey = "";
            qdatakey += hid_EMP_ID_search.Value;
            qdatakey += ",";
            qdatakey += hid_START_SDT_search.Value;
            qdatakey += ",";
            qdatakey += hid_START_EDT_search.Value;
            qdatakey += ",";
            qdatakey += hid_HR_CHG_CD_search.Value;
            qdatakey += ",";
            qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;
            qdatakey += ",";
            qdatakey += HR_CHG_NO;
            qdatakey += ",";
            qdatakey += EMP_ID;
            Response.Redirect("WFB2HC0100_Detail.aspx?qdatakey=" + qdatakey);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0100AddBatch_Click(object sender, EventArgs e)
    {
        string qdatakey = "";
        qdatakey += hid_EMP_ID_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_SDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_EDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_CD_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;

        Response.Redirect("WFB2HC0100_Add_Batch.aspx?qdatakey=" + qdatakey);
    }

    //protected void btn_save_Click(object sender, EventArgs e)
    //{

    //}

    //protected void btn_cancel_Click(object sender, EventArgs e)
    //{

    //}
    protected void WFB2HC0100EffectProc_Click(object sender, EventArgs e)
    {
        string qdatakey = "";
        qdatakey += hid_EMP_ID_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_SDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_EDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_CD_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;

        Response.Redirect("WFB2HC0100_EffectProc.aspx?qdatakey=" + qdatakey);
    }
}