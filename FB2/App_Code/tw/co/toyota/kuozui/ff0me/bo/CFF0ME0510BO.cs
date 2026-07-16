using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFF0ME0500BO 的摘要描述
/// </summary>
public class CFF0ME0510BO : BaseService
{

    public CFF0ME0510BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string chkTime() 
    {
        string rtnmessage = "";
        bool isVaild =true;
        try
        {
            //檢查:以下時間無法執行
            /*11:50~12:30, 16:50~17:30, 22:00~22:30*/
            string hh = DateTime.Now.Hour.ToString();
            string mm = DateTime.Now.Minute.ToString();
            decimal nowTime = Convert.ToDecimal((hh + mm));
            if (nowTime >= 1150 && nowTime <= 1230)
                isVaild = false;
            if (nowTime >= 1650 && nowTime <= 1730)
                isVaild = false;
            if (nowTime >= 2200 && nowTime <= 2230)
                isVaild = false;

            if (isVaild == false)
                rtnmessage = "現在時間不允許執行 (11:50-12:30/16:50-17:30/22:00-22:30)!";

            return rtnmessage;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //執行
    public string exec_SP()
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功
        CFF0ME0510DAO DC010DAO = new CFF0ME0510DAO();
        try
        {
            //檢查:以下時間無法執行
            //rtnmessage=chkTime();

            //若檢查正確
            if (rtnmessage == "")
            {
                //轉中介檔
                rtnmessage = DC010DAO.exec_SP_IMPORT("SP_MM001_PRICE_01");
                rtnmessage = DC010DAO.exec_SP("SP_MM001_PRICE_02");
                rtnmessage = DC010DAO.exec_SP("SP_MM001_PRICE_03");

                if (rtnmessage != "")
                {
                    return rtnmessage;
                }
                return "0";
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }  
  
}