using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sn_WFB2SN0100_Qry : BasePage
{
    CFB2SN0100BO service = new CFB2SN0100BO();
    public static string type = "";
    public static string key1 = "";
    public static string key2 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        ViewState["Queryble"] = false;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value           
            txt_YEAR.Text = DateTime.Now.ToString("yyyy");
            txt_SEARCH_YEAR.Text = DateTime.Now.ToString("yyyy");
            createAFA_FOR();

            create_SEARCH_AFA_FOR();           

            this.exportExcel();  
        }

        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {            
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void createAFA_FOR()
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2SN0100DAO dao = new CFB2SN0100DAO();
            dao.YEAR = txt_YEAR.Text;
            dt = service.afa_for_Data(dao);
            ddl_AFA_FOR.Items.Clear();
            ddl_AFA_FOR.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_AFA_FOR.Items.Add(new ListItem(dt.Rows[i]["showWord"].ToString(), dt.Rows[i]["id"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void create_SEARCH_AFA_FOR()
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2SN0100DAO dao = new CFB2SN0100DAO();
            dao.YEAR = txt_SEARCH_YEAR.Text;
            dt = service.search_afa_for_Data(dao);
            ddl_SEARCH_AFA_FOR.Items.Clear();
            ddl_SEARCH_AFA_FOR.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SEARCH_AFA_FOR.Items.Add(new ListItem(dt.Rows[i]["showWord"].ToString(), dt.Rows[i]["id"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("AFA_APPROVE_MARK DESC,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SN0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[9].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string tt = e.Row.Cells[9].Text;
            if (e.Row.Cells[9].Text == "V")
            {
                ((CheckBox)e.Row.FindControl("cb_check")).Checked = true;

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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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

    protected void txt_YEAR_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_YEAR.Text.Length == 4)
            {
                createAFA_FOR();
            }            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_SEARCH_YEAR_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_SEARCH_YEAR.Text.Length == 4)
            {
                create_SEARCH_AFA_FOR();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SN0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;            
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SN0200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SN0100ExcelDown_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SN010.xlsx")))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2SN010.xlsx"));  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2SN010.xlsx", System.Text.Encoding.UTF8));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

            }

        }
    }
    protected void WFB2SN0100Upload_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SN0100DAO dao = new CFB2SN0100DAO();

            //檢核1:判斷登入者有無權限上傳
            string userid = SessionHandle.Current.emp_id;
            string flag = "";
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("SN", "AFA_ENTRY", "", "");  //sub_cd
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CODE_VAL1"].ToString() == userid)
                    {
                        flag = "0";
                    } 
                }
                if (flag != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "您沒有上傳資料的權限!!" + "');", true);
                    return;
                }            
            }
                        
            string tempId = ddl_AFA_FOR.SelectedValue;
            string[] lines = Regex.Split(tempId, ":");
            dao.TYPE = lines[0];//哪一種獎金
            dao.KEY1 = lines[1];
            dao.KEY2 = lines[2];
            dt.Clear();
            if (dao.TYPE == "a") //年獎
            {
                 dt = service.is_AWARD_approve(dao);
                if (dt.Rows.Count > 0)
                {
                    //檢核2:原作業機能主檔已經簽核完成(APPROVE_BY <> '')
                    if (dt.Rows[0]["APPROVE_BY"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "年獎作業未完成簽核，無法上傳檔案!!" + "');", true);
                        return;
                    }
                    //檢核3:主檔是否已經薪資轉出(SALARY_TRANS_BY <> '' )
                    if (dt.Rows[0]["SALARY_TRANS_BY"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "年獎作業已薪資轉出，無法上傳檔案!!" + "');", true);
                        return;
                    }
                }
            }
            else if (dao.TYPE == "b")//紅利
            {
                dt = service.is_BONUS_approve(dao);
                if (dt.Rows.Count > 0)
                {
                    //檢核2:原作業機能主檔已經簽核完成(APPROVE_BY <> '')
                    if (dt.Rows[0]["APPROVE_BY"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "紅利作業未完成簽核，無法上傳檔案!!" + "');", true);
                        return;
                    }
                    //檢核3:主檔是否已經薪資轉出(SALARY_TRANS_BY <> '' )
                    if (dt.Rows[0]["SALARY_TRANS_BY"].ToString() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "紅利作業已薪資轉出，無法上傳檔案!!" + "');", true);
                        return;
                    }
                }
            }
            else //一時金
            {
                dt = service.is_FESTIVAL_approve(dao);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //檢核2:原作業機能主檔已經簽核完成(APPROVE_BY <> '')
                        if (dt.Rows[i]["APPROVE_BY"].ToString() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "一時金作業未完成簽核，無法上傳檔案!!" + "');", true);
                            return;
                        }
                        //檢核3:主檔是否已經薪資轉出(SALARY_TRANS_BY <> '' )
                        if (dt.Rows[i]["SALARY_TRANS_BY"].ToString() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "一時金作業未完成簽核，無法上傳檔案!!" + "');", true);
                            return;
                        }
                    }
                    
                }
            }            
            
            //開始上傳作業
            if (FileUpload.HasFile)
            {
                //判斷上傳檔案是否錯誤
                String filename = System.IO.Path.GetExtension(FileUpload.PostedFile.FileName);
                if (filename != ".xlsx")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "上傳檔案格式錯誤" + "');", true);
                    return;
                }
                
                dao.AFA_FOR = ddl_AFA_FOR.SelectedValue;
                dao.YEAR = txt_YEAR.Text.Replace("/", "");
                //string msg = service.uploadExcel(FileUpload.FileContent, System.IO.Path.GetExtension(FileUpload.PostedFile.FileName), dao);
                IWorkbook workbook = service.uploadExcel(FileUpload.FileContent, System.IO.Path.GetExtension(FileUpload.PostedFile.FileName), dao);


                if (workbook == null)
                {
                    create_SEARCH_AFA_FOR();
                    //WFB2SN0100Search_Click(null, null);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                }
                else
                {
                    #region 存在SERVER取代SESSION
                    //刪除檔案
                    File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx"));

                    string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                    FileStream file = new FileStream(@toPath + "/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion
                    //Session["workbook_SH0200"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SN0100_Qry.aspx";
                    Session["FileType_SN010"] = "excelERR";

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                }

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");

            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SN010"] != null && Session["FileType_SN010"].ToString() != "")
            {
                string fileType = Session["FileType_SN010"].ToString();
                if (fileType == "excel")
                {
                    Session["FileType_SN010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SN010_SAMPLE.xlsx");


                }
                if (fileType == "excelERR")
                {
                    Session["FileType_SN010"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SN010_ERR_" + SessionHandle.Current.emp_id + ".xlsx"), "檢核錯誤說明.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    
}