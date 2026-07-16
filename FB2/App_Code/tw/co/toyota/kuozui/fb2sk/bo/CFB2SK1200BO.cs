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
/// CFB2SK1200BO 的摘要描述
/// </summary>
public class CFB2SK1200BO : BaseService
{
    public CFB2SK1200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable get_MUTUAL_Data()
    {
        DataTable retVal = new DataTable(); ;
        CFB2SK1200DAO dao = new CFB2SK1200DAO();
        try
        {
            BeginTransaction();
            retVal = dao.get_MUTUAL_Data();
            Commit();
            return retVal;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get_PDF_Data(string data_ym, List<Tuple<string, string, string, string, string>> emp_data)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SK1200DAO dao = new CFB2SK1200DAO();
        try
        {
            BeginTransaction();
            retVal = dao.get_PDF_Data(data_ym, emp_data[0].Item3);
            Commit();
            return retVal;
           
        }
        catch (Exception)
        {
            throw;
        }
    }
        
    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2SK1200DAO dao = new CFB2SK1200DAO();
            return dao.getEmpName(emp_id);
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
            CFB2SK1200DAO dao = new CFB2SK1200DAO();
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
            CFB2SK1200DAO dao = new CFB2SK1200DAO();
            return dao.getEmailContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}