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
/// CFB2SC1100BO 的摘要描述
/// </summary>
public class CFB2SC1100BO : BaseService
{
    public CFB2SC1100BO()
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
    public string checkdeleteExistData(List<string> deleteList)
    {
        try
        {
            string notPassMeg = "";
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            foreach (string deleteitem in deleteList)
            {
                DataTable dt = dao.checkdeleteExistData(deleteitem);
                if (dt.Rows.Count > 0)
                {
                    notPassMeg += "薪資項目代號" + deleteitem + "已使用,無法刪除\\r\\n";
                }
            }
            if (string.IsNullOrEmpty(notPassMeg.Trim()))
            {
                return "0";
            }
            else
            {
                return notPassMeg;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
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
    //儲存
    public string saveData(CFB2SC1100DAO dao, string mod)
    {
        try
        {
            BeginTransaction();
            //更新模式
            if (mod == "mod")
            {
                dao.updateData();
            }
            else //新增模式
            {
                //取得現有資料
                DataTable tmp = dao.getExistData();
                if (tmp.Rows.Count > 0)
                {
                    return "KEY值已存在,無法新增";
                }
                BeginTransaction();
                dao.addData();
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

    #endregion
}