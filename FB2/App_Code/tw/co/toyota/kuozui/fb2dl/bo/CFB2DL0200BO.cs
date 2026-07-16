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
/// CFB2DL0200BO 的摘要描述
/// </summary>
public class CFB2DL0200BO : BaseService
{
    public CFB2DL0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string MAIN_DESC { get; set; }
    public string USER_UPD { get; set; }
    # region Qry

    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteData(deleteitem);
                //刪除主檔資料的所有明細檔
                dao.deleteData_D(deleteitem);
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

    #region Dtl
  
    public string updateDtlData(CFB2DL0200DAO dao, DataTable dtSaveData, string deleteKeyList)
    {
        try
        {
            BeginTransaction();
            foreach (string deleteItem in deleteKeyList.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                dao.deleteDtlData(deleteItem);
            }

            dao.updateData();

            for (int i = 0; i < dtSaveData.Rows.Count; i++)
            {
                if (dao.isDtlExist(Convert.ToString(dtSaveData.Rows[i]["LEAVE_PLAN_DT"])))
                    dao.updateDtlData(dtSaveData.Rows[i]);
                else
                    dao.addDtlData(dtSaveData.Rows[i]);
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
    public string addDtlData(CFB2DL0200DAO dao, DataTable dtSaveData)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistDtlData();
            if (tmp.Rows.Count > 0)
            {
                return "排休資料重覆";
            }
            else
            {
                BeginTransaction();
                dao.addData();
                for (int i = 0; i < dtSaveData.Rows.Count; i++)
                {
                    dao.addDtlData(dtSaveData.Rows[i]);
                }
                Commit();
                return "0";
            }
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion





}