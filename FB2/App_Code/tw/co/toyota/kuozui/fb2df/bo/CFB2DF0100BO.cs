using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DF0100BO 的摘要描述
/// </summary>
public class CFB2DF0100BO : BaseService
{
	public CFB2DF0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string delete_BaseNO(List<string> BASE_NO)
    {
        CFB2DF0100DAO dao = new CFB2DF0100DAO();
        string rtnmessage = "";
        try
        {
            foreach (string item in BASE_NO)
            {
                //檢查是否已存在住宿主檔
                DataTable tmp = dao.getUsedBaseNo(item);
                if ((int)tmp.Rows[0]["basecount"] > 0)
                {
                    rtnmessage += "此住宿費基準" + item + "資料正在使用中，無法刪除 \\n";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (string item in BASE_NO)
                    {
                        dao.deleteBaseNo(item);
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
                return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string addBASE_NO(CFB2DF0100DAO dao)
    {
        try
        {

            DataTable dt = dao.getExistBaseNo(dao.BASE_NO);
            BeginTransaction();

            //如資料存在, 則更新資料，如資料不存在, 則新增資料
            if (dt.Rows.Count > 0)
                dao.updateBaseNo(dao);
            else
                dao.addBaseNo(dao);

            Commit();

            return "0";


        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateBASE_NO(CFB2DF0100DAO dao)
    {
        try
        {
            BeginTransaction();
            //(1)更新 住宿費基準檔
            dao.updateBaseNo(dao);
            //(2)更新 住宿主檔
            dao.updateAccom(dao);

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}