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
/// WFB2PA0200Service 的摘要描述
/// </summary>
public class CFB2PA0200BO : BaseService
{
    public CFB2PA0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //取得修改資料
    public DataTable getUpdData(CFB2PA0200DAO dao)
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
    //更新 TB_P_M_PROPOSAL_DATA
    public string updateITEM(CFB2PA0200DAO wfb2pa)
    {
        try
        {
            BeginTransaction();

            wfb2pa.updateData();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_P_M_PROPOSAL_DATA
    public string deleteITEM(List<Tuple<string>> deletKey)
    {
        try
        {
            CFB2PA0200DAO wfb2pa = new CFB2PA0200DAO();
            BeginTransaction();
            foreach (var item in deletKey)
            {
                wfb2pa.deleteData(item.Item1);

                CFB2PA0100DAO pa0100Dao = new CFB2PA0100DAO();
                pa0100Dao.Insert_Log(SessionHandle.Current.emp_id, "FB2PA020", "刪除條碼編號 : "+item.Item1);
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
}