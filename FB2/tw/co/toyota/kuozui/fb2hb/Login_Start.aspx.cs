using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_Login_Start : BasePage
{   
    //Service 物件
    private CFB2HB0700BO service = new CFB2HB0700BO();

    protected void Page_Load(object sender, EventArgs e)    
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //txt_JOIN_DT_2.Text = DateTime.Now.ToString("yyyy/MM/dd");
            this.hid_is_check.Value = Request.QueryString["isCheck"];

            //查詢參數 
            //this.hid_join_dt.Value = Request.QueryString["join_dt"];

            //this.hid_join_dt.Value = Request.QueryString["emp_name"];
            //this.hid_join_dt.Value = Request.QueryString["dept_no"];
            //this.hid_join_dt.Value = Request.QueryString["company_cd"];
            //this.hid_join_dt.Value = Request.QueryString["plant_cd"];
            //this.hid_join_dt.Value = Request.QueryString["emp_cd"];
            //this.hid_join_dt.Value = Request.QueryString["login_cd"];
            //this.hid_join_dt.Value = Request.QueryString["ws_cd"]; 

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime dt;
        if (txt_JOIN_DT_2.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "入社日期不可為空白" + "');", true);
            return;
        }
        else {
            if (DateTime.TryParse(txt_JOIN_DT_2.Text, out dt) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "入社日期格式錯誤" + "');", true);
                return;
            }
            if (hid_is_check.Value =="Y")
            {
                DateTime dt3 = DateTime.Parse(txt_JOIN_DT_2.Text);
                DateTime dt4 = DateTime.Parse(DateTime.Now.ToShortDateString());

                if (dt4 < dt3)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "入社日不可大於系統日" + "');", true);
                    return;
                }
            }
           
        }
        //入社日檢查
        CFB2HC0100BO hc010BO = new CFB2HC0100BO();
        ArrayList data = hc010BO.Check_FN_S_SALARY_YM(txt_JOIN_DT_2.Text);
        if (data.Count > 0)
        {
            if (((string[])data[0])[0] != "")
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                string salaryYM = dao.Get_FN_S_SALARY_YM();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "入社日需大於已薪結月(" + salaryYM + ")的月底" + "');", true);
                return;
            }
        }

        //取得資料
        OpenWindowRtnJson json = new OpenWindowRtnJson();
        json.CD = txt_JOIN_DT_2.Text;
       
        string strJson = JsonConvert.SerializeObject(json, Formatting.None);

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "return", "ReturnValue('" + strJson + "');", true);


        //開始作業
        //不報到處理
        string userid = SessionHandle.Current.emp_id;
        string join_dt = Request.QueryString["join_dt"];
        string emp_name = Request.QueryString["emp_name"];
        string dept_no = Request.QueryString["dept_no"];
        string company_cd = Request.QueryString["company_cd"];
        string plant_cd = Request.QueryString["plant_cd"];
        string emp_cd = Request.QueryString["emp_cd"];
        //string login_cd = Request.QueryString["login_cd"];
        string ws_cd = Request.QueryString["ws_cd"];

        //存到 不報到人員歷史檔
        //service.insert_History(join_dt, emp_name, dept_no, company_cd, plant_cd, emp_cd, ws_cd, userid);

        //開始 啟動已報到作業
        //取得所需參數
        //service.get_Next_Empid("HB","NEXT_EMP_ID");
        //service.get_getKZ_CONTRACT_MONTHS("HB", "getKZ_CONTRACT_MONTHS");
        //service.get_OTH1_CONTRACT_MONTHS("HB", "OTH1_CONTRACT_MONTHS");
        //service.get_W_OTH1_CONTRACT_EDT("HB", "W_OTH1_CONTRACT_EDT");
        //service.get_EXAM_DAYS("HB", "EXAM_DAYS");
        

    }
}