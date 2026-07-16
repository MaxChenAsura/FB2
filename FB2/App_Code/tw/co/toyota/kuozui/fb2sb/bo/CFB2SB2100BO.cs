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
/// CFB2SB2100BO 的摘要描述
/// </summary>
public class CFB2SB2100BO : BaseService
{
    public CFB2SB2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(string emp_id, string SALARY_ID, string START_DT)
    {
        CFB2SB2100DAO dao = new CFB2SB2100DAO();
        try
        {
            return dao.getDefaultData(emp_id, SALARY_ID, START_DT);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getData2(string emp_id, string SALARY_ID, string START_DT, string seq_no)
    {
        CFB2SB2100DAO dao = new CFB2SB2100DAO();
        try
        {
            return dao.getDefaultData2(emp_id, SALARY_ID, START_DT, seq_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2SB2100DAO wfb2sb = new CFB2SB2100DAO();
    //        wfb2sb.SYS_CD = sys_cd;
    //        return wfb2sb.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getSYS_ID()
    {
        CFB2SB2100DAO wfb2sb = new CFB2SB2100DAO();
        try
        {
            return wfb2sb.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2SB2100DAO wfb2sb = new CFB2SB2100DAO();
        try
        {
            return wfb2sb.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2SB2100DAO wfb2sb = new CFB2SB2100DAO();
        try
        {
            return wfb2sb.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2SB2100DAO wfb2sb = new CFB2SB2100DAO();
        try
        {
            return wfb2sb.getFUNC_ID(ID);
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
            CFB2SB2100DAO wfb2ib = new CFB2SB2100DAO();
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
    public string updateData(CFB2SB2100DAO fb2sb)
    {
        try
        {
            string rtnmessage = "";
            DataTable dt = new DataTable();
            string sysYM = DateTime.Now.AddMonths(-1).ToString("yyyy/MM").Replace("/", "");//系統年月
            if (Convert.ToInt32(fb2sb.DATA_YM) < Convert.ToInt32(sysYM))
            {
                rtnmessage += checkData1(fb2sb);
            }
            else if (Convert.ToInt32(fb2sb.DATA_YM) == Convert.ToInt32(sysYM))
            {
                //20150528 不需檢查薪資鎖定
                //rtnmessage += checkData2(fb2sb);
            }
            //4.若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),且  異動後金額 <> 異動前金額 時 ,
            //則執行以下SQL,若資料存在則以顯示錯誤訊息"加扣款期間部分已計薪,無法修改此加扣款金額",保留原畫面不繼續執行資料修改作業。
            if (fb2sb.CHG_AMT_A != fb2sb.AMOUNT)
            {
                if (fb2sb.getExistDataCheck1() > 0)
                    rtnmessage += "加扣款期間部分已計薪,無法修改此加扣款金額\\n";
            }
            //5若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),且  異動後加扣款期間迄 < 異動前加扣款期間迄時 ,
            //則執行以下SQL,若資料存在則以顯示錯誤訊息"異動後加扣款期間迄部分已計薪,無法修改此加扣款迄日",保留原畫面不繼續執行資料修改作業。
            if (Convert.ToDateTime(fb2sb.END_DT_A) < Convert.ToDateTime(fb2sb.START_DT_E))
                if (fb2sb.getExistDataCheck2() > 0)
                    rtnmessage += "異動後加扣款期間迄部分已計薪,無法修改此加扣款迄日\\n";

            if (rtnmessage == "")
            {
                try
                {
                    fb2sb.SEQ_NO = getMax_SEQ_NO(fb2sb);
                    BeginTransaction();
                    fb2sb.updateData();
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string updateData2(CFB2SB2100DAO fb2sb,string seq_no)
    {
        try
        {
            string rtnmessage = "";
            DataTable dt = new DataTable();
            string sysYM = DateTime.Now.ToString("yyyy/MM").Replace("/", "");//系統年月
            if (sysYM.Equals(fb2sb.DATA_YM) == false)
            {
                rtnmessage += checkData1(fb2sb);
                //rtnmessage += checkData2(fb2sb);
            }


            if (rtnmessage == "")
            {
                try
                {
                    //取得序號
                    fb2sb.SEQ_NO = Convert.ToInt32(seq_no);
                    BeginTransaction();
                    fb2sb.updateData2();
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string updateData3(CFB2SB2100DAO fb2sb)
    {
        try
        {

            if (fb2sb.getDeleteDataCheck() > 0)
            {
                return "此加扣款期間迄已計薪,無法新增\\n";
            }
            else
            {
                fb2sb.SEQ_NO = getMax_SEQ_NO(fb2sb);
                BeginTransaction();
                fb2sb.updateData3();
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
    public string checkData1(CFB2SB2100DAO fb2sb)
    {
        string result = "";
        string edt = "";
        //1.檢核1:   
        DataTable dt = fb2sb.getLatestSalaryYM();
        if (dt.Rows.Count > 0)
        {
            string latestSalaryYM = dt.Rows[0]["SALARY_YM"].ToString();
            DataTable dt_edt = fb2sb.getEndtSalaryYM(latestSalaryYM);
            if (dt_edt.Rows.Count > 0)
            {
                edt = dt_edt.Rows[0]["SALARY_EDT"].ToString();
            }

            int sYM = dt.Rows[0]["SALARY_YM"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["SALARY_YM"].ToString()) : 0;
            int dYM = Convert.ToInt32(fb2sb.DATA_YM);
            if (sYM != 0 && dYM <= sYM)
            {
                if (fb2sb.EDT != edt) //如果輸入的異動後加扣款期間迄 = 最新的薪資計算區間迄日，就不擋  
                {
                    result += "此加扣款期間迄已計薪,無法新增\\n";
                }                
            }
        }
        return result;
    }

    public string checkData2(CFB2SB2100DAO fb2sb)
    {
        string result = "";
        //檢核2:   
        DataTable dt = fb2sb.getIsLoked();
        if (dt.Rows.Count > 0)
        {
            string salaryLocked = dt.Rows[0]["SALARY_LOCKED"].ToString();
            if (salaryLocked == "Y")
            {
                result += "此加扣款期間起資料已鎖定,無法新增\\n";
            }
        }
        return result;
    }

    public DataTable getIsLoked(string DataYM)
    {
        CFB2SB2100DAO fb2sb = new CFB2SB2100DAO();
        fb2sb.DATA_YM = DataYM;
        return fb2sb.getIsLoked();
    }

    public string addData(CFB2SB2100DAO fb2sb)
    {
        try
        {
            string rtnmessage = "";
            DataTable dt = new DataTable();

            //檢查是否有權限
            dt = fb2sb.getSubsidyCount();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                return "此薪資項目代號無權限使用,無法新增 \\n";
            }

            DataTable tmp = fb2sb.getExistData1();
            if (tmp.Rows.Count > 0)
            {
                return "KEY值 已存在,無法新增";
            }

            string sysYM = DateTime.Now.AddMonths(-1).ToString("yyyy/MM").Replace("/", "");//系統年月
            if (Convert.ToInt32(fb2sb.DATA_YM) < Convert.ToInt32(sysYM))
            {
                rtnmessage += checkData1(fb2sb);
            }
            else if (Convert.ToInt32(fb2sb.DATA_YM) == Convert.ToInt32(sysYM))
            {
                rtnmessage += checkData2(fb2sb);
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    fb2sb.SEQ_NO = getMax_SEQ_NO(fb2sb);
                    BeginTransaction();
                    fb2sb.addData();
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public int getMax_SEQ_NO(CFB2SB2100DAO fb2sb)
    {
        int newSEQ_NO = 0;
        DataTable dt = fb2sb.getMax_SEQ_NO();
        if (dt.Rows.Count > 0 && dt.Rows[0]["SEQ_NO"] != DBNull.Value && Convert.ToString(dt.Rows[0]["SEQ_NO"]) !="")
            newSEQ_NO = Convert.ToInt32(dt.Rows[0]["SEQ_NO"]) + 1;   //取到最大序號 + 1
        else
            newSEQ_NO = 1;
        return newSEQ_NO;
    }
    #endregion
}