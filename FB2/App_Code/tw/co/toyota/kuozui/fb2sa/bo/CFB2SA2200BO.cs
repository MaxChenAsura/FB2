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
public class CFB2SA2200BO : BaseService
{
    public CFB2SA2200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public void approveSALARY_TXN(List<CFB2SA2200DAO> dao)
    {
        try
        {
            BeginTransaction();
            int tt = dao.Count;
            for (int i = 0; i < dao.Count; i++)
            {               
                if (dao[i].CHG_STATUS == "N")
                {
                    dao[i].updateSALARY_TXN_EFFECT_EDT();
                    dao[i].insertSALARY_TXN();
                }
                else if (dao[i].CHG_STATUS == "U")
                    dao[i].updateSALARY_TXN();
                else if (dao[i].CHG_STATUS == "D")
                    dao[i].deleteSALARY_TXN();

                dao[i].updateSALARY_TXN_TMP();
            }
            Commit();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void rejectSALARY_TXN(List<CFB2SA2200DAO> dao)
    {
        try
        {
            BeginTransaction();
            for (int i = 0; i < dao.Count; i++)
            {
                dao[i].updateSALARY_TXN_TMP();
            }
            Commit();
        }
        catch (Exception)
        {

            throw;
        }
    }
}