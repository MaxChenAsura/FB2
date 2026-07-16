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
/// CFB2HC0300BO 的摘要描述
/// </summary>
public class CFB2HC0300BO : BaseService
{
    public CFB2HC0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string getDept_name(string dept_no)
    {
        try
        {
            CFB2HC0300DAO dao = new CFB2HC0300DAO();
            return dao.getDept_name(dept_no);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getEmp_name(string emp_id)
    {
        try
        {
            CFB2HC0300DAO dao = new CFB2HC0300DAO();
            return dao.getEmp_name(emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除
    public string deleteData(string EMP_ID,string START_DT,string BONUS_TYPE,string PAY_YM)
    {
        CFB2HC0300DAO hc030DAO = new CFB2HC0300DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();                   
                    //刪除
                    hc030DAO.deleteData(EMP_ID, START_DT, BONUS_TYPE, PAY_YM);
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}