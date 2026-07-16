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
/// CFB2992100BO 的摘要描述
/// </summary>
public class CFB2992100BO : BaseService
{
	public CFB2992100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    
    public string addAS400Data()
    {
        try
        {
            CFB2992100DAO wfb299 = new CFB2992100DAO();
            DataTable dt = wfb299.getAS400Data();
            if (dt.Rows.Count > 0){
                
            }
            return "0";
        }
        catch (Exception ex)
        {
            
            return ex.Message;
        }
    }
}