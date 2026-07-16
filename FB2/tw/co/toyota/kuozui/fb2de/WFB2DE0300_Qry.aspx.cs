using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0300_Qry : BasePage
{
    CFB2DE0300BO service = new CFB2DE0300BO();
    private string emp_id = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = SessionHandle.Current.emp_id;          //取得使用者ID
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        

        if (!IsPostBack)
        {
            InitialView();
        }

        //string event_target = Request.Form.Get("__EVENTTARGET");
        //string event_argu = Request.Form.Get("__EVENTARGUMENT");
        //if (event_target == "question")
        //{
        //    if (event_argu == "true")
        //    {
        //        Execute();
        //    }
          
        //}


    }

    //初始畫面
    private void InitialView()
    {
        try
        {
            //textbox
            CFB2DE0300DAO dao = new CFB2DE0300DAO();            
            if (dao.getMANAGER_YM().Equals(""))
            {
                txt_MANAGER_YM.Text = "";
            }
            else
            {
                //service
                DateTime MANAGER_YM = DateTime.ParseExact(service.getMANAGER_YM(), "yyyyMM", null);
                //DateTime MANAGER_YM = DateTime.ParseExact(dao.getMANAGER_YM(), "yyyyMM", null);
                txt_MANAGER_YM.Text = MANAGER_YM.AddMonths(1).ToString("yyyy/MM");
            }

            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();
            //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
            string syscodeatt = "";
            string st = "";
            bool isPLANT_CD = false;
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                //derolecd = "FB2DBOWNER";
                string dept = deptbean.IsDEPT;
                string departments = deptbean.Departments;
                string SysCode = deptbean.SysCode;

                foreach (string code in SysCode.Split(','))
                {
                    if (code.Trim().Equals("PLANT_CD"))
                    {
                        isPLANT_CD = true;
                        syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());
                        syscodeatt = syscodeatt.Trim();

                        if(st.IndexOf(syscodeatt,0) == -1){
                            st = st + syscodeatt + ",";                            
                        }
                    }                 

                }

            }
            //   syscodeatt = "1, 2";
            syscodeatt = st;
            if (isPLANT_CD)
            {
                if (syscodeatt.Equals(""))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "information", "alert('您已設定大分類=PLANT_CD，但尚未設定小分類項目');", true);
                    UCCommCodeDropDwonList.DataTextField = "SUB_DESC";
                    UCCommCodeDropDwonList.DataValueField = "SUB_CD";
                    UCCommCodeDropDwonList.SUB_CDs = "dont'show";
                }
                else if (syscodeatt.Length > 1)
                {
                    UCCommCodeDropDwonList.FirstItem = " ";
                    UCCommCodeDropDwonList.MAIN_CDs = "PLANT_CD";
                    UCCommCodeDropDwonList.DataTextField = "SUB_DESC";
                    UCCommCodeDropDwonList.DataValueField = "SUB_CD";
                }
                else
                {
                    UCCommCodeDropDwonList.MAIN_CDs = "PLANT_CD";
                    UCCommCodeDropDwonList.DataTextField = "SUB_DESC";
                    UCCommCodeDropDwonList.DataValueField = "SUB_CD";
                    UCCommCodeDropDwonList.SUB_CDs = syscodeatt;
                }

            }
            else
            {
                UCCommCodeDropDwonList.FirstItem = " ";
                UCCommCodeDropDwonList.MAIN_CDs = "PLANT_CD";
                UCCommCodeDropDwonList.DataTextField = "SUB_DESC";
                UCCommCodeDropDwonList.DataValueField = "SUB_CD";
            }

            UCCommCodeDropDwonList.SelectedIndex = 0;
        }
        catch (Exception)
        {
            
            throw;
        }        

    }


    protected void WFB2DE0300Execute_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            //string st = txt_MANAGER_YM.Text;
            CFB2DE0300DAO dao = new CFB2DE0300DAO();     
            //DateTime YM = DateTime.ParseExact(txt_MANAGER_YM.Text, "yyyy/MM", null);
            string MANAGER_YM = txt_MANAGER_YM.Text.Replace("/","");
            string PLANT_CD = UCCommCodeDropDwonList.SelectedValue.ToString();

            dao.MANAGER_YM = MANAGER_YM;
            dao.PLANT_CD = UCCommCodeDropDwonList.SelectedValue.ToString();
            dao.emp_id = SessionHandle.Current.emp_id;

            //有無資料
            DataTable dt = service.qry_TB_D_R_RES_ACTURL(dao.MANAGER_YM, dao.PLANT_CD);
            if (dt.Rows.Count == 0)
            {
                err += "此年月沒有資料可計算\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }
            else
            {
                string msg = service.Exec(dao);
                //dao.del_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM, PLANT_CD);
                //bool finish = false;
                //finish = dao.Execute(MANAGER_YM, PLANT_CD, emp_id);
                if (msg != "0")
                {
                    showMessage("monthExecuteFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("monthExecuteSuccessMessage");
                }
            }

            //if (msg.Equals("0"))
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "information", "alert('月結成功');", true);
            //else
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "information", "alert('月結失敗');", true);
        }
        catch (Exception)
        {

            throw;
        }

        //查詢是否內含已月結資料

        //WFB2DE0300DAO dao = new WFB2DE0300DAO();
        //DateTime MANAGER_YM = DateTime.ParseExact(txt_MANAGER_YM.Text, "yyyy/MM", null);
        //string PLANT_CD = UCCommCodeDropDwonList.SelectedValue.ToString();

        //DataTable dt = service.qry_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM.ToString("yyyyMM"), PLANT_CD);
        ////DataTable dt = dao.qry_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM.ToString("yyyyMM"));
        //if (dt.Rows.Count > 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "<script language='javascript'>checkvalue();</script>");
        //}
        //else
        //{
           
        //}
    }
    protected void WFB2DE0300Cancel_Click(object sender, EventArgs e)
    {
        InitialView();
    }
    private void Execute()
    {
        string err = "";
        try
        {
            CFB2DE0300DAO dao = new CFB2DE0300DAO();            
            DateTime YM = DateTime.ParseExact(txt_MANAGER_YM.Text, "yyyy/MM", null);
            string MANAGER_YM = YM.ToString("yyyyMM");
            string PLANT_CD = UCCommCodeDropDwonList.SelectedValue.ToString();

            dao.MANAGER_YM = YM.ToString("yyyyMM");
            dao.PLANT_CD = UCCommCodeDropDwonList.SelectedValue.ToString();
            dao.emp_id = SessionHandle.Current.emp_id;

            //有無資料
            DataTable dt = service.qry_TB_D_R_RES_ACTURL(dao.MANAGER_YM, dao.PLANT_CD);
            if (dt.Rows.Count == 0)
            {
                err += "此年月沒有資料可計算\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }
            else {
                string msg = service.Exec(dao);
                //dao.del_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM, PLANT_CD);
                //bool finish = false;
                //finish = dao.Execute(MANAGER_YM, PLANT_CD, emp_id);
                if (msg != "0")
                {
                    showMessage("monthExecuteFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("monthExecuteSuccessMessage");
                }
            }

            //if (msg.Equals("0"))
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "information", "alert('月結成功');", true);
            //else
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "information", "alert('月結失敗');", true);
        }
        catch (Exception)
        {
            
            throw;
        }
        
   

    }

}