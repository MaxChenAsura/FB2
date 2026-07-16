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
/// WFB2SJ0110Service 的摘要描述
/// </summary>
public class CFB2SJ0110BO : BaseService
{
    public CFB2SJ0110BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_ASSESS_ITEM
    public string addITEM(CFB2SJ0110DAO wfb2sj)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "考核類型+職種+資格+職務類型 資料重覆!";
           
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
    public DataTable getUpdData(CFB2SJ0110DAO dao)
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
    //更新 TB_S_M_ASSESS_ITEM
    public string updateITEM(CFB2SJ0110DAO wfb2sj)
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

    //刪除 TB_S_M_ASSESS_ITEM
    public string deleteITEM(List<Tuple<string, string, string, string>> deletKey)
    {
        try
        {
            CFB2SJ0110DAO wfb2sj = new CFB2SJ0110DAO();
            BeginTransaction();
            foreach (var item in deletKey)
            {
                wfb2sj.deleteITEM(item.Item1, item.Item2, item.Item3, item.Item4);
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