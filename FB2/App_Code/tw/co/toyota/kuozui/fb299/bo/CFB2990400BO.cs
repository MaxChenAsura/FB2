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
/// CFB2990400BO 的摘要描述
/// </summary>
public class CFB2990400BO : BaseService
{
	public CFB2990400BO()
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
    //        CFB2990400DAO wfb299 = new CFB2990400DAO();
    //        wfb299.SYS_CD = sys_cd;
    //        return wfb299.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getSYS_ID()
    {
        CFB2990400DAO wfb299 = new CFB2990400DAO();
        try
        {
            return wfb299.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2990400DAO wfb299 = new CFB2990400DAO();
        try
        {
            return wfb299.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2990400DAO wfb299 = new CFB2990400DAO();
        try
        {
            return wfb299.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getFuncData(DataTable dt)
    {
        CFB2990400DAO wfb299 = new CFB2990400DAO();
        try
        {
            return wfb299.getFuncData(dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2990400DAO wfb299 = new CFB2990400DAO();
        try
        {
            return wfb299.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteFUNC(string MODE_ID)
    {
        try
        {
            CFB2990400DAO wfb299 = new CFB2990400DAO();
            BeginTransaction();

            wfb299.deleteFUNC(MODE_ID);
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string deleteData(List<Tuple<string, string>> deleteList, List<string> modeidList)
    {
        try
        {
            CFB2990400DAO wfb299 = new CFB2990400DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb299.deleteData(deleteitem.Item1,deleteitem.Item2);
            }

            foreach (var modeid in modeidList)
            {
                //刪除明細檔
                wfb299.deleteDtlData(modeid);
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
    public string updateData(CFB2990400DAO fb299)
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
    public string addData(CFB2990400DAO fb299)
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
    public string add_SYS_D_Data(CFB2990400DAO fb299)
    {
        try
        {
            //取得現有資料
            //DataTable tmp = fb299.getExist_SYS_D_Data();
            //if (tmp.Rows.Count > 0)
            //{
            //}
            //else {
            //}
            BeginTransaction();
            fb299.add_SYS_D_Data();
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