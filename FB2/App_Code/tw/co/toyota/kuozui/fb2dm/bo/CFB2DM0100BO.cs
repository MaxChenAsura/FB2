using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DM0100BO 的摘要描述
/// </summary>
public class CFB2DM0100BO : BaseService
{
	public CFB2DM0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getSalaryDT(string salary_ym)
    {
        CFB2DM0100DAO wfb2dm = new CFB2DM0100DAO();
        try
        {
            return wfb2dm.getSalaryDT(salary_ym);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string callSP(CFB2DM0100DAO dao)
    {
        
        try
        {

            return dao.SP_D_DUTY_CLOSE();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getSalaryCTL(string salary_dt)
    {
        CFB2DM0100DAO wfb2dm = new CFB2DM0100DAO();
        try
        {
            return wfb2dm.getSalaryCTL(salary_dt);
        }
        catch (Exception)
        {
            throw;
        }
    }
}