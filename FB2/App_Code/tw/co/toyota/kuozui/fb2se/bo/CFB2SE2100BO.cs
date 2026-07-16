using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SE2100BO 的摘要描述
/// </summary>
public class CFB2SE2100BO : BaseService
{
	public CFB2SE2100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //檢查資料是否鎖定
    public string CheckADJ_H(string effect_ym)
    {
        string rtnmessage = "0";
        try
        {
            CFB2SE2100DAO fb2SA = new CFB2SE2100DAO();
            DataTable dt = fb2SA.get_SALARY_ADJ_H(effect_ym);
            if (!dt.Rows[0]["APPROVE_STATUS"].ToString().Equals("Y"))
            {
                rtnmessage = "此生效年月之調薪資料,核定狀態須為核可,才可執行此功能。 \\n";
                dt.Clear();
                return rtnmessage;
            }
            if (!dt.Rows[0]["MEM_CREATE_BY"].ToString().Equals(""))
            {
                rtnmessage = "此生效年月之不調薪對象已處理過,不允許重複執行。 \\n";
                dt.Clear();
                return rtnmessage;
            }
            else {
                dt.Clear();
                return rtnmessage;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //不調薪異動對象生成
    public string Process(string EFFECT_YM)
    {
        try
        {
            CFB2SE2100DAO fb2SE = new CFB2SE2100DAO();
            string vmess = "";
            BeginTransaction();
            vmess = fb2SE.Process_mark(EFFECT_YM);
            Commit();
            return vmess;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}