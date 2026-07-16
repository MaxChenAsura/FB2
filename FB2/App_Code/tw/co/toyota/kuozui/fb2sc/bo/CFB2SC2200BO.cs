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
/// CFB2SC2200BO 的摘要描述
/// </summary>
public class CFB2SC2200BO : BaseService
{
    public CFB2SC2200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getDetail1(string salary_type, string salary_ym, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail1Data(salary_type, salary_ym, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDetail2_duty(string salary_dt, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail2Data_duty(salary_dt, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDetail2_overtime(string salary_dt, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail2Data_overtime(salary_dt, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDetail2_leave(string salary_dt, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail2Data_leave(salary_dt, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDetail2_work(string salary_dt, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail2Data_work(salary_dt, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDetail2_available(string salary_dt, string emp_id)
    {
        try
        {
            CFB2SC2200DAO dao = new CFB2SC2200DAO();
            return dao.getDetail2Data_available(salary_dt, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }
}