using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2de_WFB2DE0200_Qry : BasePage
{
    CFB2DE0200BO service = new CFB2DE0200BO();
    
    private string emp_id = "";
    private string emp_name = "";
    private string emp_company_cd = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = SessionHandle.Current.emp_id;          //取得使用者ID
        emp_name = SessionHandle.Current.emp_name;      //取得使用者Name
        CFB2DE0200DAO dao = new CFB2DE0200DAO();
        emp_company_cd = dao.getCOMPANY_CD(emp_id);     //取得KZ會社區分
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value
            getMaxDay();
        }
    }

    private void getMaxDay()
    {
        CFB2DE0200DAO dao = new CFB2DE0200DAO();
        string st = "";        
        try
        {
            
            st = dao.getMaxDay();
            if (st == "")
            {
                txt_MANAGER_DT.Text = "";
                txt_MANAGER_DT_NOW_S.Text = "";
                txt_MANAGER_DT_NOW_E.Text = "";
            }
            else {
                txt_MANAGER_DT.Text = st.Replace("-", "/");
                //本回結算日期 
                txt_MANAGER_DT_NOW_S.Text = Convert.ToString(Convert.ToDateTime(st).AddDays(1).ToString("yyyy/MM/dd"));
                txt_MANAGER_DT_NOW_E.Text = Convert.ToString(Convert.ToDateTime(st).AddDays(1).ToString("yyyy/MM/dd"));
            }   
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2DE0200Execute_Click(object sender, EventArgs e)
    {
        CFB2DE0200DAO dao = new CFB2DE0200DAO();
        dao.COMPANY_CD = emp_company_cd;
        dao.SDT = txt_MANAGER_DT_NOW_S.Text.Replace("/", "-");
        dao.EDT = txt_MANAGER_DT_NOW_E.Text.Replace("/", "-");
        string err = "";
        //檢查有無資料
        err = service.checkData(dao);
        //if (dt1.Rows.Count ==0)
        //{
        //     err += "此日期區間沒有資料可計算\\n";
        //     ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
        //     return;
        //}
        if (err != "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            return;
        }
        else
        {
            dao.SDT = txt_MANAGER_DT_NOW_S.Text;
            dao.EDT = txt_MANAGER_DT_NOW_E.Text;
            string msg = service.doExec(dao);
            if (msg != "0")
            {
                showMessage("calFailMessage", msg);
                return;
            }
            else
            {
                showMessage("calSuccessMessage");
            }
        }

        
    }
}