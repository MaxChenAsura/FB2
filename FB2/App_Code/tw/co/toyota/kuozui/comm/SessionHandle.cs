using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// SessionHandle 的摘要描述
/// </summary>
[Serializable]
public class SessionHandle : BaseDAO
{
    
	public SessionHandle()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public void setUserSession(string emp_id)
    {
        
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            
            sb.Append("Select a.EMP_ID,a.EMP_NAME,b.dept_no,b.dept_name,b.head_emp_id,b.up_dept_no ,b.dept_level");
            sb.Append(" from VW_H_EMP_DATA a,VW_H_DEPT_DATA b");
            sb.Append(" where a.DEPT_NO = b.DEPT_NO");
            sb.Append(" and emp_id = @emp_id");
            ht.Add("@emp_id", emp_id);

            DataTable user_info = dbConn.Query(sb, ht);

            if (user_info.Rows.Count > 0)
            {
                //建立Session
                SessionHandle.Current.emp_id = user_info.Rows[0]["EMP_ID"].ToString();
                SessionHandle.Current.emp_name = user_info.Rows[0]["EMP_NAME"].ToString();
                SessionHandle.Current.dept_no = user_info.Rows[0]["dept_no"].ToString();
                SessionHandle.Current.dept_name = user_info.Rows[0]["dept_name"].ToString();
                SessionHandle.Current.head_emp_id = user_info.Rows[0]["head_emp_id"].ToString();
                SessionHandle.Current.up_dept_no = user_info.Rows[0]["up_dept_no"].ToString();
                SessionHandle.Current.dept_level = user_info.Rows[0]["dept_level"].ToString();
                //setAuthData();
            }
            
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    //改寫在utilities 角色權限設定
    /*
    public void setAuthData() {
        try
        {
            string is_super = "N";
            string is_dept = "N";     //取得 「是否含部門以下」
            string departments_result = "";

            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            List<string> all_departments = new List<string>();
            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                try
                {
                    string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                    ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                    string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                    string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                    string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                        if (code.Trim().Equals("SUPER"))
                        {
                            is_super = "Y";
                            break;
                        }
                    }
                    if (dept == "Y")
                        is_dept = "Y";
                    all_departments.Add(departments);
                }
                catch (Exception)
                {

                }
            }

            if (all_departments.Count > 0)
            {
                string final_departments = "";
                List<string> departments = new List<string>();
                for (int i = 0; i < all_departments.Count; i++)
                {
                    for (int k = 0; k < all_departments[i].Split(',').Length; k++)
                    {
                        string temp = all_departments[i].Split(',')[k].Trim();
                        if (departments.Contains(temp))
                            continue;

                        departments.Add(temp);
                    }
                }

                for (int i = 0; i < departments.Count; i++)
                {
                    if (i == 0)
                    {
                        final_departments = departments[i];
                        continue;
                    }
                    final_departments += "," + departments[i];
                }

                departments_result = final_departments;
            }
            SessionHandle.Current.is_super = is_super;
            SessionHandle.Current.departments = departments_result;
            SessionHandle.Current.is_dept = is_dept;
        }
        catch (Exception ex)
        {
        }

    }
    */
    public static SessionHandle Current
    {
        get
        {
            SessionHandle session =
              (SessionHandle)HttpContext.Current.Session["__MySession__"];
            if (session == null)
            {
                session = new SessionHandle();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }

    public string fun_id { get; set; }
    public string emp_id { get; set; }
    public string emp_name { get; set; }
    public string dept_no { get; set; }
    public string dept_name { get; set; }
    public string head_emp_id { get; set; }
    public string up_dept_no { get; set; }
    public string dept_level { get; set; }
    public string is_super { get; set; }    //資料角色是否為super
    public string departments { get; set; } //部門權限
    public string is_dept { get; set; } // 「是否含部門以下」
    

    public IList<string> db_role { get; set; }

    public List<FUNC_DATA> FUNC_DATAs { get; set; }
    public string UID { get; set; }

    public string FUNC_NAME { get; set; }

    public string FUNC_ID { get; set; }
}
public class FUNC_DATA
{
    public string UID { get; set; }

    public string FUNC_NAME { get; set; }

    public string FUNC_ID { get; set; }
}