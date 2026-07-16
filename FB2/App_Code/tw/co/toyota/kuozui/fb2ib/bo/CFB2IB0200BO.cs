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
/// CFB2IB0200BO 的摘要描述
/// </summary>
public class CFB2IB0200BO : BaseService
{
	public CFB2IB0200BO()
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
    //        CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
    //        wfb2ib.SYS_CD = sys_cd;
    //        return wfb2ib.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getSYS_ID()
    {
        CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
        try
        {
            return wfb2ib.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
        try
        {
            return wfb2ib.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
        try
        {
            return wfb2ib.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
        try
        {
            return wfb2ib.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteData_1(List<string> deleteList)
    {
        try
        {
            CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                DataTable tmp = wfb2ib.getExistData2(deleteitem);
                if (tmp.Rows.Count > 0)
                {


                    return "1";


                }

                
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
    public string deleteData_2(List<string> deleteList)
    {
        try
        {
            CFB2IB0200DAO wfb2ib = new CFB2IB0200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
               

                //刪除主檔資料
                wfb2ib.deleteData(deleteitem);
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
    public string updateData(CFB2IB0200DAO fb2ib)
    {
        try
        {
            BeginTransaction();
            fb2ib.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2IB0200DAO fb2ib)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2ib.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb2ib.addData();
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