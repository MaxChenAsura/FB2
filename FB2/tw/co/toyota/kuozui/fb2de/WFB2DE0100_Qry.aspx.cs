using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0100_Qry : BasePage
{
    CFB2DE0100BO service = new CFB2DE0100BO();
    private string emp_id = "";
    private string emp_name = "";
    private string emp_company_cd = "";
    private string func_id = "FB2DE010";
    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionHandle.Current.emp_id = "11001";
        //SessionHandle.Current.emp_name = "TEST"; 
        emp_id = SessionHandle.Current.emp_id;          //取得使用者ID
        emp_name = SessionHandle.Current.emp_name;      //取得使用者Name
        CFB2DE0100DAO dao = new CFB2DE0100DAO();
        emp_company_cd = dao.getCOMPANY_CD(emp_id);     //取得KZ會社區分

        if (!IsPostBack)
        {
            InitialView();
            InitData();
        }

    }

    //初始畫面
    private void InitialView()
    {

        for (int i = 0; i <= 23; i++)
        {
            string HHValue = Convert.ToString(i);
            if (i <= 9)
            {
                HHValue = "0" + HHValue;
            }
            ddl_BR_END.Items.Insert(i, HHValue);
            ddl_BR_START.Items.Insert(i, HHValue);
            ddl_COURSE_DN_TIME.Items.Insert(i, HHValue);
            ddl_DN_END.Items.Insert(i, HHValue);
            ddl_DN_START.Items.Insert(i, HHValue);
            ddl_LAST_DN_TIME.Items.Insert(i, HHValue);
            ddl_LAST_BR_TIME.Items.Insert(i, HHValue);
            ddl_MD_START.Items.Insert(i, HHValue);
            ddl_MD_END.Items.Insert(i, HHValue);
        }

    }

    //初始資料
    private void InitData()
    {
        CFB2DE0100DAO dao = new CFB2DE0100DAO();
        DataTable dt = dao.getTB_D_M_RES_PARA(emp_company_cd);
        if (dt.Rows.Count > 0)              //以 KZ會社區分 讀取 餐廳參數設定檔 如有資料則帶入
        {
            txt_BF_AMOUNT.Text = dt.Rows[0]["BF_AMOUNT"].ToString();
            txt_DN_AMOUNT.Text = dt.Rows[0]["DN_AMOUNT"].ToString();

            string BR_START = dt.Rows[0]["BR_START"].ToString();
            string BR_END = dt.Rows[0]["BR_END"].ToString();
            string COURSE_DN_TIME = dt.Rows[0]["COURSE_DN_TIME"].ToString();
            string DN_END = dt.Rows[0]["DN_END"].ToString();
            string DN_START = dt.Rows[0]["DN_START"].ToString();
            string LAST_BR_TIME = dt.Rows[0]["LAST_BR_TIME"].ToString();
            string LAST_DN_TIME = dt.Rows[0]["LAST_DN_TIME"].ToString();
            //txt
            if (BR_START.Length ==4)            
                txt_BR_START.Text = BR_START.Substring(2, 2);
            if (BR_END.Length == 4) 
                txt_BR_END.Text = BR_END.Substring(2, 2);
            if (COURSE_DN_TIME.Length == 4) 
                txt_COURSE_DN_TIME.Text = COURSE_DN_TIME.Substring(2, 2);
            if (DN_END.Length == 4) 
                txt_DN_END.Text = DN_END.Substring(2, 2);
            if (DN_START.Length == 4) 
                txt_DN_START.Text = DN_START.Substring(2, 2);
            if (LAST_BR_TIME.Length == 4) 
                txt_LAST_BR_TIME.Text = LAST_BR_TIME.Substring(2, 2);
            if (LAST_DN_TIME.Length == 4)
                txt_LAST_DN_TIME.Text = LAST_DN_TIME.Substring(2, 2);
            if (dt.Rows[0]["MD_START"].ToString().Length == 4)
                txt_MD_START.Text = dt.Rows[0]["MD_START"].ToString().Substring(2, 2);
            if (dt.Rows[0]["MD_END"].ToString().Length == 4)
                txt_MD_END.Text = dt.Rows[0]["MD_END"].ToString().Substring(2, 2);

            //ddl
            if (BR_START.Length == 4)
                ddl_BR_START.SelectedValue = BR_START.Substring(0, 2);
            if (BR_END.Length == 4)
                ddl_BR_END.SelectedValue = BR_END.Substring(0, 2);
            if (COURSE_DN_TIME.Length == 4)
                ddl_COURSE_DN_TIME.SelectedValue = COURSE_DN_TIME.Substring(0, 2);
            if (DN_END.Length == 4)
                ddl_DN_END.SelectedValue = DN_END.Substring(0, 2);
            if (DN_START.Length == 4)
                ddl_DN_START.SelectedValue = DN_START.Substring(0, 2);
            if (LAST_BR_TIME.Length == 4)
                ddl_LAST_BR_TIME.SelectedValue = LAST_BR_TIME.Substring(0, 2);
            if (LAST_DN_TIME.Length == 4)
                ddl_LAST_DN_TIME.SelectedValue = LAST_DN_TIME.Substring(0, 2);
            if (dt.Rows[0]["MD_START"].ToString().Length == 4)
                ddl_MD_START.SelectedValue = dt.Rows[0]["MD_START"].ToString().Substring(0, 2);
            if (dt.Rows[0]["MD_END"].ToString().Length == 4)
                ddl_MD_END.SelectedValue = dt.Rows[0]["MD_END"].ToString().Substring(0, 2);           
        }
        else
        {
            txt_BF_AMOUNT.Text = "";
            txt_BR_END.Text = "";
            txt_BR_START.Text = "";
            txt_COURSE_DN_TIME.Text = "";
            txt_DN_AMOUNT.Text = "";
            txt_DN_END.Text = "";
            txt_DN_START.Text = "";
            txt_LAST_BR_TIME.Text = "";
            txt_LAST_DN_TIME.Text = "";
            txt_MD_START.Text = "";
            txt_MD_END.Text = "";
            ddl_BR_START.SelectedIndex = 0;
            ddl_BR_END.SelectedIndex = 0;
            ddl_COURSE_DN_TIME.SelectedIndex = 0;
            ddl_DN_END.SelectedIndex = 0;
            ddl_DN_START.SelectedIndex = 0;
            ddl_LAST_BR_TIME.SelectedIndex = 0;
            ddl_LAST_DN_TIME.SelectedIndex = 0;
            ddl_MD_START.SelectedIndex = 0;
            ddl_MD_END.SelectedIndex = 0;
        }
    }

    //儲存
    protected void WFB2DE0100Save_Click(object sender, EventArgs e)
    {

        bool check = CheckVaild(); //檢查時間的範圍是否輸入錯誤
        int issuccess ;
        if (check)
        {
            CFB2DE0100DAO dao = new CFB2DE0100DAO();
            DataTable dt = service.getTB_D_M_RES_PARA(emp_company_cd);
            if (dt.Rows.Count > 0)              //以 KZ會社區分 讀取 餐廳參數設定檔 如有資料則帶入
            {
                issuccess = service.update_TB_D_M_RES_PARA(emp_company_cd, getBfAountValue, getDnAmountValue, getBrStartValue, getBrEndValue, getDnStartValue,
                                                            getDnEndValue, getLastBrTimeValue, getLastDnTimeValue, getCourseDnTimeValue, emp_id, func_id, getMDStartValue, getMDEndValue);
                 //issuccess = dao.update_TB_D_M_RES_PARA(emp_company_cd, getBfAountValue, getDnAmountValue, getBrStartValue, getBrEndValue, getDnStartValue,
                 //                                           getDnEndValue, getLastBrTimeValue, getLastDnTimeValue, getCourseDnTimeValue, emp_id, func_id);
                if (issuccess>0)
                    showMessage("modSuccessMessage");
                else
                    showMessage("modFailMessage");
            }
            else
            {
                issuccess = service.InsertData_TB_D_M_RES_PARA(emp_company_cd, getBfAountValue, getDnAmountValue, getBrStartValue, getBrEndValue, getDnStartValue,
                                                            getDnEndValue, getLastBrTimeValue, getLastDnTimeValue, getCourseDnTimeValue, emp_id, func_id, getMDStartValue, getMDEndValue);

                //issuccess = dao.InsertData_TB_D_M_RES_PARA(emp_company_cd, getBfAountValue, getDnAmountValue, getBrStartValue, getBrEndValue, getDnStartValue,
                //                                            getDnEndValue, getLastBrTimeValue, getLastDnTimeValue, getCourseDnTimeValue, emp_id, func_id);
                if (issuccess>0)
                    showMessage("addSuccessMessage");
                else
                    showMessage("addFailMessage");
            }
        }
    }
 
    //取消
    protected void WFB2DE0100Cancel_Click(object sender, EventArgs e)
    {
        InitData();
    }
    //檢查時間是否輸入錯誤
    private bool CheckVaild()
    {

        String errormessage = "";
        if (Convert.ToInt32(getBrStartValue) > Convert.ToInt32(getBrEndValue))
            errormessage += "早餐用餐時間起不可大於早餐用餐時間迄\\n";
        if (Convert.ToInt32(getDnStartValue) > Convert.ToInt32(getDnEndValue))
            errormessage += "晚餐用餐時間起不可大於晚餐用餐時間迄\\n";
        if (Convert.ToInt32(getMDStartValue) > Convert.ToInt32(getMDEndValue))
            errormessage += "午餐用餐時間起不可大於午餐用餐時間迄\\n";
        if (Convert.ToInt32(getBrStartValue) <= Convert.ToInt32(getDnEndValue) && Convert.ToInt32(getBrStartValue) >= Convert.ToInt32(getDnStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getBrEndValue) <= Convert.ToInt32(getDnEndValue) && Convert.ToInt32(getBrEndValue) >= Convert.ToInt32(getDnStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getDnStartValue) <= Convert.ToInt32(getBrEndValue) && Convert.ToInt32(getDnStartValue) >= Convert.ToInt32(getBrStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getDnEndValue) <= Convert.ToInt32(getBrEndValue) && Convert.ToInt32(getDnEndValue) >= Convert.ToInt32(getBrStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getMDStartValue) <= Convert.ToInt32(getBrEndValue) && Convert.ToInt32(getMDStartValue) >= Convert.ToInt32(getBrStartValue))
            errormessage += "早、午餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getMDEndValue) <= Convert.ToInt32(getBrEndValue) && Convert.ToInt32(getMDEndValue) >= Convert.ToInt32(getBrStartValue))
            errormessage += "早、午餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getMDStartValue) <= Convert.ToInt32(getDnEndValue) && Convert.ToInt32(getMDStartValue) >= Convert.ToInt32(getDnStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        else if (Convert.ToInt32(getMDEndValue) <= Convert.ToInt32(getDnEndValue) && Convert.ToInt32(getMDEndValue) >= Convert.ToInt32(getDnStartValue))
            errormessage += "早、晚餐用餐時段不可重疊\\n";
        if (errormessage.Equals(""))
            return true;
        else
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('" + errormessage + "');", true);
            return false;
        }
    }

    public string getBfAountValue
    {
        get { return txt_BF_AMOUNT.Text; }
    }
    public string getDnAmountValue
    {
        get { return txt_DN_AMOUNT.Text; }
    }
    public string getBrStartValue
    {
        get
        {
            return ddl_BR_START.SelectedValue + MMFormat(txt_BR_START.Text);
        }
    }
    public string getBrEndValue
    {
        get { return ddl_BR_END.SelectedValue + MMFormat(txt_BR_END.Text); }
    }
   
    public string getMDStartValue
    {
        get
        {
            return ddl_MD_START.SelectedValue + MMFormat(txt_MD_START.Text);
        }
    }
    public string getMDEndValue
    {
        get { return ddl_MD_END.SelectedValue + MMFormat(txt_MD_END.Text); }
    }
    public string getDnStartValue
    {
        get { return ddl_DN_START.SelectedValue + MMFormat(txt_DN_START.Text); }
    }
    public string getDnEndValue
    {
        get { return ddl_DN_END.SelectedValue + MMFormat(txt_DN_END.Text); }
    }
    public string getLastBrTimeValue
    {
        get { return ddl_LAST_BR_TIME.SelectedValue + MMFormat(txt_LAST_BR_TIME.Text); }
    }
    public string getLastDnTimeValue
    {
        get { return ddl_LAST_DN_TIME.SelectedValue + MMFormat(txt_LAST_DN_TIME.Text); }
    }
    public string getCourseDnTimeValue
    {
        get { return ddl_COURSE_DN_TIME.SelectedValue + MMFormat(txt_COURSE_DN_TIME.Text); }
    }

    private string MMFormat(string mm)
    {
        string mmvalue = mm;
        if (mmvalue.Length == 1)
            mmvalue = "0" + mm;
        return mmvalue;
    }


}