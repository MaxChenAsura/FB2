using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SE1300BO 的摘要描述
/// </summary>
public class CFB2SE1300BO : BaseService
{
	public CFB2SE1300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    public string updateData(CFB2SE1300DAO fb2se)
    {
        try
        {
            BeginTransaction();
            fb2se.updateData();
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