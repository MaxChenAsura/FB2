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
/// CFB2SC1200BO 的摘要描述
/// </summary>
public class CFB2SC1200BO : BaseService
{
    public CFB2SC1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string KIND_CD { get; set; }
    public string GROUP_ID { get; set; }
    public string GROUP_NAME { get; set; }
    public string CLASSIFY { get; set; }
    public string ORDER_SEQ { get; set; }
    public string GROUP_TYPE { get; set; }
   
    # region Qry
    public string deleteData(List<Tuple<string, string, string>> deleteList)
    {
        try
        {
            CFB2SC1200DAO wfb2sc = new CFB2SC1200DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2sc.deleteData(deleteitem.Item1,deleteitem.Item2,deleteitem.Item3);
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
    public string addData(CFB2SC1200DAO fb2sc)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2sc.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb2sc.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string updateData(CFB2SC1200DAO fb2sc)
    {
        try
        {
            BeginTransaction();
            fb2sc.updateData();
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

    #region "Detail 1"
    public string deleteDtlData(string kind_cd, string group_type, string group_id)
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            BeginTransaction();
            string msg = dao.deleteDtlData(kind_cd, group_type, group_id);
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string addDtlData(string kind_cd, string group_type, string group_id, string selectedItem)
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            foreach (string selectedSub_group_ID in selectedItem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                BeginTransaction();
                dao.addDtlData(kind_cd, group_type, group_id, selectedSub_group_ID);
                Commit();
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion

    #region "Detail 2"

    public string deleteDtlData2(List<Tuple<string, string, string, string, string>> deleteDtlList)
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            BeginTransaction();
            foreach (var deleteDtlItem in deleteDtlList)
            {
                dao.deleteDtlData2(deleteDtlItem.Item1,deleteDtlItem.Item2,deleteDtlItem.Item3,deleteDtlItem.Item4,deleteDtlItem.Item5);
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

    public string updateDtlData2(CFB2SC1200DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.updateDtlData2();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addDtlData2(CFB2SC1200DAO dao)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistDtlData2();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆";
            }
            BeginTransaction();
            dao.addDtlData2();
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