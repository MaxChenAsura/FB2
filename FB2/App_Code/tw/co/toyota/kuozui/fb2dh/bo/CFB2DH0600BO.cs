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
/// CFB2DH0600BO 的摘要描述
/// </summary>
public class CFB2DH0600BO : BaseService
{
    public CFB2DH0600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getMAIN_LEAVE_CD(string main_leave_cd)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getMAIN_LEAVE_CD(main_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSUB_LEAVE_CD(string main_leave_cd, string sub_leave_cd)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getSUB_LEAVE_CD(main_leave_cd, sub_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEAVE_TIME_UNIT(string leave_time_unit)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getLEAVE_TIME_UNIT(leave_time_unit);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getMAY_REST_TIMES_M(string emp_id, string apply_leave_dt)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getMAY_REST_TIMES_M(emp_id, apply_leave_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getMAY_REST_TIMES_Y(string emp_id, string apply_leave_dt_y)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getMAY_REST_TIMES_Y(emp_id, apply_leave_dt_y);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getMAY_REST_TIMES_Y2(string emp_id, string apply_leave_dt_y, string main_leave_cd, string sub_leave_cd)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            return wfb2dh.getMAY_REST_TIMES_Y2(emp_id, apply_leave_dt_y, main_leave_cd, sub_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool getLEAVE_MAX_DAY_CD(string main_leave_cd, string sub_leave_cd)
    {
        CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
        try
        {
            DataTable dt = wfb2dh.getLEAVE_MAX_DAY_CD(main_leave_cd, sub_leave_cd);
            if (dt.Rows.Count > 0)
                return true;

            return false;
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
            CFB2DH0600DAO wfb2dh = new CFB2DH0600DAO();
            return wfb2dh.getEmpName(emp_id);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public DataTable getOvertimeData(string emp_id,string ym)
    {

        try
        {
            CFB2DH0600DAO dao = new CFB2DH0600DAO();
            return dao.getOvertimeData(emp_id, ym);

        }
        catch (Exception)
        {

            throw;
        }
    }
}