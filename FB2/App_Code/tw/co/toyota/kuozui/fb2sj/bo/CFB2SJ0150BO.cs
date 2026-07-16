using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
/// <summary>
/// CFB2SJ0150BO 的摘要描述
/// </summary>
public class CFB2SJ0150BO : BaseService
{
    public CFB2SJ0150BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
   
    #region gv_result新刪修
    public string Add(CFB2SJ0150DAO fb2sj)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sj.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "資料重複!";
            }
            else
            {
                fb2sj.addGROUP_H();
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
    public string Update(CFB2SJ0150DAO fb2sj)
    {
        try
        {
            BeginTransaction();
            fb2sj.updateGROUP_H();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete(List<Tuple<string, string, string>> keysList)
    {
        CFB2SJ0150DAO fb2sj = new CFB2SJ0150DAO();
        try
        {
            foreach (var item in keysList)
            {
                
                BeginTransaction();
                fb2sj.Delete(item.Item1, item.Item2, item.Item3);
                fb2sj.DeleteDtl(item.Item1, item.Item2, item.Item3, "");
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得修改資料
    public DataTable getUpdData(CFB2SJ0150DAO dao)
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
    #endregion
    #region gv_result2新刪修
    public string Add_Dtl(CFB2SJ0150DAO fb2sj)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sj.getExistDataDtl();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "資料重複!";
            }
            else
            {
                fb2sj.addGROUP_D();
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
    public string DeleteDtl(List<Tuple<string, string, string, string>> keysList)
    {
        CFB2SJ0150DAO fb2sj = new CFB2SJ0150DAO();
        try
        {
            foreach (var item in keysList)
            {

                BeginTransaction();
                fb2sj.DeleteDtl(item.Item1, item.Item2, item.Item3, item.Item4);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
    //取得修改資料
    public DataTable getLevelData()
    {
        CFB2SJ0150DAO fb2sj = new CFB2SJ0150DAO();
        try
        {
            return fb2sj.getLevelData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getGroupH(String assess_year, String assess_type, String ws_cd, String grp_cd)
    {
        CFB2SJ0150DAO fb2sj = new CFB2SJ0150DAO();
        try
        {
            return fb2sj.getGroupH( assess_year,  assess_type,  ws_cd,  grp_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getNoSetDtlGroupH(String assess_year, String assess_type)
    {

        CFB2SJ0150DAO fb2sj = new CFB2SJ0150DAO();
        try
        {
            return fb2sj.getNoSetDtlGroupH(assess_year, assess_type);
        }
        catch (Exception)
        {
            throw;
        }
        

    }
}