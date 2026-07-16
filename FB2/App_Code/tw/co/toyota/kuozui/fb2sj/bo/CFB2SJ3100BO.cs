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
/// WFB2SJ3100Service 的摘要描述
/// </summary>
public class CFB2SJ3100BO : BaseService
{
    public CFB2SJ3100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_FOREIGN_DISTING
    public string addITEM(CFB2SJ3100DAO wfb2sj)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "區分代碼 資料重覆!";
           
                BeginTransaction();
                wfb2sj.addITEM();
                Commit();
           

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //取得修改資料
    public DataTable getUpdData(CFB2SJ3100DAO dao)
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
    //更新 TB_S_M_FOREIGN_DISTING
    public string updateITEM(CFB2SJ3100DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateITEM();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_FOREIGN_DISTING
    public string deleteITEM(List<Tuple<string>> deletKey)
    {
        try
        {
            CFB2SJ3100DAO wfb2sj = new CFB2SJ3100DAO();
            BeginTransaction();
            foreach (var item in deletKey)
            {
                wfb2sj.deleteITEM(item.Item1);
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