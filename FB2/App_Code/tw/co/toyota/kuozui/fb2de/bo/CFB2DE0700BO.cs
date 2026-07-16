using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DE0700BO 的摘要描述
/// </summary>
public class CFB2DE0700BO : BaseService
{
    public CFB2DE0700BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getMaxDT()
    {
        try
        {
            CFB2DE0700DAO dao = new CFB2DE0700DAO();
            return dao.getMaxDT();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEMPData(string EMP_ID)
    {
        try
        {
            CFB2DE0700DAO dao = new CFB2DE0700DAO();
            return dao.getEMPData(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getTotalAmount(string EMP_ID, Boolean b1, Boolean b2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {
            CFB2DE0700DAO dao = new CFB2DE0700DAO();
            return dao.getTotalAmount(EMP_ID, b1, b2, WORK_DT, MANAGER_DT_S, MANAGER_DT_E);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEveryCount(string EMP_ID, Boolean b1, Boolean b2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {
            CFB2DE0700DAO dao = new CFB2DE0700DAO();
            return dao.getEveryCount(EMP_ID, b1, b2, WORK_DT, MANAGER_DT_S, MANAGER_DT_E);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getALLCLOCKData()
    {
        try
        {
            CFB2DE0600DAO dao = new CFB2DE0600DAO();
            return dao.getALLCLOCKData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string addClockMoney(CFB2DE0600DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.insertCLOCKMONEY();
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateCLOCK_MONEY(CFB2DE0600DAO dao)
    {
        try
        {
            BeginTransaction();

            dao.updateCLOCK_MONEY();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除資料
    public string deleteData(List<string> CLOCK_NOS)
    {
        CFB2DE0600DAO dao = new CFB2DE0600DAO();
        try
        {
            BeginTransaction();

            foreach (string CLOCK_NO in CLOCK_NOS)
            {
                dao.CLOCK_NO = CLOCK_NO;
                //刪除主檔資料
                dao.deleteData(CLOCK_NO);
            }
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }


    }

    //刪除資料
    /*
    public string deleteData(string EMP_ID,string APPLICATION_NO,string flag)
    {
        CFB2DD0100DAO dao = new CFB2DD0100DAO();
        try
        {
            BeginTransaction();
            if (flag != "1")
            {
                //更新第二筆資料.生效日迄 = 9999/12/31
                dao.updateData(EMP_ID);

                //更新主檔資料
                dao.updateMain(EMP_ID);
            }
            else { 
                //刪除主檔資料 
                //先不刪除
                //dao.delMain(EMP_ID);
            }
            
            //刪除第一筆資料
            dao.delData(APPLICATION_NO, EMP_ID);

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }
    */

    

}