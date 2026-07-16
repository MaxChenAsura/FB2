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
/// CFB2SP1200BO 的摘要描述
/// </summary>
public class CFB2SP1200BO : BaseService
{
    public CFB2SP1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //駁回-(Dtl)
    public string reject(List<Tuple<string, string>> emp_ids)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    CFB2SP1200DAO sp120DAO = new CFB2SP1200DAO();
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    foreach (var item  in emp_ids)
                    {
                        sp120DAO = new CFB2SP1200DAO();
                        sp120DAO.EMP_ID = item.Item1;
                        sp120DAO.APPROVE_REMARK = item.Item2;
                        sp120DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sp120DAO.FUNC_ID = "FB2SP120";
                        sp120DAO.reject(now);
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


    //核可
    public string approve(List<Tuple<string, string>> emp_ids)
    {
        try
        {

            CFB2SP1200DAO sp120DAO = new CFB2SP1200DAO();
            DataTable dt = new DataTable();
            string rtnmessage = "";//存在檢查後的訊息
            int resultCount = 0;
            //檢查 該發放日期 是否 發薪狀態是否為Y																			
            foreach (var item in emp_ids)
            {
                sp120DAO = new CFB2SP1200DAO();
                sp120DAO.EMP_ID = item.Item1;
                resultCount = sp120DAO.checkIsSalary();
                if (resultCount > 0)
                {
                    rtnmessage += "工號:" + item.Item1 + "的發放日期已薪資計算,不允許執行\\n";
                }
            }

            if (rtnmessage == "")
            {
                try
                {
                   
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //更新優退金檔
                    foreach (var item in emp_ids)
                    {
                        sp120DAO = new CFB2SP1200DAO();
                        sp120DAO.EMP_ID = item.Item1;
                        sp120DAO.APPROVE_REMARK = item.Item2;
                        sp120DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sp120DAO.FUNC_ID = "FB2SP120";
                        sp120DAO.approve(now);

                        //新增節金相關檔案
                        sp120DAO.insertFestivalD("TB_S_M_FESTIVAL_D");
                        sp120DAO.insertFestivalD("TB_S_R_FESTIVAL_D");
                        sp120DAO.insertFestivalH();
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

}