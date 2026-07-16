using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SH010BO 的摘要描述
/// </summary>
public class CFB2HA0800BO : BaseService
{
	public CFB2HA0800BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    //新增
    public string insertData(CFB2HA0800DAO ha080DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = ha080DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "新舊職務代號 重覆\\n";
            }
            //01檢查新部門代號是否存在
            dupdata = ha080DAO.getPjobDate();
            if ((int)dupdata.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "新職務代號不存在\\n";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    ha080DAO.insertData();

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

    //修改
    public string updateData(CFB2HA0800DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dao.updateData();

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
    public string deleteData(List<Tuple<string, string>> keysList)
    {
        CFB2HA0800DAO ha080DAO = new CFB2HA0800DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        ha080DAO.TPJOBO = item.Item1;
                        ha080DAO.TPJOBN = item.Item2;
                        ha080DAO.deleteData();
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