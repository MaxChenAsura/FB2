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
/// CFB2SC5500BO 的摘要描述
/// </summary>
public class CFB2SC5500BO : BaseService
{
	public CFB2SC5500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getJPN_CD()
    {
        CFB2SC5500DAO wfb2sc = new CFB2SC5500DAO();
        try
        {
            return wfb2sc.getJPN_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
   
    public string getLast_SALARY_YM()
    {
        string retVal = "";
        CFB2SC5500DAO fb2sc = new CFB2SC5500DAO();
        try
        {
            retVal = fb2sc.getLast_SALARY_YM();

            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable get_PDF_Data(string SALARY_YM)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SC5500DAO fb2sc = new CFB2SC5500DAO();
        try
        {
            retVal = fb2sc.get_PDF_Data(SALARY_YM);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}
