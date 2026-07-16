using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DJ010BO 的摘要描述
/// </summary>
public class CFB2DJ0100BO : BaseService
{
	public CFB2DJ0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //查詢條件的環境津貼等級
    public DataTable getEnvType()
    {
        CFB2DJ0100DAO dj010DAO = new CFB2DJ0100DAO();
        try
        {
            return dj010DAO.getEnvType();
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    //新增
    public string insertData(CFB2DJ0100DAO dj010DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            //1.檢查PK值有無重覆
            dt = dj010DAO.getPKDataCount();
            if ((int)dt.Rows[0]["typecount"] > 0)
            {
                rtnmessage += "環境津貼等級+生效日期 重覆 \\n";
            }

             //2.檢查生效日期: 是否在相同的津貼等級生效起迄之間
            // 輸入的生效日<=max(結束日) => 不可新增
             DateTime start_dt = DateTime.Parse(dj010DAO.START_DT);
             dt = dj010DAO.getMaxEndDTByType();
             //int tt = dt.Rows.Count;
             //if (dt.DataSet != null)
             if (dt.Rows[0]["maxEndDT"].ToString() != "")
             {
                 DateTime maxStartDT = (DateTime)dt.Rows[0]["maxEndDT"];
                 if (start_dt <= maxStartDT)
                 {
                     rtnmessage += "與相同津貼等級的生效期間重疊\\n";
                 }
             }


            //3.檢查結束日期: 是否有大於系統日
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));

            if (!dj010DAO.END_DT.Trim().Equals(""))
            {
                DateTime end_dt = DateTime.Parse(dj010DAO.END_DT);
                if (end_dt < now)
                {
                    rtnmessage += "結束日期不得小於系統日 \\n";
                }

                if (start_dt > end_dt)
                {
                    rtnmessage += "結束日期不得小於生效日期 \\n";
                }
            }

            //4.檢查生效日期: 是否有大於系統日(因環境津貼是月結,故可以回溯)
            //if (start_dt < now)
            //{
            //    rtnmessage += "生效日期不得小於系統日 \\n";
            //}


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dj010DAO.insertData();

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
    public string updateData(CFB2DJ0100DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查
           //DateTime start_dt = DateTime.Parse(dj010DAO.START_DT);
            DateTime end_dt = DateTime.Parse(dao.END_DT);
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));

            if (end_dt < now){
                rtnmessage += "結束日期不可小於系統日 \\n";
            }

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
    public string deleteData(List<Tuple<string, string>> keysList, List<Tuple<string, string, string>> checkDataList)
    {
        CFB2DJ0100DAO dj010DAO = new CFB2DJ0100DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            foreach (var item in checkDataList)
            {
                //檢查 環境津貼申請資料檔 是否已使用, 已使用則無法刪除
                DataTable tmp =  dj010DAO.getExistType(item.Item1, item.Item2, item.Item3);
                if ((int)tmp.Rows[0]["typecount"] > 0)
                {
                    rtnmessage += "環境津貼等級層級" + item.Item1 + "，已存在環境津貼申請資料，不可刪除 \\n";
                }
            }



            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        dj010DAO.deleteData(item.Item1, item.Item2);
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