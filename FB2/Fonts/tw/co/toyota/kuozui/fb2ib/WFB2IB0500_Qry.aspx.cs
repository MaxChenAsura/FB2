using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ib_WFB2IB0500_Qry : BasePage
{
    CFB2IB0500BO service = new CFB2IB0500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {            
            //initial value           
            getYM();
            //將Session 的workbook 匯出Excel
            this.exportExcel();     
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getYM()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getYM();

            if (dt.Rows.Count > 0)
            {
                txt_SALARY_YM.Text = dt.Rows[0]["YM"].ToString();
            }
            else
                txt_SALARY_YM.Text = DateTime.Now.ToString("yyyyMM");

            //txt_IACYC.Text = txt_SALARY_YM.Text;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2IB0500ExcelDown_Click(object sender, EventArgs e)
    {
        CFB2IB0500DAO dao = new CFB2IB0500DAO();
        dao.Excel_YM = txt_Excel_YM.Text.Replace("/","");
        string err = service.checkExcelData(dao);
        if (err != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            return;
        }
        else {            
            //有block
            IWorkbook workbook = service.createExcel(dao, "xlsx");
            Session["workbook_IB050"] = workbook;
            dwnframe.Attributes["src"] = "WFB2IB0500_Qry.aspx?FileType_IB050 = excel";
            Session["FileType_IB050"] = "excel";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        
    }
    protected void WFB2IB0500Exec_Click(object sender, EventArgs e)
    {
        CFB2IB0500DAO dao = new CFB2IB0500DAO();
        try
        {
            dao.SALARY_YM = txt_SALARY_YM.Text.Replace("/","");
            dao.AFT_TOTAL = txt_AFT_INS_TOTAL.Text.Replace(",","");
            //dao.IACYC = txt_IACYC.Text.Replace("/", "");

            DataTable ins_dt = utilities.getParameter("IB", "INS2");
            dao.PAY_KIND = ins_dt.Rows[0]["CODE_VAL1"].ToString();//pay_kind
            ins_dt.Clear();
            //薪資發放資料別
            dao.getSys_cd();
            dao.Lno = "";
            //dao.Lno = dao.SYS_CD + dao.SALARY_YM.Substring(2,4);//20180903  改抓入賬年月
            //dao.Lno = "8881510";//待改 
            dao.TblId = "H15060FFDA1";//未提供檔名 待改

            string err = service.selectMonthData(dao.SALARY_YM);
            if (!err.Equals(""))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }
            else
            {
                //找出TB_S_R_INS2_BILL_RECORD中的週期，如果沒有，表示尚未上傳到介接檔
                
                string iacyc = service.selectIACYC(dao.SALARY_YM);
                
                if (iacyc != "")
                {
                    dao.Lno = dao.SYS_CD + iacyc.Substring(2, 4);//20180903  改抓入賬年月
                    err = service.getLogFlag(dao);
                    //err = "";
                    if (!err.Equals(""))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                        return;
                    }
                }               

                string msg = service.billDataUpload(dao);
                if (!msg.Equals("0"))
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("executeFailMessage", msg);
                }
                else
                {
                    showMessage("executeSuccessMessage");
                }
            }
            
        }
        catch (Exception ex)
        {            
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2IB0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("YM", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            HID_PageRow.Value = ""; //GridView有分頁此段必加     
            if (gv_result.Rows.Count > 0) 
            {
                lb_IaDat.Visible = true;
                txt_IaDat.Visible = true;
            }
            else
            {
                lb_IaDat.Visible = false;
                txt_IaDat.Visible = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "GRID"
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
                getSortDirection("YM", "DESC");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "YM"}; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

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
        gv_result.DataKeyNames = new string[] { "YM" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //判斷button是否disable
            string st = e.Row.Cells[2].Text; ;            
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            int index = Convert.ToInt32(e.Row.RowIndex);
            string txt = e.Row.Cells[1].Text;
            dao.YM = txt;
            dao.TblId = "H15060FFDA1";
            dao.Lno = e.Row.Cells[6].Text;
            Button bt = (Button)e.Row.Cells[4].FindControl("WFB2IB0500BillOut");

            DataTable dt = service.chk_SQLLNO(dao);

            //如二代健保傳票記錄檔.健保投保總額 < 0 ，則DISABLE掉
            if (Convert.ToInt32(e.Row.Cells[7].Text) < 1)
            {
                bt.Enabled = false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    string GCM = dt.Rows[0]["GetChveMrtMk"].ToString();//抓入成功註記
                    string AWM = dt.Rows[0]["AvWgtcmpsMk"].ToString();//可重作註記
                    if (GCM == "")
                    {
                        bt.Enabled = true;
                    }
                    else if (GCM == "N")
                    {
                        bt.Enabled = true;
                        e.Row.Cells[2].Text = "";
                        e.Row.Cells[3].Text = "";
                    }
                    else if (GCM == "Y")
                    {
                        if (AWM == "Y")
                        {
                            bt.Enabled = true;
                        }
                        else
                        {
                            bt.Enabled = false;
                        }
                    }                  
                }
                else
                {
                    bt.Enabled = true;
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
        gv_result.DataKeyNames = new string[] { "YM" }; //設定GridView Key
    }

    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "SentToBill")
        {

            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            string err = "";
            int index = Convert.ToInt32(e.CommandArgument);

            string txt = gv_result.Rows[index].Cells[1].Text;
            //20161005 IB050 轉傳票畫面 需增加入帳日期欄位，傳票檔案的入帳週期=入帳日期的年月
            //               需款週期欄位修改成需款日期 直接指定哪一天付款

            dao.IaDat = txt_IaDat.Text;//入帳日期
            dao.NcrDat = ((TextBox)gv_result.Rows[index].FindControl("txt_NcrDat")).Text;//需款日期
            dao.Lno = gv_result.Rows[index].Cells[6].Text;
            dao.SALARY_YM = txt.Replace("/","");
            dao.YM = txt;                       
            err = utilities.checkDateFormat(dao.NcrDat, "需款日期", false);
            if(err!="")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "$.unblockUI();alert('" + err.Replace("\n", "") + "');", true);
                return;
            }            
            err = utilities.checkDateFormat(dao.IaDat, "入帳日期", false);
            if (err != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "$.unblockUI();alert('" + err.Replace("\n", "") + "');", true);
                return;
            }

           

            //20201216 
            dao.BILL_NO = gv_result.Rows[index].Cells[3].Text.Replace("&nbsp;","") ;
            if (dao.BILL_NO !="" && dao.chek_SAP_DONE() == "E")
                err = "傳票SAP已立帳,不允執行!";

            if (err != "") {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "$.unblockUI();alert('"+err+"');", true);
                return;
            }            

            dao.IACYC = (dao.IaDat).Substring(0, 7);//入帳週期
            //若該月調整後補充保險費 總計 <=0 則不可上傳
            string AFT_INS2_COST = service.selectAFT_INS2_COST(dao.SALARY_YM);
            AFT_INS2_COST = AFT_INS2_COST == "" ? "0" : AFT_INS2_COST;
            if (Convert.ToInt32(AFT_INS2_COST) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "$.unblockUI();alert('調整後補充保險費小於0，不可上傳');", true);
                return;
            }

            //轉出傳票            
            err = service.transToBill(dao);
            
            if (!err.Equals("0"))
            {
                err = err.Replace("\r\n", "");
                err = err.Replace("'", "");                
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + err + "')", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + err + "');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "$.unblockUI();alert('傳票轉出完成');", true);
                //showMessage("DC080SuccessMessage");

            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

           
        }
        
    }
    #endregion

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_IB050"] != null && Session["FileType_IB050"].ToString() != "")
            {
                string fileType = Session["FileType_IB050"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_IB050"];
                    Session["FileType_IB050"] = "";
                    Session["workbook_IB050"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2IB050.xlsx");

                }               

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


}