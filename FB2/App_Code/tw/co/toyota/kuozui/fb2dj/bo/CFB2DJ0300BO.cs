using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DJ030BO 的摘要描述
/// </summary>
public class CFB2DJ0300BO : BaseService
{
	public CFB2DJ0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    /// <summary>
    /// 修改(加扣項) 
    /// </summary>
    /// <param name="dj030DAO"></param>
    /// <returns></returns>
    public string updateData(CFB2DJ0300DAO dj030DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            //DataTable dupdata = dj030DAO.getPKData();
            //if ((string)dupdata.Rows[0]["ENV_CHECK_STATUS"] !="E")
            //{
            //    rtnmessage += "未比對，無法進行加扣項處理";
            //}


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                   dj030DAO.updateData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //刪除
    public string deleteData(List<Tuple<string, string, string>> keysList, List<string> checkStatusList)
    {
        CFB2DJ0300DAO dj030DAO = new CFB2DJ0300DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查  是否已比對, 已使用則無法刪除
            int num = 0;
            foreach (var item in checkStatusList)
            {
                string checkStatus = item;
                if (item.Contains("E") || item.Contains("Y") )
                {
                    num += 1;
                }
            }
            if (num != 0)
            {
                rtnmessage += "有" + num + "筆 已比對資料，不可刪除 \\n";
            }


            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        dj030DAO.deleteData(item.Item1, item.Item2, item.Item3);
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