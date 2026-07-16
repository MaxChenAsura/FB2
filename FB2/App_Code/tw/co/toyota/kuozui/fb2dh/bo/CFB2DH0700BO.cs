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
/// CFB2DH0700BO 的摘要描述
/// </summary>
public class CFB2DH0700BO : BaseService
{
    public CFB2DH0700BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getSUB_LEAVE_CD(string main_leave_cd)
    {
        CFB2DH0700DAO wfb2dh = new CFB2DH0700DAO();
        try
        {
            return wfb2dh.getSUB_LEAVE_CD(main_leave_cd);
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
            CFB2DH0700DAO wfb2dh = new CFB2DH0700DAO();
            return wfb2dh.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            CFB2DH0700DAO wfb2dh = new CFB2DH0700DAO();
            return wfb2dh.getMAIN_LEAVE_DESC(main_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}