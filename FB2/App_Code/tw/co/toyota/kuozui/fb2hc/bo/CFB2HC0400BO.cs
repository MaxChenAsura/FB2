using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2HC0400BO 的摘要描述
/// </summary>
public class CFB2HC0400BO : BaseService
{
    public CFB2HC0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }    
    //結算作業
    public string WFB2HC0400StlAmt_proc(string pay_ym)
    {
        try 
        {
            string rtnval = "";
            this.BeginTransaction();
            CFB2HC0400DAO dao = new CFB2HC0400DAO();
            string emp_id = SessionHandle.Current.emp_id;
            rtnval = dao.WFB2HC0400StlAmt_proc(pay_ym, emp_id);
            this.Commit();

            rtnval = utilities.getSPLOG("SP_H_CONTRACT_BONUS_STL");
            if (rtnval == "") {
                rtnval = Resources.Resource.wfb2hc_WFB2HC0400StlAmt_proc_ok;
            }
            return rtnval;
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw new Exception(ex.Message);
        }
    }

    public string WFB2HC0400StlLock_proc_step1(string pay_ym)
    {
        try
        {
            string rtnval = "";            
            CFB2HC0400DAO dao = new CFB2HC0400DAO();
            string emp_id = SessionHandle.Current.emp_id;
            rtnval = dao.WFB2HC0400StlLock_proc_step1(pay_ym, emp_id);
            return rtnval;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string WFB2HC0400StlLock_proc_step2(string pay_ym)
    {
        try
        {
            string rtnval = "";
            this.BeginTransaction();
            CFB2HC0400DAO dao = new CFB2HC0400DAO();
            string emp_id = SessionHandle.Current.emp_id;
            rtnval = dao.WFB2HC0400StlLock_proc_step2(pay_ym, emp_id);
            this.Commit();
            return rtnval;
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw new Exception(ex.Message);
        }
    }

    public string WFB2HC0400StlUnLock_proc(string pay_ym)
    {
        try
        {
            string rtnval = "";
            this.BeginTransaction();
            CFB2HC0400DAO dao = new CFB2HC0400DAO();
            string emp_id = SessionHandle.Current.emp_id;
            rtnval = dao.WFB2HC0400StlUnLock_proc(pay_ym, emp_id);
            this.Commit();
            return rtnval;
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw new Exception(ex.Message);
        }
    }
    
}