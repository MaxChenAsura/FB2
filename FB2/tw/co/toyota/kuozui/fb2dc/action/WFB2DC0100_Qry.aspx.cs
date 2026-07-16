using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DC0100_Qry : BasePage
{
    //Service 物件
    private CFB2DC0100BO service = new CFB2DC0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生卡鐘類別選單
            createCLOCK_TYPE();
            //產生工廠區分選單
            createPLANT_CD();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //產生卡鐘類別選單
    private void createCLOCK_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DC", "CLOCK_TYPE", "", "");
            ddl_CLOCK_TYPE2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CLOCK_TYPE2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生工廠區分選單
    private void createPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "PLANT_CD", "", "");
            ddl_PLANT_CD2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("CLOCK_TYPE,CLOCK_NO");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //卡鐘類別
            DropDownList ddl_CLOCK_TYPE = (DropDownList)e.Row.FindControl("ddl_CLOCK_TYPE");
            HiddenField hid_CLOCK_TYPE = (HiddenField)e.Row.FindControl("hid_CLOCK_TYPE");
            if (ddl_CLOCK_TYPE != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("DC", "CLOCK_TYPE", "", "");
                ddl_CLOCK_TYPE.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_CLOCK_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid_CLOCK_TYPE != null)
                    ddl_CLOCK_TYPE.SelectedValue = hid_CLOCK_TYPE.Value.Split('-')[0];

            }

            //使用中
            DropDownList ddl_IS_VALID = (DropDownList)e.Row.FindControl("ddl_IS_VALID");
            HiddenField hid_IS_VALID = (HiddenField)e.Row.FindControl("hid_IS_VALID");
            if (ddl_IS_VALID != null)
            {
                ddl_IS_VALID.Items.Add(new ListItem("", "-1"));
                ddl_IS_VALID.Items.Add(new ListItem("Y-使用中", "Y"));
                ddl_IS_VALID.Items.Add(new ListItem("N-不使用", "N"));
                if (hid_IS_VALID != null)
                    ddl_IS_VALID.SelectedValue = hid_IS_VALID.Value;
            }

            //用途區分
            DropDownList ddl_CLOCK_USED_CD = (DropDownList)e.Row.FindControl("ddl_CLOCK_USED_CD");
            HiddenField hid_CLOCK_USED_CD = (HiddenField)e.Row.FindControl("hid_CLOCK_USED_CD");
            if (ddl_CLOCK_TYPE != null && ddl_CLOCK_USED_CD != null)
            {
                DataTable dt = new DataTable();
                //dt = utilities.getCommCode("DC", "CLOCK_USED_CD_" + ddl_CLOCK_TYPE.SelectedValue, "", "");

                //當卡鐘類別為餐廳(B)時用途區分是取共用代碼檔的MAIN_CD = RESTAURANT_CD
                //當卡鐘類別為停車場(C)時用途區分是取共用代碼檔的MAIN_CD = PARKING_PLANT_CD
                if (ddl_CLOCK_TYPE.SelectedValue == "B")
                    dt = utilities.getCommCode("DE", "RESTAURANT_CD", "", "");
                else if (ddl_CLOCK_TYPE.SelectedValue == "C")
                    dt = utilities.getCommCode("DG", "PARKING_PLANT_CD", "", "");

                ddl_CLOCK_USED_CD.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_CLOCK_USED_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid_CLOCK_USED_CD != null)
                    ddl_CLOCK_USED_CD.SelectedValue = hid_CLOCK_USED_CD.Value.Split('-')[0];

                if (ddl_CLOCK_TYPE.SelectedValue == "B" || ddl_CLOCK_TYPE.SelectedValue == "C")
                    ddl_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 215, 215);
                else
                    ddl_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 255, 255);

            }

            //工廠區分
            DropDownList ddl_PLANT_CD = (DropDownList)e.Row.FindControl("ddl_PLANT_CD");
            HiddenField hid_PLANT_CD = (HiddenField)e.Row.FindControl("hid_PLANT_CD");
            if (ddl_PLANT_CD != null)
            {
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
                if (hid_PLANT_CD != null)
                    ddl_PLANT_CD.SelectedValue = hid_PLANT_CD.Value.Split('-')[0];
            }

            //全員使用(停車場)
            DropDownList ddl_PARK_DEFAULT = (DropDownList)e.Row.FindControl("ddl_PARK_DEFAULT");
            HiddenField hid_PARK_DEFAULT = (HiddenField)e.Row.FindControl("hid_PARK_DEFAULT");
            if (ddl_PARK_DEFAULT != null)
            {
                ddl_PARK_DEFAULT.Items.Add(new ListItem("", "-1"));
                ddl_PARK_DEFAULT.Items.Add(new ListItem("Y", "Y"));
                ddl_PARK_DEFAULT.Items.Add(new ListItem("N", "N"));

                if (hid_PARK_DEFAULT != null)
                    ddl_PARK_DEFAULT.SelectedValue = hid_PARK_DEFAULT.Value;

                if (ddl_CLOCK_TYPE.SelectedValue == "C")
                {
                    ddl_PARK_DEFAULT.Enabled = true;
                    ddl_PARK_DEFAULT.BackColor = Color.FromArgb(255, 215, 215);
                }
                else
                {
                    ddl_PARK_DEFAULT.BackColor = Color.FromArgb(255, 255, 255);
                    ddl_PARK_DEFAULT.Enabled = false;
                }

            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lb_IS_VALID = (Label)e.Row.Cells[5].FindControl("lb_IS_VALID");
            if (lb_IS_VALID != null)
            {
                if (lb_IS_VALID.Text == "Y")
                    lb_IS_VALID.Text = "Y-使用中";
                else
                    lb_IS_VALID.Text = "N-不使用";
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

    //GridView資料繫結完成後,格式化資料繫結內容
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

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //卡鐘類別
            DropDownList ddl_NEW_CLOCK_TYPE = (DropDownList)e.Row.FindControl("ddl_NEW_CLOCK_TYPE");
            if (ddl_NEW_CLOCK_TYPE != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("DC", "CLOCK_TYPE", "", "");
                ddl_NEW_CLOCK_TYPE.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_NEW_CLOCK_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            //使用中
            DropDownList ddl_NEW_IS_VALID = (DropDownList)e.Row.FindControl("ddl_NEW_IS_VALID");
            if (ddl_NEW_IS_VALID != null)
            {
                ddl_NEW_IS_VALID.Items.Add(new ListItem("", "-1"));
                ddl_NEW_IS_VALID.Items.Add(new ListItem("Y-使用中", "Y"));
                ddl_NEW_IS_VALID.Items.Add(new ListItem("N-不使用", "N"));
            }

            //工廠區分
            DropDownList ddl_NEW_PLANT_CD = (DropDownList)e.Row.FindControl("ddl_NEW_PLANT_CD");
            if (ddl_NEW_PLANT_CD != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("HB", "PLANT_CD", "", "");
                ddl_NEW_PLANT_CD.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_NEW_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            //全員使用(停車場)
            DropDownList ddl_NEW_PARK_DEFAULT = (DropDownList)e.Row.FindControl("ddl_NEW_PARK_DEFAULT");
            if (ddl_NEW_PARK_DEFAULT != null)
            {
                ddl_NEW_PARK_DEFAULT.Items.Add(new ListItem("", "-1"));
                ddl_NEW_PARK_DEFAULT.Items.Add(new ListItem("Y", "Y"));
                ddl_NEW_PARK_DEFAULT.Items.Add(new ListItem("N", "N"));
                ddl_NEW_PARK_DEFAULT.Enabled = false;
            }
        }

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

            //gv_result.ShowFooter = false;
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2DC0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CLOCK_TYPE,CLOCK_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CLOCK_TYPE,CLOCK_NO", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0100Add.Visible = true;
                WFB2DC0100Edit.Visible = true;
                WFB2DC0100Delete.Visible = true;
            }
            else
            {
                WFB2DC0100Edit.Visible = false;
                WFB2DC0100Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2DC0100Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CLOCK_TYPE,CLOCK_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CLOCK_TYPE,CLOCK_NO", 0, 10);

            WFB2DC0100Save.Visible = true;
            WFB2DC0100Cancel.Visible = true;

            WFB2DC0100Add.Visible = false;
            WFB2DC0100Edit.Visible = false;
            WFB2DC0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> clock_no = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    clock_no.Add(gv_result.DataKeys[i].Values["CLOCK_NO"].ToString());
                }
            }

            string msg = service.deleteCLOCK(clock_no);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2DC0100Search.Visible = false;
            btn_clear.Visible = false;

            WFB2DC0100Save.Visible = true;
            WFB2DC0100Cancel.Visible = true;

            WFB2DC0100Add.Visible = false;
            WFB2DC0100Edit.Visible = false;
            WFB2DC0100Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_CLOCK_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CLOCK_NO");
                TextBox txt_NEW_CLOCK_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CLOCK_DESC");
                DropDownList ddl_NEW_CLOCK_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CLOCK_TYPE");
                DropDownList ddl_NEW_IS_VALID = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_IS_VALID");
                DropDownList ddl_NEW_CLOCK_USED_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CLOCK_USED_CD");
                DropDownList ddl_NEW_PLANT_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PLANT_CD");
                DropDownList ddl_NEW_PARK_DEFAULT = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PARK_DEFAULT");
                TextBox txt_NEW_CLOCK_IP = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CLOCK_IP");

                if ((ddl_NEW_CLOCK_TYPE.SelectedValue == "B" || ddl_NEW_CLOCK_TYPE.SelectedValue == "C") &&
                    ddl_NEW_CLOCK_USED_CD.SelectedValue == "-1")
                {
                    errmsg += "卡鐘類別為餐廳及停車場時，用途區分不可空白\\n";
                }
                if (ddl_NEW_CLOCK_TYPE.SelectedValue == "C" && ddl_NEW_PARK_DEFAULT.SelectedValue == "-1")
                {
                    errmsg += "卡鐘類別為停車場時，全員使用(停車場)不可空白";
                }
                if (errmsg != "")
                {
                    gv_result.PagerSettings.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                    return;
                }

                CFB2DC0100DAO wfb2dc = new CFB2DC0100DAO();
                wfb2dc.CLOCK_NO = txt_NEW_CLOCK_NO.Text.ToUpper();
                wfb2dc.CLOCK_DESC = txt_NEW_CLOCK_DESC.Text;
                wfb2dc.CLOCK_TYPE = ddl_NEW_CLOCK_TYPE.SelectedValue;
                wfb2dc.IS_VALID = ddl_NEW_IS_VALID.SelectedValue;
                if (ddl_NEW_CLOCK_USED_CD.SelectedValue == "-1")
                    wfb2dc.CLOCK_USED_CD = "";
                else
                    wfb2dc.CLOCK_USED_CD = ddl_NEW_CLOCK_USED_CD.SelectedValue;
                wfb2dc.PLANT_CD = ddl_NEW_PLANT_CD.SelectedValue;
                if (ddl_NEW_PARK_DEFAULT.SelectedValue == "-1")
                    wfb2dc.PARK_DEFAULT = "";
                else
                    wfb2dc.PARK_DEFAULT = ddl_NEW_PARK_DEFAULT.SelectedValue;
                wfb2dc.CLOCK_IP = txt_NEW_CLOCK_IP.Text;
                wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.FUNC_ID = "FB2DC010";

                string msg = service.addCLOCK(wfb2dc);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_CLOCK_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CLOCK_NO");
                    TextBox txt_NEW_CLOCK_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CLOCK_DESC");
                    DropDownList ddl_NEW_CLOCK_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CLOCK_TYPE");
                    DropDownList ddl_NEW_IS_VALID = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_IS_VALID");
                    DropDownList ddl_NEW_CLOCK_USED_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CLOCK_USED_CD");
                    DropDownList ddl_NEW_PLANT_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PLANT_CD");
                    DropDownList ddl_NEW_PARK_DEFAULT = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PARK_DEFAULT");
                    TextBox txt_NEW_CLOCK_IP = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CLOCK_IP");

                    if ((ddl_NEW_CLOCK_TYPE.SelectedValue == "B" || ddl_NEW_CLOCK_TYPE.SelectedValue == "C") &&
                         ddl_NEW_CLOCK_USED_CD.SelectedValue == "-1")
                    {
                        errmsg += "卡鐘類別為餐廳及停車場時，用途區分不可空白\\n";
                    }
                    if (ddl_NEW_CLOCK_TYPE.SelectedValue == "C" && ddl_NEW_PARK_DEFAULT.SelectedValue == "-1")
                    {
                        errmsg += "卡鐘類別為停車場時，全員使用(停車場)不可空白";
                    }
                    if (errmsg != "")
                    {
                        gv_result.PagerSettings.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                        return;
                    }

                    CFB2DC0100DAO wfb2dc = new CFB2DC0100DAO();
                    wfb2dc.CLOCK_NO = txt_NEW_CLOCK_NO.Text.ToUpper();
                    wfb2dc.CLOCK_DESC = txt_NEW_CLOCK_DESC.Text;
                    wfb2dc.CLOCK_TYPE = ddl_NEW_CLOCK_TYPE.SelectedValue;
                    wfb2dc.IS_VALID = ddl_NEW_IS_VALID.SelectedValue;
                    if (ddl_NEW_CLOCK_USED_CD.SelectedValue == "-1")
                        wfb2dc.CLOCK_USED_CD = "";
                    else
                        wfb2dc.CLOCK_USED_CD = ddl_NEW_CLOCK_USED_CD.SelectedValue;
                    wfb2dc.PLANT_CD = ddl_NEW_PLANT_CD.SelectedValue;
                    if (ddl_NEW_PARK_DEFAULT.SelectedValue == "-1")
                        wfb2dc.PARK_DEFAULT = "";
                    else
                        wfb2dc.PARK_DEFAULT = ddl_NEW_PARK_DEFAULT.SelectedValue;
                    wfb2dc.CLOCK_IP = txt_NEW_CLOCK_IP.Text;
                    wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC010";

                    string msg = service.addCLOCK(wfb2dc);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }
                }
                else
                {
                    //更新
                    TextBox txt_CLOCK_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_CLOCK_DESC");
                    DropDownList ddl_CLOCK_TYPE = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_CLOCK_TYPE");
                    DropDownList ddl_IS_VALID = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_IS_VALID");
                    DropDownList ddl_CLOCK_USED_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_CLOCK_USED_CD");
                    DropDownList ddl_PLANT_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_PLANT_CD");
                    DropDownList ddl_PARK_DEFAULT = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_PARK_DEFAULT");
                    TextBox txt_CLOCK_IP = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_CLOCK_IP");

                    if ((ddl_CLOCK_TYPE.SelectedValue == "B" || ddl_CLOCK_TYPE.SelectedValue == "C") &&
                         ddl_CLOCK_USED_CD.SelectedValue == "-1")
                    {
                        errmsg += "卡鐘類別為餐廳及停車場時，用途區分不可空白\\n";
                    }
                    if (ddl_CLOCK_TYPE.SelectedValue == "C" && ddl_PARK_DEFAULT.SelectedValue == "-1")
                    {
                        errmsg += "卡鐘類別為停車場時，全員使用(停車場)不可空白";
                    }
                    if (errmsg != "")
                    {
                        gv_result.PagerSettings.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                        return;
                    }

                    CFB2DC0100DAO wfb2dc = new CFB2DC0100DAO();
                    wfb2dc.CLOCK_NO = gv_result.DataKeys[gv_result.EditIndex].Values["CLOCK_NO"].ToString();
                    wfb2dc.CLOCK_DESC = txt_CLOCK_DESC.Text;
                    wfb2dc.CLOCK_TYPE = ddl_CLOCK_TYPE.SelectedValue;
                    wfb2dc.IS_VALID = ddl_IS_VALID.SelectedValue;
                    if (ddl_CLOCK_USED_CD.SelectedValue == "-1")
                        wfb2dc.CLOCK_USED_CD = "";
                    else
                        wfb2dc.CLOCK_USED_CD = ddl_CLOCK_USED_CD.SelectedValue;
                    wfb2dc.PLANT_CD = ddl_PLANT_CD.SelectedValue;
                    if (ddl_PARK_DEFAULT.SelectedValue == "-1")
                        wfb2dc.PARK_DEFAULT = "";
                    else
                        wfb2dc.PARK_DEFAULT = ddl_PARK_DEFAULT.SelectedValue;
                    wfb2dc.CLOCK_IP = txt_CLOCK_IP.Text.Trim();
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC010";

                    string msg = service.updateCLOCK(wfb2dc);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2DC0100Search.Visible = true;
            btn_clear.Visible = true;

            WFB2DC0100Save.Visible = false;
            WFB2DC0100Cancel.Visible = false;
            WFB2DC0100Add.Visible = true;
            WFB2DC0100Edit.Visible = true;
            WFB2DC0100Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0100Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2DC0100Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC0100Edit.Visible = true;
            WFB2DC0100Delete.Visible = true;
        }

        WFB2DC0100Save.Visible = false;
        WFB2DC0100Cancel.Visible = false;
        WFB2DC0100Add.Visible = true;
    }
    protected void ddl_CLOCK_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //取得該列的dropdownlist在將值填入
            //用途區分依卡鐘類別而不同
            DropDownList ddl_CLOCK_USED_CD = (DropDownList)gv_result.Rows[rowIndex].FindControl("ddl_CLOCK_USED_CD");
            //全員使用(停車場)
            DropDownList ddl_PARK_DEFAULT = (DropDownList)gv_result.Rows[rowIndex].FindControl("ddl_PARK_DEFAULT");

            if (ddl_CLOCK_USED_CD != null && ddl != null)
            {
                ddl_CLOCK_USED_CD.Items.Clear();
                DataTable dt = new DataTable();
                //dt = utilities.getCommCode("DC", "CLOCK_USED_CD_" + ddl.SelectedValue, "", "");

                //當卡鐘類別為餐廳(B)時用途區分是取共用代碼檔的MAIN_CD = RESTAURANT_CD
                //當卡鐘類別為停車場(C)時用途區分是取共用代碼檔的MAIN_CD = PARKING_PLANT_CD
                if (ddl.SelectedValue == "B")
                    dt = utilities.getCommCode("DE", "RESTAURANT_CD", "", "");
                else if (ddl.SelectedValue == "C")
                    dt = utilities.getCommCode("DG", "PARKING_PLANT_CD", "", "");

                ddl_CLOCK_USED_CD.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_CLOCK_USED_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

                if (ddl.SelectedValue == "B" || ddl.SelectedValue == "C")
                    ddl_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 215, 215);
                else
                    ddl_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 255, 255);

                if (ddl_PARK_DEFAULT != null)
                {
                    if (ddl.SelectedValue == "C")
                    {
                        ddl_PARK_DEFAULT.Enabled = true;
                        ddl_PARK_DEFAULT.BackColor = Color.FromArgb(255, 215, 215);
                    }
                    else
                    {
                        ddl_PARK_DEFAULT.SelectedValue = "-1";
                        ddl_PARK_DEFAULT.BackColor = Color.FromArgb(255, 255, 255);
                        ddl_PARK_DEFAULT.Enabled = false;
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
    protected void ddl_NEW_CLOCK_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;
            //用途區分依卡鐘類別而不同
            DropDownList ddl_NEW_CLOCK_USED_CD = new DropDownList();
            //全員使用(停車場)
            DropDownList ddl_NEW_PARK_DEFAULT = new DropDownList();
            //用途區分依卡鐘類別而不同
            //取得該列的dropdownlist在將值填入
            if (gv_result.Rows.Count == 0)
            {
                ddl_NEW_CLOCK_USED_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CLOCK_USED_CD");
                ddl_NEW_PARK_DEFAULT = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PARK_DEFAULT");
            }
            else
            {
                ddl_NEW_CLOCK_USED_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CLOCK_USED_CD");
                ddl_NEW_PARK_DEFAULT = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PARK_DEFAULT");
            }

            if (ddl_NEW_CLOCK_USED_CD != null && ddl != null)
            {
                ddl_NEW_CLOCK_USED_CD.Items.Clear();
                DataTable dt = new DataTable();
                //dt = utilities.getCommCode("DC", "CLOCK_USED_CD_" + ddl.SelectedValue, "", "");

                //當卡鐘類別為餐廳(B)時用途區分是取共用代碼檔的MAIN_CD = RESTAURANT_CD
                //當卡鐘類別為停車場(C)時用途區分是取共用代碼檔的MAIN_CD = PARKING_CD
                if (ddl.SelectedValue == "B")
                    dt = utilities.getCommCode("DE", "RESTAURANT_CD", "", "");
                else if (ddl.SelectedValue == "C")
                    dt = utilities.getCommCode("DG", "PARKING_CD", "", "");

                ddl_NEW_CLOCK_USED_CD.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_NEW_CLOCK_USED_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

                if (ddl.SelectedValue == "B" || ddl.SelectedValue == "C")
                    ddl_NEW_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 215, 215);
                else
                    ddl_NEW_CLOCK_USED_CD.BackColor = Color.FromArgb(255, 255, 255);

                if (ddl_NEW_PARK_DEFAULT != null)
                {
                    if (ddl.SelectedValue == "C")
                    {
                        ddl_NEW_PARK_DEFAULT.Enabled = true;
                        ddl_NEW_PARK_DEFAULT.BackColor = Color.FromArgb(255, 215, 215);
                    }
                    else
                    {
                        ddl_NEW_PARK_DEFAULT.SelectedValue = "-1";
                        ddl_NEW_PARK_DEFAULT.BackColor = Color.FromArgb(255, 255, 255);
                        ddl_NEW_PARK_DEFAULT.Enabled = false;
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

    protected void hid_getCLOCK_DESC_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCLOCK_DESC(txt_CLOCK_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_CLOCK_DESC2.Text = dt.Rows[0]["CLOCK_DESC"].ToString();
            }
            else
            {
                txt_CLOCK_DESC2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}