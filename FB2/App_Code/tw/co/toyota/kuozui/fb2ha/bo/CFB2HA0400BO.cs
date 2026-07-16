using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HA0400BO 的摘要描述
/// </summary>
public class CFB2HA0400BO : BaseService
{
	public CFB2HA0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getLevelCD(string start_dt)
    {
        CFB2HA0400DAO wfb2ha = new CFB2HA0400DAO();
        try
        {
            return wfb2ha.getLevelCD(start_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable check_LEVEL_CD(string LEVEL_CD)
    {
        CFB2HA0400DAO wfb2ha = new CFB2HA0400DAO();
        try
        {
            return wfb2ha.check_LEVEL_CD(LEVEL_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    


   
}