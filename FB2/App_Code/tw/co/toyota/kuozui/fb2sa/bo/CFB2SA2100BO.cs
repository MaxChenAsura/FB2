using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2SA2100BO 的摘要描述
/// </summary>
public class CFB2SA2100BO : BaseService
{
    public CFB2SA2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getAllSALARY_ID()
    {
        CFB2SA2100DAO dao = new CFB2SA2100DAO();
        try
        {
            return dao.getAllSALARY_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARY_ID(string emp_id)
    {
        CFB2SA2100DAO dao = new CFB2SA2100DAO();
        try
        {
            return dao.getSALARY_ID(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpData(string emp_id)
    {
        CFB2SA2100DAO dao = new CFB2SA2100DAO();
        try
        {
            return dao.getEMPData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDetailFromSALARY_TXN(CFB2SA2100DAO dao)
    {
        try
        {
            if (dao.PROCESS_STATUS == "Y")
                return dao.getDetailFromSALARY_TXN();
            else
                return dao.getDetailFromSALARY_TXN_TMP();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int checkSALARY_TXN_duplicate(CFB2SA2100DAO dao)
    {
        int retVal = 0;
        try
        {
            retVal = dao.checkSALARY_TXN_duplicate(dao.EMP_ID,dao.SALARY_ID,dao.EFFECT_SDT_A,dao.EFFECT_EDT_A);

            return retVal;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void insertSALARY_TXN_TMP(CFB2SA2100DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.insertSALARY_TXN_TMP();
            Commit();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateSALARY_TXN_TMP(CFB2SA2100DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.updateSALARY_TXN_TMP();
            Commit();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteSALARY_TXN_TMP(CFB2SA2100DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.deleteSALARY_TXN_TMP();
            Commit();
        }
        catch (Exception)
        {

            throw;
        }
    }


}