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
/// CFB2990200BO 的摘要描述
/// </summary>
public class CFB2990200BO : BaseService
{
	public CFB2990200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
 
    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2990200DAO wfb299 = new CFB2990200DAO();
    //        wfb299.SYS_CD = sys_cd;
    //        return wfb299.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    public string deleteData(List<Tuple<string, string>> deleteList)
    {
        try
        {
            CFB2990200DAO wfb299 = new CFB2990200DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb299.deleteData(deleteitem.Item1, deleteitem.Item2);
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
    public string updateData(CFB2990200DAO fb299)
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
    public string addData(CFB2990200DAO fb299)
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
}