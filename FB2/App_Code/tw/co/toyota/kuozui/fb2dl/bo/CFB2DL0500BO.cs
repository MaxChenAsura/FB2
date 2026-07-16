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
/// CFB2DL0500BO 的摘要描述
/// </summary>
public class CFB2DL0500BO : BaseService
{
	public CFB2DL0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	} 
 
    # region Qry


    public DataTable getData(string dl_gen_Cd)
    {
        try
        {
            CFB2DL0500DAO dl050DAO = new CFB2DL0500DAO();
            dl050DAO.DL_GEN_CD = dl_gen_Cd;
            return dl050DAO.getData();
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
            CFB2DL0500DAO dl050DAO = new CFB2DL0500DAO();
            char[] ch1 = new Char[] { '|' };
            string[] split1 = null;            
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                split1 = deleteitem.Split(ch1);
                 dl050DAO.DL_GEN_CD = split1[0].ToString();
                //刪除主檔資料
                 dl050DAO.deleteData();
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
   
    public string addData(CFB2DL0500DAO dl050DAO)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dl050DAO.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            dl050DAO.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updData(CFB2DL0500DAO dl050DAO)
    {
        try
        {
           
            BeginTransaction();
            dl050DAO.updData();
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