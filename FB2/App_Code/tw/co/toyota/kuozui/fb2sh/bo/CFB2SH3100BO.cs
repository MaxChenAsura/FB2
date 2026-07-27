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
/// WFB2SH3100Service 的摘要描述
/// </summary>
public class CFB2SH3100BO : BaseService
{
    public CFB2SH3100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_FOREIGN_AWARD
    public string addAwardITEM(CFB2SH3100DAO wfb2sh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sh.getExistAwardData();
            if (tmp.Rows.Count > 0)
                return "職務代號 資料重覆!";
           
                BeginTransaction();
                wfb2sh.addAwardITEM();
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
    public DataTable getUpdAwardData(CFB2SH3100DAO dao)
    {
        try
        {
            return dao.getUpdAwardData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_FOREIGN_AWARD
    public string updateAwardITEM(CFB2SH3100DAO wfb2sh)
    {
        try
        {
            BeginTransaction();

            wfb2sh.updateAwardITEM();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_FOREIGN_AWARD
    public string deleteITEM(List<Tuple<string,string>> deletKey)
    {
        try
        {
            CFB2SH3100DAO wfb2sh = new CFB2SH3100DAO();
            BeginTransaction();
            foreach (var item in deletKey)
            {
                wfb2sh.deleteAwardITEM(item.Item1,item.Item2);
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

    //新增 TB_S_M_FR_BASEBONUS
    public string addBaseBounsITEM(CFB2SH3100DAO wfb2sh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sh.getExistBaseBounsData();
            if (tmp.Rows.Count > 0)
                return "職務代號 資料重覆!";

            BeginTransaction();
            wfb2sh.addBaseBounsITEM();
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
    public DataTable getUpdBaseBounsData(CFB2SH3100DAO dao)
    {
        try
        {
            return dao.getUpdBaseBounsData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_FR_BASEBONUS
    public string updateBaseBounsITEM(CFB2SH3100DAO wfb2sh)
    {
        try
        {
            BeginTransaction();

            wfb2sh.updateBaseBounsITEM();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_FR_BASEBONUS
    public string deleteBaseBounsITEM(List<Tuple<string,string>> deletKey)
    {
        try
        {
            CFB2SH3100DAO wfb2sh = new CFB2SH3100DAO();
            BeginTransaction();
            foreach (var item in deletKey)
            {
                wfb2sh.deleteBaseBounsITEM(item.Item1,item.Item2);
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
    public DataTable getPJOB_NAME(string pjob_cd)
    {
        try
        {
            CFB2SH3100DAO wfb2sh = new CFB2SH3100DAO();
            return wfb2sh.getPJOB_NAME(pjob_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}