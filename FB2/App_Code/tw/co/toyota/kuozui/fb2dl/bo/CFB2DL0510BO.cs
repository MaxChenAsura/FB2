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
/// CFB2DL0510BO 的摘要描述
/// </summary>
public class CFB2DL0510BO : BaseService
{
	public CFB2DL0510BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	} 
 
    # region Qry

    //取得特休代碼資料
    public DataTable getData(string hr_chg_cd,string dl_gen_Cd)
    {
        try
        {
            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();
            dl051DAO.HR_CHG_CD = hr_chg_cd;
            dl051DAO.DL_GEN_CD = dl_gen_Cd;
            return dl051DAO.getData();
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
            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();
            char[] ch1 = new Char[] { '|' };
            string[] split1 = null;            
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                split1 = deleteitem.Split(ch1);
                dl051DAO.HR_CHG_CD = split1[0].ToString();
                 dl051DAO.DL_GEN_CD = split1[1].ToString();
                //刪除主檔資料
                 dl051DAO.deleteData_H();
                 //刪除明細檔資料
                 dl051DAO.deleteData_D();
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
   
    public string addData(CFB2DL0510DAO dl051DAO)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dl051DAO.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }

            //檢查人事異動代碼是否存在
            tmp = dl051DAO.getCHG_CD();
            if (tmp.Rows.Count == 0)
            {
                return "人事異動代碼不存在!";
            }
            
            BeginTransaction();
            dl051DAO.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updData(CFB2DL0510DAO dl051DAO)
    {
        try
        {
           
            BeginTransaction();
            dl051DAO.updData_H();

            //刪除明細檔資料
            if(dl051DAO.IS_BIND_PJOB=="N")               
                dl051DAO.deleteData_D();
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


    # region Dtl
    public string addDtlData(CFB2DL0510DAO dl051DAO)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dl051DAO.getDtlExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }

            tmp = dl051DAO.getSameHRCD();
            if (tmp.Rows.Count > 0)
            {
                return "已有設定相同的人事異動代碼";
            }

            //檢查職務代碼是否存在
            tmp = dl051DAO.getPJOB_CD();
            if (tmp.Rows.Count == 0)
            {
                return "職務代碼不存在!";
            }

            BeginTransaction();
            dl051DAO.addDtlData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string deleteDtlData(List<string> deleteList,string hr_chg_cd ,string dl_gen_Cd )
    {   
        try
        {

            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();
            dl051DAO.HR_CHG_CD = hr_chg_cd;
            dl051DAO.DL_GEN_CD = dl_gen_Cd;
           
            BeginTransaction();
            foreach (string deleteDtlItem in deleteList)
            {
                dl051DAO.PJOB_CD = deleteDtlItem;
                dl051DAO.deleteDtlData();
                
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
    #endregion
}