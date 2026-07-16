
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SA3100_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SA3100BO SA310BO = new CFB2SA3100BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //txt_YM.Text = "2020/01";  //測試用
            //匯出EXCEL檔
            this.exportExcel();

            //取得  資料
            initialValue();
            
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            getQryField();

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //寄件否
            dt = utilities.getCommCode("99", "IS_MAIL", "", "", "Y");
            ddl_IS_MAIL.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_IS_MAIL.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }            

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


    #region GridView的必要function
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
                getSortDirection("START_DT desc,EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SA3100_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

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
        gv_result.DataKeyNames = new string[] { "HR_CHG_NO", "EMP_ID" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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


    #region button 事件
    //查詢功能
    protected void WFB2SA3100Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            //setQryField(true);

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SA3100DEL_MAIL.Visible = false;
                WFB2SA3100EXCELDOWN.Visible = false;
                WFB2SA3100DO_MAIL.Visible = false;
                WFB2SA3100MAIL.Visible = false;
                WFB2SA3100RE_MAIL.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2SA3100DEL_MAIL.Visible = true;
                WFB2SA3100RE_MAIL.Visible = true;
                WFB2SA3100DO_MAIL.Visible = true;
                WFB2SA3100EXCELDOWN.Visible = true;
                WFB2SA3100MAIL.Visible = true;
                
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //EXCEL下載
    protected void WFB2SA3100EXCELDOWN_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SA3100DAO dao = new CFB2SA3100DAO();
            dao.YM =txt_YM.Text.Replace("/","");
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.IS_MAIL = ddl_IS_MAIL.SelectedValue;


            DataTable dt = new DataTable();
            //取得下載資料
            dt = dao.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SA310_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = SA310BO.excelDownload(Server.MapPath("~/ExcelTemplate/WFB2SA310.xlsx"), dao);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SA310_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            dwnframe.Attributes["src"] = "WFB2SA3100_Qry.aspx?FileType_SA3100 = excel";
            Session["FileType_SA3100"] = "excel";
            if (workbook != null)
            {
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("'", "\"") + "');", true);
        }
    }

    //重寄送(修改N)
    protected void WFB2SA3100RE_MAIL_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(
                          gv_result.DataKeys[i].Values["HR_CHG_NO"].ToString()
                        , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        ));
                }
            }
            if (keysList.Count() == 0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }

            string msg = "";
            //進行修改作業
            msg = SA310BO.updSave(keysList,"N");

            //成功修改的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }
            //重整畫面
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
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }
    }
    //已寄(修改為Y)
    protected void WFB2SA3100DO_MAIL_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(
                          gv_result.DataKeys[i].Values["HR_CHG_NO"].ToString()
                        , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        ));
                }
            }
            if (keysList.Count() == 0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }

            string msg = "";

            //進行修改作業
            msg = SA310BO.updSave(keysList, "Y");

            //成功修改的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }
            //重整畫面
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
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }

    }
    //不寄送(修改為D)
    protected void WFB2SA3100DEL_MAIL_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(
                          gv_result.DataKeys[i].Values["HR_CHG_NO"].ToString()
                        , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        ));
                }
            }
            if (keysList.Count() == 0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }

            string msg = "";
        
            //進行修改作業
            msg = SA310BO.updSave(keysList,"D");

            //成功修改的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }
            //重整畫面
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
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }
    }

    //寄件通知(多筆查詢明細)
    protected void WFB2SA3100MAIL_Click(object sender, EventArgs e)
    {
        try
        {
             //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (  ((HiddenField)gv_result.Rows[i].FindControl("hid_IS_MAIL")).Value=="N")
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() ==0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('無未寄出的資料!')", true);
                return;
            }


            //保留查詢資料
            setQryField(true);
            Response.Redirect("WFB2SA3100_Dtl.aspx?");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //對象生成
    protected void WFB2SA3100GEN_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SA3100DAO dao = new CFB2SA3100DAO();
            dao.YM = txt_YM.Text.Replace("/", "");
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SA310";

            string msg = "0";
 
            //執行轉薪資SP
            msg = SA310BO.exec_GEN_2SCHG_SALARY_EMP(dao);

            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
            }

            //重整畫面
            WFB2SA3100Search_Click(sender, e);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    #endregion

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SA3100"] != null && Session["FileType_SA3100"].ToString() != "")
            {
                Session["FileType_SA3100"] = "";
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SA310_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SA310Excel.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SA3100_Is_Search").ToString() == "Y")
            {
                txt_YM.Text = hashtable_get("SA3100_txt_YM").ToString();
                txt_EMP_ID.Text = hashtable_get("SA3100_txt_EMP_ID").ToString();
                ddl_IS_MAIL.SelectedValue = hashtable_get("SA3100_ddl_IS_MAIL").ToString();

                ViewState["PerPageRow"] = hashtable_get("SA3100_ddlPerPageRow").ToString();
                WFB2SA3100Search_Click(null, null);
                setQryField(false);
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
            hashtable_set("SA3100_txt_YM", txt_YM.Text);
            hashtable_set("SA3100_txt_EMP_ID", txt_EMP_ID.Text);
            hashtable_set("SA3100_ddl_IS_MAIL", ddl_IS_MAIL.SelectedValue);
            hashtable_set("SA3100_ddl_IS_MAIL_DESC", ddl_IS_MAIL.SelectedItem);
        }
        else
        {
            hashtable_set("SA3100_Is_Search", "N");
        }
    }

    
   

    #endregion





}

