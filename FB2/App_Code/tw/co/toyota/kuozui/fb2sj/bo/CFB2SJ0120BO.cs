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
/// WFB2SJ0120Service 的摘要描述
/// </summary>
public class CFB2SJ0120BO : BaseService
{
    public CFB2SJ0120BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_ASSESS_DISTING
    public string addDISTING(CFB2SJ0120DAO wfb2sj)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "考核區分代碼 資料重覆!";
           
                BeginTransaction();
                wfb2sj.addDISTING();
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
    public DataTable getUpdData(CFB2SJ0120DAO dao)
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
    //更新 TB_S_M_ASSESS_DISTING
    public string updateDISTING(CFB2SJ0120DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateDISTING();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_ASSESS_DISTING
    public string deleteDISTING(List<Tuple<string>> disting_cd)
    {
        try
        {
            CFB2SJ0120DAO wfb2sj = new CFB2SJ0120DAO();
            BeginTransaction();
            foreach (var item in disting_cd)
            {
                wfb2sj.deleteDISTING(item.Item1);
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