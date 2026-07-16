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
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class COMMBO : BaseService
{
    public COMMBO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
 
    # region Qry
    //取得公司代碼
    public DataTable getCOMPANY(string COMPANY_CD)
    {
        try
        {
            COMMDAO comm = new COMMDAO();
            return comm.getCOMPANY(COMPANY_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion
}