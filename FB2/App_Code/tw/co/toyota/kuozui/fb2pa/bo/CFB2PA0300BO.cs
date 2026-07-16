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
/// WFB2PA0300Service 的摘要描述
/// </summary>
public class CFB2PA0300BO : BaseService
{
    public CFB2PA0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
   
    //更新 資料
    public string update(CFB2PA0300DAO wfb2pa)
    {
        try
        {
            BeginTransaction();

            wfb2pa.insertCloseYm();
            wfb2pa.updateProposalData();

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