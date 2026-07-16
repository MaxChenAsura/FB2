using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using iTextSharp.text;

/// <summary>
/// CFB2SA1300BO 的摘要描述
/// </summary>
public class CFB2SA1300BO : BaseService
{
	public CFB2SA1300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public string Release(string DATA_YEAR,string START_DT,string END_DT)
    {
        try
        {
            CFB2SA1300DAO fb2sa = new CFB2SA1300DAO();
            BeginTransaction();
            fb2sa.Update_TB_S_HIRING_SALARY_TMP_H(DATA_YEAR, START_DT, END_DT);
            fb2sa.Update_TB_S_HIRING_SALARY_TMP_D(DATA_YEAR);
            fb2sa.Update_TB_S_HIRING_SALARY_SET(DATA_YEAR);
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