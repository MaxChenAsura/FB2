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
/// CFB2SL3100BO 的摘要描述
/// </summary>
public class CFB2SL3100BO : BaseService
{
	public CFB2SL3100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable get_PDF_Data(string data_ym, List<Tuple<string, string, string, string, string, string>> emp_data)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SL3100DAO fb2sl = new CFB2SL3100DAO();
        try
        {
            BeginTransaction();
            retVal = fb2sl.get_PDF_Data(data_ym, emp_data[0].Item1, emp_data[0].Item6);
            Commit();
            return retVal;
            //retVal = fb2sl.get_PDF_Data(data_ym, item.Item1);
            //return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get_PDF_Data2(string data_ym, string salary_dt1, string salary_dt2, List<Tuple<string, string, string, string, string, string>> emp_data, string sdt, string edt)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SL3100DAO fb2sl = new CFB2SL3100DAO();
        
        try
        {
            BeginTransaction();
            retVal = fb2sl.get_PDF_Data2(data_ym, salary_dt1, salary_dt2, emp_data[0].Item1, sdt, edt, emp_data[0].Item3);
            Commit();
            return retVal;
            //retVal = fb2sl.get_PDF_Data2(data_ym, salary_dt1, salary_dt2, item.Item1);
            //return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEmpName(string emp_id, string value)
    {
        try
        {
            CFB2SL3100DAO dao = new CFB2SL3100DAO();
            if (value == "1")
                return dao.getEmpName(emp_id);
            else
                return dao.getVENDOR_MEMBER_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSendToEmail()
    {
        try
        {
            CFB2SL3100DAO dao = new CFB2SL3100DAO();
            return dao.getSendToEmail();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEmailContent()
    {
        try
        {
            CFB2SL3100DAO dao = new CFB2SL3100DAO();
            return dao.getEmailContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}