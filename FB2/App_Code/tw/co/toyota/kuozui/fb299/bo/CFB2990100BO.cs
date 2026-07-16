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
/// CFB2990100BO 的摘要描述
/// </summary>
public class CFB2990100BO : BaseService
{
    public CFB2990100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string MAIN_DESC { get; set; }
    public string USER_UPD { get; set; }
    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2990100DAO wfb299 = new CFB2990100DAO();
    //        wfb299.SYS_CD = sys_cd;
    //        return wfb299.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    public string deleteData(List<Tuple<string, string>> deleteList, List<string> user_updList)
    {
        try
        {
            CFB2990100DAO wfb299 = new CFB2990100DAO();
            foreach (string user_upd in user_updList)
            {
                if (user_upd == "N")
                    return "使用者可否異動為N，無法刪除!";
            }
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb299.deleteData(deleteitem.Item1,deleteitem.Item2);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string updateData(CFB2990100DAO fb299)
    {
        try
        {
            BeginTransaction();
            fb299.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2990100DAO fb299)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb299.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb299.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion

    #region Dtl

    public string deleteDtlData(List<string> deleteDtlList,string sys_cd, string main_cd)
    {
        try
        {
            CFB2990100DAO wfb299 = new CFB2990100DAO();
            foreach (string deleteDtlItem in deleteDtlList)
            {
                BeginTransaction();
                wfb299.deleteDtlData(deleteDtlItem, sys_cd, main_cd);
                Commit();
            }
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public DataTable getDtlHeader(string SYS_CD,string MAIN_CD)
    {
        try
        {
            CFB2990100DAO wfb299 = new CFB2990100DAO();
            wfb299.SYS_CD = SYS_CD;
            wfb299.MAIN_CD = MAIN_CD;
            return wfb299.getDtlHeader();
        }
        catch (Exception)
        {
            throw;
        }
    }
   
    public string updateDtlData(CFB2990100DAO fb299)
    {
        try
        {
            DataTable tmp = fb299.getExistDtlData();
            if (tmp.Rows.Count != 1)
            {
                return "資料已被其他使用者異動或刪除";
            }

            BeginTransaction();
            fb299.updateDtlData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addDtlData(CFB2990100DAO fb299)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb299.getExistDtlData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆";
            }
            BeginTransaction();
            fb299.addDtlData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion





}