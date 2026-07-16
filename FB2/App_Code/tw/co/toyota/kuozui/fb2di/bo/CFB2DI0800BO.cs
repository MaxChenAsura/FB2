using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DI0800BO 的摘要描述
/// </summary>
public class CFB2DI0800BO : BaseService
{
    public CFB2DI0800BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public List<string> getSearchDept()
    {
        try
        {
            CFB2DI0800DAO dao = new CFB2DI0800DAO();
            Dept_Search tv = new Dept_Search();
            string sp_dept = "";
            string header = "N";
            List<string> dept;
            List<string> spDept;
            //ACES權限
            ACESLib.ACES aces = new ACESLib.ACES();

            //取得角色資料權限
            String dbRole = aces.GetRoles();
            IList<string> role = dbRole.Split(',');

            foreach (string item in role)
            {
                //取得部門權限聯集
                try
                {
                    string derolecd = item.Trim();
                    ACESLib.DEPTBean deptbean = (ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd);
                    string SysCode = deptbean.SysCode; //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        if (code.Trim().Equals("SUPER"))
                        {
                            header = "Y";
                            break;
                        }
                    }

                    
                    sp_dept += deptbean.Departments; //取得 「使用其它部門權限」

                }
                catch (Exception)
                {

                }


            }

            spDept = new List<string>();
            dept = new List<string>();

            spDept = sp_dept.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (header == "Y")
            {
                dept = dao.getDEPT_LIST();
                dept.AddRange(spDept);
            }
            else
            {
                dept = tv.getHead_Dept(SessionHandle.Current.emp_id);
                dept.AddRange(spDept);
                dept.Add(SessionHandle.Current.dept_no);
            }

            return dept;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getLeaveData(string dept_no, string target_type, string ym)
    {
        try
        {
            CFB2DI0800DAO dao = new CFB2DI0800DAO();
            return dao.getLeaveData(dept_no, target_type, ym);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getOvertimeTargetData(string dept_no, string target_type, string ym)
    {
        try
        {
            CFB2DI0800DAO dao = new CFB2DI0800DAO();
            return dao.getOvertimeTargetData(dept_no, target_type, ym);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getData(string emp_id, string ym)
    {
        try
        {
            CFB2DI0800DAO dao = new CFB2DI0800DAO();
            return dao.getdata(emp_id, ym);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string call_SP_DI_OVERTIME_TOTAL_IFLOW(string dept_no, string work_day_cd, string ym, string target_type)
    {
        CFB2DI0800DAO di080DAO = new CFB2DI0800DAO();
        try
        {
            di080DAO.SP_DI_OVERTIME_TOTAL_IFLOW(dept_no, work_day_cd, ym, target_type);
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    public System.Data.DataTable getTOTAL_TIME_OVERTIME_IFLOW(string dept_no, string work_day_cd, string ym, string target_type)
    {
        try
        {
            CFB2DI0800DAO dao = new CFB2DI0800DAO();
            return dao.getTOTAL_TIME_OVERTIME_IFLOW(dept_no, work_day_cd, ym, target_type);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getDEPT_NAME(string dept_no, string dept_no_list)
    {
        try
        {
            CFB2DI0800DAO wfb2di = new CFB2DI0800DAO();
            return wfb2di.getDEPT_NAME(dept_no, dept_no_list);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getOVERTIME_SPECIAL_HOUR()
    {
        try
        {
            CFB2DI0800DAO wfb2di = new CFB2DI0800DAO();
            return wfb2di.getOVERTIME_SPECIAL_HOUR();
        }
        catch (Exception)
        {

            throw;
        }
    }
}