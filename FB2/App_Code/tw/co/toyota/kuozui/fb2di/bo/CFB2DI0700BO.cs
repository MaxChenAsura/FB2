using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DI0700BO 的摘要描述
/// </summary>
public class CFB2DI0700BO : BaseService
{
	public CFB2DI0700BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    public System.Data.DataTable getTotalOvertimeData(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getTotalOvertimeData(emp_id,date1,date2,overtime_dt_ym,overtime_dt_s,overtime_dt_e);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /*
    public System.Data.DataTable getTotalOvertimeData_CTL(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getTotalOvertimeData_CTL(emp_id, date1, date2, overtime_dt_ym, overtime_dt_s, overtime_dt_e);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */

    public System.Data.DataTable getOvertimeCtlData(string emp_id)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getOvertimeCtlData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLeaveData(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getLeaveData(emp_id, date1, date2, overtime_dt_ym, overtime_dt_s, overtime_dt_e);
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
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getTOTAL_TIME_OVERTIME_IFLOW(string emp_id, string overtime_cd, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            return dao.getTOTAL_TIME_OVERTIME_IFLOW(emp_id, overtime_cd, date1, date2, overtime_dt_ym, overtime_dt_s, overtime_dt_e);
        }
        catch (Exception)
        {

            throw;
        }
    }
}