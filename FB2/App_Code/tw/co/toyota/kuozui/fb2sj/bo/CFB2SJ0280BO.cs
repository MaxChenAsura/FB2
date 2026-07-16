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
/// WFB2SJ0280Service 的摘要描述
/// </summary>
public class CFB2SJ0280BO : BaseService
{
    public CFB2SJ0280BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //取得修改資料
    public DataTable getUpdData(CFB2SJ0280DAO dao)
    {
        try
        {
            return dao.getUpdData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 TB_S_M_ASSESS_MA_PEO
    public string updateData(CFB2SJ0280DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateData();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    
}