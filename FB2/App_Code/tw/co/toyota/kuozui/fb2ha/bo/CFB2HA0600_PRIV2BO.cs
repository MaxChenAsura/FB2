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
/// CFB2HA0600BO 的摘要描述
/// </summary>
public class CFB2HA0600_PRIV2BO : BaseService
{
    public CFB2HA0600_PRIV2BO()
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
    //        CFB2HA0600DAO wfb2ha = new CFB2HA0600DAO();
    //        wfb2ha.SYS_CD = sys_cd;
    //        return wfb2ha.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getSYS_ID()
    {
        CFB2HA0600_PRIV2DAO wfb2ha = new CFB2HA0600_PRIV2DAO();
        try
        {
            return wfb2ha.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2HA0600_PRIV2DAO wfb2ha = new CFB2HA0600_PRIV2DAO();
        try
        {
            return wfb2ha.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2HA0600_PRIV2DAO wfb2ha = new CFB2HA0600_PRIV2DAO();
        try
        {
            return wfb2ha.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2HA0600_PRIV2DAO wfb2ha = new CFB2HA0600_PRIV2DAO();
        try
        {
            return wfb2ha.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2HA0600_PRIV2DAO wfb2ha = new CFB2HA0600_PRIV2DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2ha.deleteData(deleteitem);
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
    public string updateData(CFB2HA0600_PRIV2DAO fb2ha)
    {
        try
        {
            BeginTransaction();
            fb2ha.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2HA0600_PRIV2DAO fb2ha)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2ha.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb2ha.addData();
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