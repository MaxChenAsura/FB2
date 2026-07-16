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
/// CFB2SA2200BO 的摘要描述
/// </summary>
public class CFB2SA1400BO : BaseService
{
    public CFB2SA1400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getHIRING_SALARY_TMP_HData(string data_year)
    {
        CFB2SA1400DAO dao = new CFB2SA1400DAO();
        try
        {
            return dao.getHIRING_SALARY_TMP_HData(data_year);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void approveHIRING_SALARY(CFB2SA1400DAO dao)
    {
        try
        {
            dao.APPROVE_STATUS = "Y";
            dao.PROCESS_STATUS = "Y";
            dao.APPROVE_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SA140";

            BeginTransaction();
            dao.updateHIRING_SALARY_TMP_H();
            dao.insertHIRING_SALARY();
            dao.updateHIRING_SALARY_EFFECT_EDT();
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public void rejectHIRING_SALARY(CFB2SA1400DAO dao, List<CFB2SA1400DAO> dao1, List<CFB2SA1400DAO> dao2)
    {
        try
        {
            dao.APPROVE_STATUS = "B";
            dao.PROCESS_STATUS = "N";
            dao.APPROVE_BY = "";
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SA140";

            BeginTransaction();
            dao.updateHIRING_SALARY_TMP_H();

            for (int i = 0; i < dao1.Count; i++)
            {
                dao1[i].updateHIRING_SALARY_TMP_D();
            }

            for (int i = 0; i < dao2.Count; i++)
            {
                dao2[i].updateHIRING_SALARY_SET();
            }
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
}