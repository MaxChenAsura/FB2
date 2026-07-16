using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
/// <summary>
/// CFB2HB0800BO 的摘要描述
/// </summary>
public class CFB2HB0800BO : BaseService
{
    public CFB2HB0800BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
   
    #region gv_result新刪修
    public string Add(CFB2HB0800DAO fb2hb)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2hb.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "資料重複!";
            }
            else
            {
                fb2hb.addEmpLanguage();
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
    public string Update(CFB2HB0800DAO fb2hb)
    {
        try
        {
            BeginTransaction();
            fb2hb.updateEmpLanguage();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete(List<Tuple<string>> keysList)
    {
        CFB2HB0800DAO fb2hb = new CFB2HB0800DAO();
        try
        {
            foreach (var item in keysList)
            {
                
                BeginTransaction();
                fb2hb.Delete(item.Item1);
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
    public DataTable getUpdData(CFB2HB0800DAO dao)
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
    

   
}