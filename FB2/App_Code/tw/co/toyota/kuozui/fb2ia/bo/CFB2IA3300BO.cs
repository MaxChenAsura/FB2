using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2IA3300BO 的摘要描述
/// </summary>
public class CFB2IA3300BO : BaseService
{
	public CFB2IA3300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string Add(CFB2IA3300DAO fb2ia)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2ia.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                
                return "資料重複!";
            }
            else
            {
                fb2ia.Add();
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
    public string Update(CFB2IA3300DAO fb2ia)
    {
        try
        {
            BeginTransaction();
            fb2ia.Update();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete(List<string> delitem_list, List<string> APPROVE_STATUS_list)
    {
        CFB2IA3300DAO fb2ia = new CFB2IA3300DAO();
        try
        {
            for (int i = 0; i < APPROVE_STATUS_list.Count; i++)
            {
                string APPROVE_STATUS = APPROVE_STATUS_list[i];
                string delitem = delitem_list[i];
                BeginTransaction();
                //if (APPROVE_STATUS=="Y")
                //{
                //    return "主管已核定,不允刪除";
                //}
                //else
                //{
                    fb2ia.Delete(delitem);
                //}
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
}