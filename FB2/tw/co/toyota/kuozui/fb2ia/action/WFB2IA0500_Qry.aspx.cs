using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2IA0500_Qry : BasePage
{
    //Service 物件
    private CFB2IA0500BO service = new CFB2IA0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //建立團保對象別清單
            createTARGET_TYPE();
            //建立團保險種清單
            createGINS_KIND();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //建立團保對象別清單
    private void createTARGET_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("IA","TARGET_TYPE", "", "");
            ddl_TARGET_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TARGET_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //建立團保險種清單
    private void createGINS_KIND()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("GINS_KIND", "", "");
            ddl_GINS_KIND.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GINS_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("TARGET_TYPE,GINS_KIND,GINS_ITEM,PERSON_QTY_S");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "TARGET_TYPE", "GINS_KIND", "GINS_ITEM", "PERSON_QTY_S" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2IA0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("TARGET_TYPE,GINS_KIND,GINS_ITEM,PERSON_QTY_S", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("TARGET_TYPE,GINS_KIND,GINS_ITEM,PERSON_QTY_S", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2IA0500Add.Visible = true;
                WFB2IA0500Edit.Visible = true;
                WFB2IA0500Delete.Visible = true;
            }
            else
            {
                WFB2IA0500Edit.Visible = false;
                WFB2IA0500Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2IA0500Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2IA0500Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("TARGET_TYPE,GINS_KIND,GINS_ITEM,PERSON_QTY_S", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("TARGET_TYPE,GINS_KIND,GINS_ITEM,PERSON_QTY_S", 0, 10);

            WFB2IA0500Save.Visible = true;
            WFB2IA0500Cancel.Visible = true;

            WFB2IA0500Add.Visible = false;
            WFB2IA0500Edit.Visible = false;
            WFB2IA0500Delete.Visible = false;
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

    //刪除按鈕事件
    protected void WFB2IA0500Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string>> target_type =
                new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    target_type.Add(
                        new Tuple<string, string, string, string>(
                            gv_result.DataKeys[i].Values["TARGET_TYPE"].ToString().Split('-')[0],
                            gv_result.DataKeys[i].Values["GINS_KIND"].ToString(),
                            gv_result.DataKeys[i].Values["GINS_ITEM"].ToString(),
                            gv_result.DataKeys[i].Values["PERSON_QTY_S"].ToString()));

                }
            }

            string msg = service.deleteGROUP_KIND(target_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改按鈕事件
    protected void WFB2IA0500Edit_Click(object sender, EventArgs e)
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
            WFB2IA0500Search.Visible = false;
            btn_clear.Visible = false;

            WFB2IA0500Save.Visible = true;
            WFB2IA0500Cancel.Visible = true;

            WFB2IA0500Add.Visible = false;
            WFB2IA0500Edit.Visible = false;
            WFB2IA0500Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕
    protected void WFB2IA0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                DropDownList TARGET_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_TARGET_TYPE");
                DropDownList GINS_KIND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_GINS_KIND");
                TextBox GINS_ITEM = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_GINS_ITEM");
                TextBox GINS_ITEM_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_GINS_ITEM_NAME");
                TextBox AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AMT");
                TextBox PERSON_QTY_S = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PERSON_QTY_S");
                TextBox PERSON_QTY_E = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PERSON_QTY_E");
                CheckBox HOUSE_YN = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_NEW_HOUSE_YN");
                TextBox EMP_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_RATE");
                TextBox CMP_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CMP_RATE");
                TextBox UNION_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_UNION_RATE");

                if (PERSON_QTY_E.Text == "")
                    PERSON_QTY_E.Text = "1";
                if (EMP_RATE.Text == "")
                    EMP_RATE.Text = "0";
                if (CMP_RATE.Text == "")
                    CMP_RATE.Text = "0";
                if (UNION_RATE.Text == "")
                    UNION_RATE.Text = "0";

                if (!check_data(Convert.ToInt32(PERSON_QTY_S.Text), Convert.ToInt32(PERSON_QTY_E.Text),
                     Convert.ToInt32(EMP_RATE.Text), Convert.ToInt32(CMP_RATE.Text), Convert.ToInt32(UNION_RATE.Text)))
                    return;

                CFB2IA0500DAO wfb2ia = new CFB2IA0500DAO();
                wfb2ia.TARGET_TYPE = TARGET_TYPE.SelectedValue;
                wfb2ia.GINS_KIND = GINS_KIND.SelectedValue;
                wfb2ia.GINS_ITEM = GINS_ITEM.Text.ToUpper();
                wfb2ia.GINS_ITEM_NAME = GINS_ITEM_NAME.Text;
                wfb2ia.AMT = AMT.Text;
                wfb2ia.PERSON_QTY_S = PERSON_QTY_S.Text;
                wfb2ia.PERSON_QTY_E = PERSON_QTY_E.Text;
                if (HOUSE_YN.Checked)
                    wfb2ia.HOUSE_YN = "Y";
                else
                    wfb2ia.HOUSE_YN = "N";
                wfb2ia.EMP_RATE = EMP_RATE.Text;
                wfb2ia.CMP_RATE = CMP_RATE.Text;
                wfb2ia.UNION_RATE = UNION_RATE.Text;
                wfb2ia.FEES_YN = "N"; //新增(保費計算否)
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA050";

                string msg = service.addGROUP_KIND(wfb2ia);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
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
                    DropDownList TARGET_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_TARGET_TYPE");
                    DropDownList GINS_KIND = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_GINS_KIND");
                    TextBox GINS_ITEM = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_GINS_ITEM");
                    TextBox GINS_ITEM_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_GINS_ITEM_NAME");
                    TextBox AMT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AMT");
                    TextBox PERSON_QTY_S = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PERSON_QTY_S");
                    TextBox PERSON_QTY_E = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PERSON_QTY_E");
                    CheckBox HOUSE_YN = (CheckBox)gv_result.FooterRow.FindControl("cb_NEW_HOUSE_YN");
                    TextBox EMP_RATE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_RATE");
                    TextBox CMP_RATE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CMP_RATE");
                    TextBox UNION_RATE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_UNION_RATE");

                    if (PERSON_QTY_E.Text == "")
                        PERSON_QTY_E.Text = "1";
                    if (EMP_RATE.Text == "")
                        EMP_RATE.Text = "0";
                    if (CMP_RATE.Text == "")
                        CMP_RATE.Text = "0";
                    if (UNION_RATE.Text == "")
                        UNION_RATE.Text = "0";

                    if (!check_data(Convert.ToInt32(PERSON_QTY_S.Text), Convert.ToInt32(PERSON_QTY_E.Text),
                         Convert.ToInt32(EMP_RATE.Text), Convert.ToInt32(CMP_RATE.Text), Convert.ToInt32(UNION_RATE.Text)))
                        return;

                    CFB2IA0500DAO wfb2ia = new CFB2IA0500DAO();
                    wfb2ia.TARGET_TYPE = TARGET_TYPE.SelectedValue;
                    wfb2ia.GINS_KIND = GINS_KIND.SelectedValue;
                    wfb2ia.GINS_ITEM = GINS_ITEM.Text.ToUpper();
                    wfb2ia.GINS_ITEM_NAME = GINS_ITEM_NAME.Text;
                    wfb2ia.AMT = AMT.Text;
                    wfb2ia.PERSON_QTY_S = PERSON_QTY_S.Text;
                    wfb2ia.PERSON_QTY_E = PERSON_QTY_E.Text;
                    if (HOUSE_YN.Checked)
                        wfb2ia.HOUSE_YN = "Y";
                    else
                        wfb2ia.HOUSE_YN = "N";
                    wfb2ia.EMP_RATE = EMP_RATE.Text;
                    wfb2ia.CMP_RATE = CMP_RATE.Text;
                    wfb2ia.UNION_RATE = UNION_RATE.Text;
                    wfb2ia.FEES_YN = "N"; //新增(保費計算否)
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA050";

                    string msg = service.addGROUP_KIND(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
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
                    TextBox GINS_ITEM_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_GINS_ITEM_NAME");
                    TextBox AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_AMT");
                    Label PERSON_QTY_S = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_PERSON_QTY_S");
                    TextBox PERSON_QTY_E = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_PERSON_QTY_E");
                    CheckBox HOUSE_YN = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_HOUSE_YN");
                    TextBox EMP_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EMP_RATE");
                    TextBox CMP_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_CMP_RATE");
                    TextBox UNION_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_UNION_RATE");

                    if (PERSON_QTY_E.Text == "")
                        PERSON_QTY_E.Text = "1";
                    if (EMP_RATE.Text == "")
                        EMP_RATE.Text = "0";
                    if (CMP_RATE.Text == "")
                        CMP_RATE.Text = "0";
                    if (UNION_RATE.Text == "")
                        UNION_RATE.Text = "0";

                    if (!check_data(Convert.ToInt32(PERSON_QTY_S.Text), Convert.ToInt32(PERSON_QTY_E.Text),
                         Convert.ToInt32(EMP_RATE.Text), Convert.ToInt32(CMP_RATE.Text), Convert.ToInt32(UNION_RATE.Text)))
                        return;

                    CFB2IA0500DAO wfb2ia = new CFB2IA0500DAO();
                    wfb2ia.TARGET_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["TARGET_TYPE"].ToString().Split('-')[0];
                    wfb2ia.GINS_KIND = gv_result.DataKeys[gv_result.EditIndex].Values["GINS_KIND"].ToString();
                    wfb2ia.GINS_ITEM = gv_result.DataKeys[gv_result.EditIndex].Values["GINS_ITEM"].ToString();
                    wfb2ia.GINS_ITEM_NAME = GINS_ITEM_NAME.Text;
                    wfb2ia.AMT = AMT.Text;
                    wfb2ia.PERSON_QTY_S = PERSON_QTY_S.Text;
                    wfb2ia.PERSON_QTY_E = PERSON_QTY_E.Text;
                    if (HOUSE_YN.Checked)
                        wfb2ia.HOUSE_YN = "Y";
                    else
                        wfb2ia.HOUSE_YN = "N";
                    wfb2ia.EMP_RATE = EMP_RATE.Text;
                    wfb2ia.CMP_RATE = CMP_RATE.Text;
                    wfb2ia.UNION_RATE = UNION_RATE.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    
                    string msg = service.updateGROUP_KIND(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
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
            gv_result.DataKeyNames = new string[] { "TARGET_TYPE", "GINS_KIND", "GINS_ITEM", "PERSON_QTY_S" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2IA0500Search.Visible = true;
            btn_clear.Visible = true;

            WFB2IA0500Save.Visible = false;
            WFB2IA0500Cancel.Visible = false;
            WFB2IA0500Add.Visible = true;
            WFB2IA0500Edit.Visible = true;
            WFB2IA0500Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //驗證輸入的資料
    private bool check_data(int PERSON_QTY_S, int PERSON_QTY_E, int EMP_RATE, int CMP_RATE, int UNION_RATE)
    {
        string msg = "";
        if (PERSON_QTY_E < PERSON_QTY_S)
        {
            msg += "人數迄不允許小於人數起!\\n";
        }

        if ((EMP_RATE + CMP_RATE + UNION_RATE) != 100)
        {
            msg += "員工自付比率+公司負擔比率+福利會負擔比例 需等於100!\\n";
        }

        if (msg != "")
        {
            gv_result.PagerSettings.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            return false;
        }
        return true;
    }

    //取消按鈕
    protected void WFB2IA0500Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2IA0500Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2IA0500Edit.Visible = true;
            WFB2IA0500Delete.Visible = true;
        }

        WFB2IA0500Save.Visible = false;
        WFB2IA0500Cancel.Visible = false;
        WFB2IA0500Add.Visible = true;
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
        gv_result.DataKeyNames = new string[] { "TARGET_TYPE", "GINS_KIND", "GINS_ITEM", "PERSON_QTY_S" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_TARGET_TYPE");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = utilities.getCommCode("IA", "TARGET_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_GINS_KIND");
            if (ddl != null)
            {
                dt = utilities.getCommCode("GINS_KIND", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //以戶計
            HiddenField HID_HOUSE_YN = (HiddenField)e.Row.Cells[9].FindControl("HID_HOUSE_YN");
            if (HID_HOUSE_YN != null)
            {
                CheckBox cb_HOUSE_YN = (CheckBox)e.Row.Cells[9].FindControl("cb_HOUSE_YN");
                if (cb_HOUSE_YN != null)
                {
                    if (HID_HOUSE_YN.Value == "Y")
                        cb_HOUSE_YN.Checked = true;
                    else
                        cb_HOUSE_YN.Checked = false;
                }
            }

            //團保對象別
            Label lb_TARGET_TYPE = (Label)e.Row.Cells[2].FindControl("lb_TARGET_TYPE");
            TextBox txt_PERSON_QTY_S = (TextBox)e.Row.Cells[7].FindControl("txt_PERSON_QTY_S");
            if (lb_TARGET_TYPE != null && txt_PERSON_QTY_S != null)
            {
                if (lb_TARGET_TYPE.ToolTip != "3")
                {
                    //人數_起
                    ((TextBox)e.Row.Cells[7].FindControl("txt_PERSON_QTY_S")).Text = "1";
                    ((TextBox)e.Row.Cells[7].FindControl("txt_PERSON_QTY_S")).Enabled = false;
                    //以戶計
                    ((CheckBox)e.Row.Cells[9].FindControl("cb_HOUSE_YN")).Enabled = false;
                }

                if (lb_TARGET_TYPE.ToolTip == "1" || lb_TARGET_TYPE.ToolTip == "2")
                {
                    //人數_迄
                    ((TextBox)e.Row.Cells[8].FindControl("txt_PERSON_QTY_E")).Text = "1";
                    ((TextBox)e.Row.Cells[8].FindControl("txt_PERSON_QTY_E")).Enabled = false;
                }
                else if (lb_TARGET_TYPE.ToolTip == "4")
                {
                    ((TextBox)e.Row.Cells[8].FindControl("txt_PERSON_QTY_E")).Text = "2";
                    ((TextBox)e.Row.Cells[8].FindControl("txt_PERSON_QTY_E")).Enabled = false;
                }
            }

        }

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
        gv_result.DataKeyNames = new string[] { "TARGET_TYPE", "GINS_KIND", "GINS_ITEM", "PERSON_QTY_S" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }

}