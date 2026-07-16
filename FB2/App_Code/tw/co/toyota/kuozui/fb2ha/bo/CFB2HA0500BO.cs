using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HA0500BO 的摘要描述
/// </summary>
public class CFB2HA0500BO : BaseService
{
	public CFB2HA0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getLevelCD(string start_dt)
    {
        CFB2HA0500DAO wfb2ha = new CFB2HA0500DAO();
        try
        {
            return wfb2ha.getLevelCD(start_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getEditLevelCD()
    {
        CFB2HA0500DAO wfb2ha = new CFB2HA0500DAO();
        try
        {
            return wfb2ha.getEditLevelCD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string addPjob(CFB2HA0500DAO fb2ha050)
    {
        string rtnmessage = "";
        try
        {
            if (fb2ha050.END_DT != "9999/12/31")
            {
                //檢查該職務代號是否存在員工人事資料
                DataTable dt = fb2ha050.getTB_H_M_EMP();
                if ((int)dt.Rows[0]["empcount"] > 0)
                    rtnmessage += "員工人事主檔存在入社日期>畫面輸入之結束日期的資料 \\n";

                //檢查該職務代號是否存在人事異動資料
                DataTable dt2 = fb2ha050.getTB_H_M_EMP_HR_CHANGE_H();
                if ((int)dt2.Rows[0]["empcount"] > 0)
                    rtnmessage += "人事異動單存在職務異動生效日期>畫面輸入之結束日期的資料 \\n";
            }

            //檢查重複資料
            DataTable dupdata = fb2ha050.getExistData();
            if (dupdata.Rows.Count > 0)
                rtnmessage += "職務代號+生效日期重覆";

            //檢查OK新增
            if (rtnmessage == "")
            {
               
                BeginTransaction();
                try
                {
                    fb2ha050.addPjob();

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

    public string updatePjob(CFB2HA0500DAO fb2ha050)
    {
        string rtnmessage = "";
        try
        {

            DateTime end_dt = DateTime.Parse(fb2ha050.END_DT);
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));

            if (end_dt < now)
                rtnmessage += "結束日期必須≧系統日 \\n";

            if (fb2ha050.END_DT != "9999/12/31")
            {
                //檢查該職務代號是否存在員工人事資料
                DataTable dt = fb2ha050.getTB_H_M_EMP();
                if ((int)dt.Rows[0]["empcount"] > 0)
                    rtnmessage += "員工人事主檔存在入社日期>畫面輸入之結束日期的資料 \\n";

                //檢查該職務代號是否存在人事異動資料
                DataTable dt2 = fb2ha050.getTB_H_M_EMP_HR_CHANGE_H();
                if ((int)dt2.Rows[0]["empcount"] > 0)
                    rtnmessage += "人事異動單存在職務異動生效日期>畫面輸入之結束日期的資料 \\n";
            }

            
            //檢查OK更新
            if (rtnmessage == "")
            {

                BeginTransaction();
                try
                {
                    fb2ha050.updatePjob();

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


    public string delete_Pjob(List<Tuple<string, string>> pjob_cd)
    {
        CFB2HA0500DAO fb2ha050 = new CFB2HA0500DAO();
        string rtnmessage = "";
        try
        {
            foreach (var item in pjob_cd)
            {
                //檢查是否已存在員工人事主檔
                DataTable tmp = fb2ha050.getExistEmpData(item.Item1);
                if ((int)tmp.Rows[0]["empcount"] > 0)
                {
                    rtnmessage += "職務代號" + item.Item1 + "，已存在員工人事資料，不可刪除 \\n";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in pjob_cd)
                    {
                        fb2ha050.deletePjobCD(item.Item1,item.Item2);
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
            else
                return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        } 
    }
}