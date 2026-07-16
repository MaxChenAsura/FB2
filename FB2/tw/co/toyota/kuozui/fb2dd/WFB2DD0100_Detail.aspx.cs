using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dd_WFB2DD0100_Detail : BasePage
{
    CFB2DD0100BO service = new CFB2DD0100BO();
    string EMP_ID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = true;
        //第一次進入頁面執行
        EMP_ID = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {                        
            //initial value
            hid_EMP_ID.Value = EMP_ID;
            init(EMP_ID);
            //getGridView();   
            getGridView("APPLICATION_NO", 0, 10);
            this.gv_result.ShowFooter = false;

            dailyPay.Visible = false;            
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        
        
      
    }

    public void init(string EMP_ID)
    {
        if (EMP_ID != "")
        {
            DataTable dt = service.getEmp_data(EMP_ID);
            DataTable dt1 = service.getCar_data(EMP_ID);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_ID.Text = EMP_ID;
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                txt_EMP_CHG_CD.Text = dt.Rows[0]["EMP_CHG_CD"].ToString();
                txt_EMP_CD.Text = dt.Rows[0]["EMP_CD"].ToString();
                txt_JOIN_DT.Text = dt.Rows[0]["JOIN_DT"].ToString().Replace("-","/");
                txt_WORK_SHIFT_CD.Text = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_CD"].ToString();
                txt_CONTACT_TEL.Text = dt.Rows[0]["CONTACT_TEL"].ToString();
                txt_MOBILE_TEL_1.Text = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_CONTACT_ADDR.Text = dt.Rows[0]["CONTACT_ADDR"].ToString();
                txt_REGISTER_ADDR.Text = dt.Rows[0]["REGISTER_ADDR"].ToString();
                hid_PLANT_CD.Value = dt.Rows[0]["PLANT_CD"].ToString();

                //申請交通津貼不一定有停車位等資料
                if (dt1.Rows.Count > 0)
                {
                    txt_CAR_NO.Text = dt1.Rows[0]["CAR_NO"].ToString();
                    txt_PARKING_TOOL.Text = dt1.Rows[0]["PARKING_TOOL"].ToString();
                }
                
            }
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
                //getSortDirection("APPLICATION_NO DESC,START_DT", "DESC");
                getSortDirection("START_DT DESC,APPLICATION_NO ", "DESC");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "APPLICATION_NO" }; //設定GridView Key
            gv_result.DataBind();

            //if (gv_result.Rows.Count == 0)
            //{
            //    showMessage("QryNotFoundMessage");
            //}

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
        gv_result.DataKeyNames = new string[] { "APPLICATION_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[18].Visible = false;
            e.Row.Cells[19].Visible = false;
            e.Row.Cells[20].Visible = false;
            e.Row.Cells[21].Visible = false;
            e.Row.Cells[22].Visible = false;
            e.Row.Cells[23].Visible = false;
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
        gv_result.DataKeyNames = new string[] { "APPLICATION_NO" }; //設定GridView Key
    }

    protected void WFB2DD0101Add_Click(object sender, EventArgs e)
    {
        string empty = "";
        //帶入預設值
        string day = Convert.ToString(DateTime.Now.Day).Length == 2 ? Convert.ToString(DateTime.Now.Day) : "0" + Convert.ToString(DateTime.Now.Day);
        string mon = Convert.ToString(DateTime.Now.Month).Length == 2 ? Convert.ToString(DateTime.Now.Month) : "0" + Convert.ToString(DateTime.Now.Month);
        string word = Convert.ToString(DateTime.Now.Year) +"/"+ mon+"/"+day;
        txt_START_DT.Text = word;
        txt_ADDRESS.Text = txt_CONTACT_ADDR.Text;
        rb_IS_CALCULATE.SelectedValue = "1";
        rb_IS_CANCEL.SelectedValue = "N";
        create_CHG_REASON(empty);
        createPLANT_CD(hid_PLANT_CD.Value);
        createAREA_CD(empty);
        createTRANSPORT_CD(empty);
        ddl_LINE_CD.Items.Add(new ListItem("", "-1"));

        //隱藏與顯示按鈕
        WFB2DD0101Add.Visible = false;
        WFB2DD0101EditInsert.Visible = false;
        WFB2DD0101Delete.Visible = false;
        WFB2DD0101Save.Visible = true;
        WFB2DD0101Cancel.Visible = true;
        dailyPay.Visible = true;
        btn_back1.Visible = false;
    }
    //異動原因
    private void create_CHG_REASON(string dValue)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("CHG_REASON", "", "");
            ddl_CHG_REASON.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CHG_REASON.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                   
                }
                if (dValue != "")
                {
                    ddl_CHG_REASON.SelectedValue = dValue;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createPLANT_CD(string dValue)
    {
        try
        {           
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ALLOWANCE_PLANT_CD", "", "");
            ddl_FACTORY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_FACTORY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                                       
                }
                if (dValue != "")
                {
                    ddl_FACTORY_CD.SelectedValue = dValue;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createAREA_CD(string dValue)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("AREA_CD", "", "");
            ddl_AREA_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_AREA_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                   
                }
                if (dValue != "")
                {
                    ddl_AREA_CD.SelectedValue = dValue;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createTRANSPORT_CD(string dValue)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCOM("DD","TRANSPORT_CD", "", "","");
            ddl_TRANSPORT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRANSPORT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));                   
                }
                if (dValue != "")
                {
                    ddl_TRANSPORT_CD.SelectedValue = dValue;
                }
            }
            string code_val1 = service.getCode_Val(ddl_TRANSPORT_CD.SelectedValue);
            hid_TRANSPORT_MONEY.Value = code_val1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //交通工具選擇後查詢路線
    protected void ddl_TRANSPORT_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("DD", "LINE_CD", "", "");

            ddl_LINE_CD.Items.Clear();
            if (ddl_TRANSPORT_CD.SelectedValue == "03" || ddl_TRANSPORT_CD.SelectedValue == "05" || ddl_TRANSPORT_CD.SelectedValue == "06"
                || ddl_TRANSPORT_CD.SelectedValue == "11" || ddl_TRANSPORT_CD.SelectedValue == "12" || ddl_TRANSPORT_CD.SelectedValue == "13")
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_LINE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
            else
            {
                ddl_LINE_CD.Items.Add(new ListItem("", "-1"));
            }

            //放入預設站別 CODE_VAL1=01
            DataTable dt1 = new DataTable();
            dt1 = dao.getCommCode("DD", "STATION_CD", "01", "");

            ddl_STATION_CD.Items.Clear();
            if (ddl_TRANSPORT_CD.SelectedValue == "03" || ddl_TRANSPORT_CD.SelectedValue == "05" || ddl_TRANSPORT_CD.SelectedValue == "06"
                || ddl_TRANSPORT_CD.SelectedValue == "11" || ddl_TRANSPORT_CD.SelectedValue == "12" || ddl_TRANSPORT_CD.SelectedValue == "13")
            {
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ddl_STATION_CD.Items.Add(new ListItem(dt1.Rows[i]["sub_desc"].ToString(), dt1.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
            else
            {
                ddl_STATION_CD.Items.Add(new ListItem("", "-1"));
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //路線選擇後查詢交通車站別
    protected void ddl_LINE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            dt = dao.getCommCode("DD", "STATION_CD", ddl_LINE_CD.SelectedValue, "");
            ddl_STATION_CD.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_STATION_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            else
            {
                ddl_STATION_CD.Items.Add(new ListItem("", "-1"));
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DD0101Save_Click(object sender, EventArgs e)
    {
         string errormessage = "";         
         string msg = "";
         string trans = ""; 
        //新增作業

        //一般欄位檢核
         CFB2DD0100DAO dao = new CFB2DD0100DAO();
         DataTable dt = new DataTable();
         DataTable dt1 = new DataTable();
         if (ddl_TRANSPORT_CD.SelectedValue == "01" || ddl_TRANSPORT_CD.SelectedValue == "05" || ddl_TRANSPORT_CD.SelectedValue == "07"
             || ddl_TRANSPORT_CD.SelectedValue == "09" || ddl_TRANSPORT_CD.SelectedValue == "12")
         {
             trans = "01";
         }
         else if (ddl_TRANSPORT_CD.SelectedValue == "02" || ddl_TRANSPORT_CD.SelectedValue == "06" || ddl_TRANSPORT_CD.SelectedValue == "08"
           || ddl_TRANSPORT_CD.SelectedValue == "10" || ddl_TRANSPORT_CD.SelectedValue == "13")
         {
             trans = "02";
         }
         

         dt = dao.getKM("DD", "ALLOWANCE_LTD_CD", trans, "", "");  
         if (dt.Rows.Count > 0)
         {
             dao.CL_KM = dt.Rows[0]["CODE_VAL1"].ToString();//中壢  
             dao.KN_KM = dt.Rows[0]["CODE_VAL2"].ToString();//觀音 
         }
         dt1 = dao.getKM("DD", "ALLOWANCE_LTD_CD", "03", "", "");  
         if (dt1.Rows.Count > 0)
         {
             dao.CL_FR = dt1.Rows[0]["CODE_VAL1"].ToString();//中壢  
             dao.KN_FR = dt1.Rows[0]["CODE_VAL2"].ToString();//觀音 
         }        

         bool b = CheckPara(dao);
         if (b)
         {             
             //是否能申請交通津貼
             string err = service.isAllow(txt_EMP_ID.Text);
             if (!err.Equals(""))  
             {
                 ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                 WFB2DD0101Add.Visible = true;
                 WFB2DD0101EditInsert.Visible = true;
                 WFB2DD0101Delete.Visible = true;
                 WFB2DD0101Save.Visible = false;
                 WFB2DD0101Cancel.Visible = false;
                 dailyPay.Visible = false;
                 return;
             }
             else 
             { 
                //KEEP 畫面參數
                 dao.D_EMP_ID = txt_EMP_ID.Text;
                 dao.D_START_DT = txt_START_DT.Text +" "+ Convert.ToString(DateTime.Now.Hour) +":"+ Convert.ToString(DateTime.Now.Minute) +":"+ Convert.ToString(DateTime.Now.Second);
                 dao.D_FACTORY_CD = ddl_FACTORY_CD.SelectedValue == "-1" ? "" : ddl_FACTORY_CD.SelectedValue;
                 dao.D_AREA_CD = ddl_AREA_CD.SelectedValue == "-1" ? "" : ddl_AREA_CD.SelectedValue;
                 dao.D_TRANSPORT_CD = ddl_TRANSPORT_CD.SelectedValue == "-1" ? "" : ddl_TRANSPORT_CD.SelectedValue;
                 dao.D_LINE_CD = ddl_LINE_CD.SelectedValue == "-1" ? "" : ddl_LINE_CD.SelectedValue;
                 dao.D_STATION_CD = ddl_STATION_CD.SelectedValue == "-1" ? "" : ddl_STATION_CD.SelectedValue;
                 dao.D_KILOMETER_AMOUNT = txt_KILOMETER_AMOUNT.Text == "" ? "0" : txt_KILOMETER_AMOUNT.Text;
                 dao.D_FARE_PRICE = txt_FARE_PRICE.Text == "" ? "0" : txt_FARE_PRICE.Text;
                 dao.D_SINGLE_TRIP = cb_SINGLE_TRIP.Checked == true ? "Y" : "N";
                 dao.D_REMARK = txt_REMARK.Text;
                 dao.D_ADDRESS = utilities.toWide(txt_ADDRESS.Text);
                 dao.D_IS_CALCULATE = rb_IS_CALCULATE.SelectedValue;
                 dao.D_IS_CANCEL = rb_IS_CANCEL.SelectedValue;
                 dao.D_CHG_REASON = ddl_CHG_REASON.SelectedValue == "-1" ? "" : ddl_CHG_REASON.SelectedValue;
                 dao.PLANT_CD = txt_PLANT_CD.Text.Substring(0,1);//編號用的廠區別
                 dao.CREATED_BY = SessionHandle.Current.emp_id;
                 dao.UPDATED_BY = SessionHandle.Current.emp_id;
                 dao.FUNC_ID = "FB2DD010";

                 msg = service.insertTRANS_ALLOWANCE_D(dao);
                 if (msg != "0")
                 {
                     showMessage("addFailMessage", msg);
                     return;
                 }
                 else
                 {
                     showMessage("addSuccessMessage");
                 }
             }    
            
           }
           else
           {
               return;
           }

         //畫面條件欄位拿掉
         txt_START_DT.Text = "";
         ddl_FACTORY_CD.Items.Clear();
         ddl_AREA_CD.Items.Clear();
         ddl_TRANSPORT_CD.Items.Clear();
         ddl_LINE_CD.Items.Clear();
         ddl_STATION_CD.Items.Clear();
         txt_KILOMETER_AMOUNT.Text = "";
         txt_FARE_PRICE.Text = "";
         cb_SINGLE_TRIP.Checked = false;
         txt_REMARK.Text = "";
         txt_ADDRESS.Text = "";       
         rb_IS_CALCULATE.SelectedValue = "";
         rb_IS_CANCEL.SelectedValue = "";
         ddl_CHG_REASON.Items.Clear();

         ViewState["NewPageIndex"] = gv_result.PageIndex;
         if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
             gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
         else
             gv_result.PageSize = 10;

         gv_result.DataSourceID = "ods1";
         gv_result.DataKeyNames = new string[] { "APPLICATION_NO" };
         gv_result.EditIndex = -1;
         gv_result.ShowFooter = false;       

         //隱藏與顯示按鈕
         WFB2DD0101Add.Visible = true;
         WFB2DD0101EditInsert.Visible = true;
         WFB2DD0101Delete.Visible = true;
         WFB2DD0101Save.Visible = false;
         WFB2DD0101Cancel.Visible = false;
         dailyPay.Visible = false;
         txt_DAILY_PAY.Text = "";
         btn_back1.Visible = true;
    }


    private bool CheckPara(CFB2DD0100DAO dao)
    {        
        string errormessage = "";
        if (txt_START_DT.Text == "")
        {
            errormessage += "生效日不可空白\\n";
        }
        if (txt_ADDRESS.Text == "")
        {
            errormessage += "交通津貼位址不可空白\\n";
        }

        //輸入公里數
        if (ddl_TRANSPORT_CD.SelectedValue == "03" || ddl_TRANSPORT_CD.SelectedValue == "04"
            || ddl_TRANSPORT_CD.SelectedValue == "11" || ddl_TRANSPORT_CD.SelectedValue == "14")
        {
            if (!txt_KILOMETER_AMOUNT.Text.Equals("") && txt_KILOMETER_AMOUNT.Text != "0")
            {
                errormessage += "交通工具非選擇到汽、機車時，不可輸入公里數\\n";
            }
        }
        else { 
            //必須輸入公里數
            //if (txt_KILOMETER_AMOUNT.Text.Equals("") || txt_KILOMETER_AMOUNT.Text.Equals("0"))
            if (txt_KILOMETER_AMOUNT.Text.Equals(""))
            {
                errormessage += "交通工具選擇到汽、機車時，公里數不可為空白\\n";
            }
            else {
                if (ddl_FACTORY_CD.SelectedValue == "1")
                {//中壢
                    //dao.CL_KM = dao.CL_KM.Substring(0, dao.CL_KM.Length - 1);
                    if (Convert.ToInt32(txt_KILOMETER_AMOUNT.Text) > Convert.ToInt32(dao.CL_KM))
                    {
                        errormessage += "輸入的公里數超出上限" + dao.CL_KM + "\\n";
                    }
                }
                if (ddl_FACTORY_CD.SelectedValue == "2")//觀音
                {
                    //dao.KN_KM = dao.KN_KM.Substring(0, dao.KN_KM.Length - 1);
                    if (Convert.ToInt32(txt_KILOMETER_AMOUNT.Text) > Convert.ToInt32(dao.KN_KM))
                    {
                        errormessage += "輸入的公里數超出上限" + dao.KN_KM + "\\n";
                    }
                }
            }
        }

        //輸入票價
        if (ddl_TRANSPORT_CD.SelectedValue == "01" || ddl_TRANSPORT_CD.SelectedValue == "02"
            || ddl_TRANSPORT_CD.SelectedValue == "03" || ddl_TRANSPORT_CD.SelectedValue == "05"
            || ddl_TRANSPORT_CD.SelectedValue == "06" || ddl_TRANSPORT_CD.SelectedValue == "14")
        {
            if (!txt_FARE_PRICE.Text.Equals("") && txt_FARE_PRICE.Text != "0")
            {
                errormessage += "交通工具非選擇到大眾工具時，票價不可輸入\\n";
            }
        }
        else {
            //必須輸入票價
            //if (txt_FARE_PRICE.Text.Equals("") || txt_FARE_PRICE.Text.Equals("0"))
            if (txt_FARE_PRICE.Text.Equals(""))
            {
                errormessage += "交通工具選擇到大眾工具時，票價不可為空白\\n";
            }
            else {
                if (ddl_FACTORY_CD.SelectedValue == "1")//中壢
                {
                    if (Convert.ToInt32(txt_FARE_PRICE.Text) > Convert.ToInt32(dao.CL_FR))
                    {
                        errormessage += "票價上限為" + dao.CL_FR + "元，輸入的票價不可大於該廠區的票價上限值\\n";
                    }
                }
                if (ddl_FACTORY_CD.SelectedValue == "2")//觀音
                {
                    if (Convert.ToInt32(txt_FARE_PRICE.Text) > Convert.ToInt32(dao.KN_FR))
                    {
                        errormessage += "票價上限為" + dao.KN_FR + "元，輸入的票價不可大於該廠區的票價上限值\\n";
                    }
                }
            }
        }

        //輸入路線
        if (ddl_TRANSPORT_CD.SelectedValue == "03" || ddl_TRANSPORT_CD.SelectedValue == "05"
            || ddl_TRANSPORT_CD.SelectedValue == "06" || ddl_TRANSPORT_CD.SelectedValue == "11"
            || ddl_TRANSPORT_CD.SelectedValue == "12" || ddl_TRANSPORT_CD.SelectedValue == "13")
        {
            //必須輸入路線站別
            if (ddl_LINE_CD.SelectedValue == "-1")
            {
                errormessage += "交通工具選擇到交通車時，路線不可空白\\n";
            }
            if (ddl_STATION_CD.SelectedValue == "-1")
            {
                //errormessage += "交通工具選擇到交通車時，站別不可空白\\n";
            }
        }
        else {
            if (ddl_LINE_CD.SelectedValue != "-1")
            {
                errormessage += "交通工具未選擇到交通車，路線不可輸入\\n";
            }
            if (ddl_STATION_CD.SelectedValue != "-1")
            {
                //errormessage += "交通工具未選擇到交通車，站別不可輸入\\n";
            }
        }



        //機車汽車
        //if (ddl_TRANSPORT_CD.SelectedValue == "01" || ddl_TRANSPORT_CD.SelectedValue == "02")
        //{
        //    if (txt_FARE_PRICE.Text != "")
        //    {
        //        errormessage += "交通工具選擇汽、機車時，票價不可輸入\\n";
        //    }
        //    if (txt_KILOMETER_AMOUNT.Text == "")
        //    {
        //        errormessage += "交通工具選擇汽、機車時，公里數不可為空白\\n";
        //    }
        //    else {
        //        if (ddl_FACTORY_CD.SelectedValue == "1")
        //        {//中壢
        //            CL = CL.Substring(0,CL.Length-1);
        //            if (Convert.ToInt32(txt_KILOMETER_AMOUNT.Text) > Convert.ToInt32(CL))
        //            {
        //                errormessage += "輸入的公里數超出上限" + CL + "\\n";
        //            }
        //        }
        //        if (ddl_FACTORY_CD.SelectedValue == "2")//觀音
        //        {
        //            KN = KN.Substring(0,KN.Length-1);
        //            if (Convert.ToInt32(txt_KILOMETER_AMOUNT.Text) > Convert.ToInt32(KN))
        //            {
        //                errormessage += "輸入的公里數超出上限" + KN + "\\n";
        //            }
        //        }
        //    }
        //    if (ddl_LINE_CD.SelectedValue != "-1")
        //    {
        //        errormessage += "交通工具選擇汽、機車時，路線不可輸入\\n";
        //    }
        //    if (ddl_STATION_CD.SelectedValue != "-1")
        //    {
        //        errormessage += "交通工具選擇汽、機車時，站別不可輸入\\n";
        //    }
            
        //}

        ////交通車
        //if (ddl_TRANSPORT_CD.SelectedValue == "03")
        //{
        //    if (txt_KILOMETER_AMOUNT.Text != "")
        //    {
        //        errormessage += "交通工具選擇交通車時，公里數不可輸入\\n";
        //    }
        //    if (ddl_LINE_CD.SelectedValue == "")
        //    {
        //        errormessage += "交通工具選擇交通車時，路線不可空白\\n";
        //    }
        //    if (txt_FARE_PRICE.Text !="")
        //    {
        //        if (ddl_FACTORY_CD.SelectedValue == "1")//中壢
        //        {
        //            if (Convert.ToInt32(txt_FARE_PRICE.Text) > Convert.ToInt32(CL))
        //            {
        //                errormessage += "票價上限為" + CL + "元，輸入的票價不可大於該廠區的票價上限值\\n";
        //            }
        //        }
        //        if (ddl_FACTORY_CD.SelectedValue == "2")//觀音
        //        {
        //            if (Convert.ToInt32(txt_FARE_PRICE.Text) > Convert.ToInt32(KN))
        //            {
        //                errormessage += "票價上限為" + KN + "元，輸入的票價不可大於該廠區的票價上限值\\n";
        //            }
        //        }
        //   }
            
        //}
        ////
        //if (ddl_TRANSPORT_CD.SelectedValue == "04")
        //{
        //    if (txt_KILOMETER_AMOUNT.Text != "")
        //    {
        //        errormessage += "交通工具選擇其他時，公里數不可輸入\\n";
        //    }
        //    if (ddl_LINE_CD.SelectedValue != "")
        //    {
        //        errormessage += "交通工具選擇其他時，路線不可輸入\\n";
        //    }
        //}

        if (rb_IS_CANCEL.SelectedValue == "Y" && rb_IS_CALCULATE.SelectedValue == "1")
        {
            errormessage += "此工號的交通津貼設定為永久註銷，請將是否計算交通津貼改為否，再繼續進行儲存作業\\n";
        }


        if (errormessage.Equals(""))
            return true;
        else
        {            
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + errormessage + "');", true);
            return false;
        }
    }


    protected void WFB2DD0101EditInsert_Click(object sender, EventArgs e)
    {
        CFB2DD0100DAO dao = new CFB2DD0100DAO();
        try
        {
            //給FLAG
            //hid_Valid_Flag.Value = "N";

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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('複製新增請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('複製新增請選擇一筆資料')", true);
                return;
            }
            else
            {               
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {                        
                        //txt_START_DT.Text = gv_result.Rows[i].Cells[3].Text.Replace("-","/");
                        string day = Convert.ToString(DateTime.Now.Day).Length == 2 ? Convert.ToString(DateTime.Now.Day) : "0" + Convert.ToString(DateTime.Now.Day);
                        string mon = Convert.ToString(DateTime.Now.Month).Length == 2 ? Convert.ToString(DateTime.Now.Month) : "0" + Convert.ToString(DateTime.Now.Month);
                        string word = Convert.ToString(DateTime.Now.Year) + "/" + mon + "/" + day;
                        txt_START_DT.Text = word;
                        
                        //txt_IFLOW_NO.Text = gv_result.Rows[i].Cells[4].Text;
                        //ddl_CHG_REASON.SelectedValue = gv_result.Rows[i].Cells[19].Text.Replace("&nbsp;","");
                        rb_IS_CANCEL.SelectedValue = gv_result.Rows[i].Cells[7].Text.Replace("&nbsp;", "")=="是" ? "Y" : "N";
                        rb_IS_CALCULATE.SelectedValue = gv_result.Rows[i].Cells[8].Text.Replace("&nbsp;", "") == "是" ? "1" : "0";
                        txt_KILOMETER_AMOUNT.Text = gv_result.Rows[i].Cells[13].Text.Replace("&nbsp;", "") == "0" ? "" : gv_result.Rows[i].Cells[13].Text.Replace("&nbsp;", "");
                        txt_FARE_PRICE.Text = gv_result.Rows[i].Cells[14].Text.Replace("&nbsp;", "") == "0" ? "" : gv_result.Rows[i].Cells[14].Text.Replace("&nbsp;", "");
                        cb_SINGLE_TRIP.Checked = gv_result.Rows[i].Cells[15].Text=="Y" ? true : false ;
                        txt_ADDRESS.Text = gv_result.Rows[i].Cells[17].Text.Replace("&nbsp;", "");
                        txt_REMARK.Text = gv_result.Rows[i].Cells[18].Text.Replace("&nbsp;", "");                    


                        create_CHG_REASON(gv_result.Rows[i].Cells[20].Text.Replace("&nbsp;", ""));
                        createPLANT_CD(gv_result.Rows[i].Cells[21].Text.Replace("&nbsp;", ""));
                        createAREA_CD(gv_result.Rows[i].Cells[22].Text.Replace("&nbsp;", ""));
                        createTRANSPORT_CD(gv_result.Rows[i].Cells[23].Text.Replace("&nbsp;", ""));

                        
                        //如果路線有預設值
                        if (gv_result.Rows[i].Cells[24].Text != "-1" && gv_result.Rows[i].Cells[24].Text != "&nbsp;")
                        {
                            DataTable dt = new DataTable();
                            dt = dao.getCOM("DD", "LINE_CD", "", "", "");
                            if (dt.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    ddl_LINE_CD.Items.Add(new ListItem(dt.Rows[j]["sub_desc"].ToString(), dt.Rows[j]["sub_cd"].ToString()));
                                    
                                }
                                if (gv_result.Rows[i].Cells[24].Text != "-1" && gv_result.Rows[i].Cells[24].Text != "&nbsp;")
                                {
                                    ddl_LINE_CD.SelectedValue = gv_result.Rows[i].Cells[24].Text.Replace("&nbsp;", "");
                                }
                                
                            }
                            //放入預設站別 CODE_VAL1=ddl_LINE_CD.SelectedValue
                            DataTable dt1 = new DataTable();
                            dt1 = dao.getCommCode("DD", "STATION_CD", gv_result.Rows[i].Cells[24].Text.Replace("&nbsp;", ""), "");

                            ddl_STATION_CD.Items.Clear();
                            if (dt1.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt1.Rows.Count; k++)
                                {
                                    ddl_STATION_CD.Items.Add(new ListItem(dt1.Rows[k]["sub_desc"].ToString(), dt1.Rows[k]["sub_cd"].ToString()));
                                   
                                }
                                ddl_STATION_CD.SelectedValue = gv_result.Rows[i].Cells[19].Text.Replace("&nbsp;", "");
                            }
                            else
                            {
                                ddl_STATION_CD.Items.Add(new ListItem("", "-1"));
                                ddl_STATION_CD.SelectedValue = "-1";
                            }
                        }
                        else
                        {
                            ddl_LINE_CD.Items.Add(new ListItem("", "-1"));
                            ddl_STATION_CD.Items.Add(new ListItem("", "-1"));                           
                        }                  


                    }
                }          

            }

            //隱藏與顯示按鈕
            WFB2DD0101Add.Visible = false;
            WFB2DD0101EditInsert.Visible = false;
            WFB2DD0101Delete.Visible = false;
            WFB2DD0101Save.Visible = true;
            WFB2DD0101Cancel.Visible = true;
            dailyPay.Visible = true;
            btn_back1.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    protected void WFB2DD0101Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();            
            string APPLICATION_NO = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {                
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    //檢查是否有勾選，有勾則加入該列的資料key
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                    }
                }                
                
            }

            
            
            //if (((CheckBox)gv_result.Rows[0].FindControl("cb_check")).Checked == false)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除只能選擇最新一筆資料，請重新選擇資料')", true);
            //    return;
            //}
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除只能選擇最新一筆資料，請重新選擇資料')", true);
                return;
            }
            else if (emp_id.Count() == 1)
            {
                string st="";
                APPLICATION_NO = emp_id[0];
                //判斷是否為最新資料
                st = service.checkFirst(APPLICATION_NO, txt_EMP_ID.Text.Trim());
                if (st == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除只能選擇最新一筆資料，請重新選擇資料')", true);
                    return;
                }
                else
                {
                    string msg = "", flag = "";
                    APPLICATION_NO = gv_result.DataKeys[0].Value.ToString();
                    //判斷是否為最新資料
                    if (this.gv_result.Rows.Count == 1)
                    {
                        flag = "1";
                        //刪除明細檔第一筆
                        msg = service.deleteData(txt_EMP_ID.Text, APPLICATION_NO, flag);
                    }
                    else
                    {
                        //更新第二筆資料.生效日迄=9999/12/31
                        //更新 交通津貼主檔
                        //刪除 第一筆資料
                        msg = service.deleteData(txt_EMP_ID.Text, APPLICATION_NO, flag);
                    }


                    if (msg != "0")
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                    else
                        showMessage("deleteSuccessMessage");

                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "APPLICATION_NO" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false; 
                }                
            }
          
           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DD0101Cancel_Click(object sender, EventArgs e)
    {
        //清除畫面欄位值
        //畫面條件欄位拿掉
        txt_START_DT.Text = "";
        ddl_FACTORY_CD.Items.Clear();
        ddl_AREA_CD.Items.Clear();
        ddl_TRANSPORT_CD.Items.Clear();
        ddl_LINE_CD.Items.Clear();
        ddl_STATION_CD.Items.Clear();
        txt_KILOMETER_AMOUNT.Text = "";
        txt_FARE_PRICE.Text = "";
        cb_SINGLE_TRIP.Checked = false;
        txt_REMARK.Text = "";
        txt_ADDRESS.Text = "";
       
        rb_IS_CALCULATE.SelectedValue = "";
        rb_IS_CANCEL.SelectedValue = "";
        ddl_CHG_REASON.Items.Clear();
        rb_IS_CALCULATE.SelectedValue = "";


        ViewState["NewPageIndex"] = gv_result.PageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "APPLICATION_NO" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;      


        //隱藏與顯示按鈕
        WFB2DD0101Add.Visible = true;
        WFB2DD0101EditInsert.Visible = true;
        WFB2DD0101Delete.Visible = true;
        WFB2DD0101Save.Visible = false;
        WFB2DD0101Cancel.Visible = false;
        dailyPay.Visible = false;
        txt_DAILY_PAY.Text = "";
        btn_back1.Visible = true;
    }
    protected void btn_back1_Click(object sender, EventArgs e)
    {
        Session["DD010_Is_Search"] = "Y";
        Response.Redirect("WFB2DD0100_Qry.aspx");
    }
}