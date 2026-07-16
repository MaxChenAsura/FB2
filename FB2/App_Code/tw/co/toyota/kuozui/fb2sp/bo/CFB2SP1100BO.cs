using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
/// <summary>
/// CFB2SP1100BO 的摘要描述
/// </summary>
public class CFB2SP1100BO : BaseService
{
    public CFB2SP1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //刪除
    public string deleteData(List<string> emp_ids)
    {
        CFB2SP1100DAO sp110DAO = new CFB2SP1100DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var emp_id in emp_ids)
                    {
                        //刪除 年獎維護檔
                        sp110DAO.deleteData(emp_id);
                    }
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


    //檢核
    public string valid(CFB2SP1100DAO sp110DAO)
    {
        string rtnmessage = "0";//存在檢查後的訊息
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            //1.檢查發放日期須大於薪資已計算月份
            dt = sp110DAO.checkExecute2();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                rtnmessage = "發放日期不可小於已計算薪資年月:" + dt.Rows[0]["SALARY_YM"].ToString();
                return rtnmessage;
            }

            //2.檢查是否已核可或已存在
            dt = sp110DAO.checkExecute1();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                if (dt.Rows[0]["APPROVE_BY"].ToString() != "")
                {
                    rtnmessage = "此工號優退金資料主管已核可,不允重算 \\n";
                }
                else
                {
                    rtnmessage = "confirm";
                }
            }
            dt.Clear();
           
            return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    //計算
    public string execute(CFB2SP1100DAO sp110DAO)
    {
        try
        {
            string rtnmessage = "";//存在檢查後的訊息
            //檢查 該發放日期 是否 發薪狀態是否為Y	
            int resultCount = sp110DAO.checkIsSalary();
            if (resultCount > 0)
            {
                rtnmessage += "發放日期已薪資計算,不允許執行\\n";
            }

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    sp110DAO.execute();
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