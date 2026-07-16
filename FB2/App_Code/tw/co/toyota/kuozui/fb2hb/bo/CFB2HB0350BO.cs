using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HB0350BO 的摘要描述
/// </summary>
public class CFB2HB0350BO : BaseService
{
	public CFB2HB0350BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
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

    public DataTable getiniData(string emp_id, string hr_chg_no)
    {
        try
        {
            CFB2HB0350DAO hb035DAO = new CFB2HB0350DAO();
            return hb035DAO.getiniData(emp_id, hr_chg_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改
    public string update(CFB2HB0350DAO hb035DAO)
    {
        try
        {
            string rtnmessage = "0";

            BeginTransaction();
            hb035DAO.update();
            Commit();

            return rtnmessage;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}