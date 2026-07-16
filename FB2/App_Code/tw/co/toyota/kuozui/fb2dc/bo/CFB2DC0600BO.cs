using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DC0600BO 的摘要描述
/// </summary>
public class CFB2DC0600BO : BaseService
{
    public CFB2DC0600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteData(List<Tuple<string, string, string, string>> datas)
    {
        try
        {
            CFB2DC0600DAO dao = new CFB2DC0600DAO();
            BeginTransaction();
            foreach (var item in datas)
            {
                dao = new CFB2DC0600DAO();
                dao.deleteABNORMAL_APPLY(item.Item1, item.Item2, item.Item3, item.Item4);

                //將 日勤務狀態檔 比對結果 改為 N
                dao.EMP_ID = item.Item1;
                dao.CALENDAR_DT = item.Item3;
                dao.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN();
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

    public System.Data.DataTable getData(string emp_id, string abnormal_type, string calendar_dt, string abnormal_source_cd)
    {
        try
        {
            CFB2DC0600DAO dao = new CFB2DC0600DAO();
            return dao.getData(emp_id, abnormal_type, calendar_dt, abnormal_source_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveABNORMAL_APPLY(CFB2DC0600DAO dao, string mod)
    {
        try
        {
            if (mod == "add")
            {
                DataTable dt = dao.getDupData();
                if (dt.Rows.Count > 0)
                    return "異常刷卡申請資料重覆";
            }
            BeginTransaction();
            if (mod == "mod")
            {
                dao.updateABNORMAL_APPLY();
            }
            else
            {
                dao.addABNORMAL_APPLY();
            }
            Commit();
            //將 日勤務狀態檔 比對結果 改為 N
            dao.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN();

            if (dao.IS_RE_MAKE == "Y")
            {
                //如果 明細畫面.重新製卡 = 是(Y) 才需要執行項目(2)
                //呼叫維護卡片資料檔
                dao.callSP_D_UPD_CARD_DATA(dao.EMP_ID);
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //一括新增
    public string batchABNORMAL_APPLY(CFB2DC0600DAO dao, List<string> emp_data)
    {
        try
        {
            string errmessage = "";
            //檢查 Grid中的工號 且 勤務日期 是否已申請 異常刷卡資料 
            foreach (var emp_id in emp_data)
            {
                DataTable dt = dao.getExistData(emp_id);
                if (dt.Rows.Count > 0)
                {
                    errmessage += emp_id + dt.Rows[0]["EMP_NAME"].ToString().Trim() + " 該工號已存在異常刷卡申請資料檔 \\n";
                }
            }
            if (errmessage == "")
            {
                //將Grid的資料，新增至異常刷卡資料檔
                try
                {
                    BeginTransaction();
                    foreach (var emp_id in emp_data)
                    {
                        dao.EMP_ID = emp_id;
                        dao.addABNORMAL_APPLY();

                        //將 日勤務狀態檔 比對結果 改為 N
                        dao.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN();
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

                if (dao.IS_RE_MAKE == "Y")
                {
                    //如果 明細畫面.重新製卡 = 是(Y) 才需要執行項目(2)
                    //呼叫維護卡片資料檔
                    foreach (var emp_id in emp_data)
                    {
                        dao.callSP_D_UPD_CARD_DATA(emp_id);
                    }
                }
            }
            else
                return errmessage;

            return "0";
        }
        catch (Exception ex)
        {

            return ex.Message;
        }
    }
}