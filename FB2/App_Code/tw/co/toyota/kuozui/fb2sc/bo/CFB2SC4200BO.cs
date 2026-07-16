using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SC4200BO 的摘要描述
/// </summary>
public class CFB2SC4200BO : BaseService
{
	public CFB2SC4200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    #region "grid1"
    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteData(deleteitem);
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
    public string updateData(CFB2SC4200DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2SC4200DAO dao)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            dao.addData();
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

    #region "grid2"
    public string deleteDataOwe(List<string> deleteList, string emp_id, string debit_dt, int amount)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除明細資料
                dao.deleteDataOwe(deleteitem);
            }
            //update 欠薪主檔
            dao.updateARREARS_H(emp_id, debit_dt, amount);

            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string addDataOwe1(CFB2SC4200DAO dao)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistDataOwe();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            dao.addDataOwe1();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string addDataOwe(CFB2SC4200DAO dao)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistDataOwe();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            dao.addDataOwe();
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

    #region "grid3"
    public string deleteDataRepay(List<string> deleteList)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteDataRepay(deleteitem);
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
    public string updateDataRepay(CFB2SC4200DAO dao)
    {
        try
        {
            BeginTransaction();
            dao.updateDataRepay();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addDataRepay(CFB2SC4200DAO dao)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistDataRepay();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            dao.addDataRepay();
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