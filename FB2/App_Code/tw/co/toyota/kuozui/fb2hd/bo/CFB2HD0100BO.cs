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
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class CFB2HD0100BO : BaseService
{
	public CFB2HD0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	} 
 
    # region Qry
    public DataTable getData(string qdatakey)
    {
        try
        {
            CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
            wfb2hd.QDATAKEY = qdatakey;
            return wfb2hd.getData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工基本資料
    public DataTable getEMPFile(string emp_id)
    {
        try
        {
            CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
            wfb2hd.EMP_ID = emp_id;
            return wfb2hd.getEMPFile();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getJUDGEMENT_TYPE()
    {
        CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
        try
        {
            return wfb2hd.getJUDGEMENT_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getREASON_CD()
    {
        CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
        try
        {
            return wfb2hd.getREASON_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getREASON_CD(string CODE_VAL1)
    {
        CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
        try
        {
            return wfb2hd.getREASON_CD(CODE_VAL1);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
        try
        {
            return wfb2hd.getFUNC_ID(ID);
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
            CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2hd.deleteData(deleteitem);
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
    public string updateData(CFB2HD0100DAO fb2hd)
    {
        try
        {
            BeginTransaction();
            fb2hd.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2HD0100DAO fb2hd)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2hd.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb2hd.addData();
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