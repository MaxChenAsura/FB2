using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HB0200BO 的摘要描述
/// </summary>
public class CFB2HB0200BO : BaseService
{
	public CFB2HB0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getHR_CHG_CD()
    {
        try
        {
            CFB2HB0200DAO dao = new CFB2HB0200DAO();

            return dao.getHR_CHG_CD();
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public string getEmpName(string emp_id)
    {
        try
        {
            CFB2HB0400DAO dao = new CFB2HB0400DAO();
            DataTable dt = dao.getEmpName(emp_id);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }
}