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
/// WFB2IA0500Service 的摘要描述
/// </summary>
public class CFB2IA0500BO : BaseService
{
	public CFB2IA0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //新增 TB_I_M_GROUP_KIND
    public string addGROUP_KIND(CFB2IA0500DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.getExistData();

            if (tmp.Rows.Count > 0)
                return "團保對象別+團保險種+保險項目+人數_起 資料重覆!";
            else
            {
                BeginTransaction();
                wfb2ia.addGROUP_KIND();
                Commit();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //更新 TB_I_M_GROUP_KIND
    public string updateGROUP_KIND(CFB2IA0500DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateGROUP_KIND();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_I_M_GROUP_KIND
    public string deleteGROUP_KIND(List<Tuple<string, string, string, string>> target_type)
    {
        try
        {
            CFB2IA0500DAO wfb2ia = new CFB2IA0500DAO();
            string rtnmessage = "";

            foreach (var item in target_type)
            {
                //檢查是否已存在TB_I_M_GROUP_TXN資料
                //DataTable tmp = wfb2ia.getExistGINS_KIND(item.Item1,item.Item2);
                //if ((int)tmp.Rows[0]["txncount"] > 0)
                //{
                //    rtnmessage += "團保險種" + item.Item2 + "，該險種已被使用,不允刪除 \\n";
                //}

                //檢查是否該險種已計算過團保費用
                DataTable tmp =
                    wfb2ia.getExistGINS_KIND2(item.Item1, item.Item2, item.Item3, item.Item4);
                if ((int)tmp.Rows[0]["txncount"] > 0)
                {
                    rtnmessage += "團保險種" + item.Item2 + "，該險種已計算過團保費用,不允刪除 \\n";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in target_type)
                    {
                        wfb2ia.deleteGROUP_KIND(item.Item1, item.Item2, item.Item3, item.Item4);
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
}