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
/// CFB2DI0300BO 的摘要描述
/// </summary>
public class CFB2DI0300BO : BaseService
{
    public CFB2DI0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteOVERTIME_TARGET(List<Tuple<string, string, string>> dept_no)
    {
        try
        {
            CFB2DI0300DAO wfb2di = new CFB2DI0300DAO();

            BeginTransaction();
            foreach (var item in dept_no)
            {
                wfb2di.deleteOVERTIME_TARGET(item);
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

    public string addOVERTIME_TARGET(CFB2DI0300DAO wfb2di)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2di.getExistData();

            if (tmp.Rows.Count > 0)
                return "部門+管理類別+年度 資料重覆";
            else
            {
                BeginTransaction();
                wfb2di.addOVERTIME_TARGET();
                Commit();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateOVERTIME_TARGET(CFB2DI0300DAO wfb2di)
    {
        try
        {
            BeginTransaction();

            wfb2di.updateOVERTIME_TARGET();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string deleteOVERTIME_TARGET_EMP(List<Tuple<string, string, string, string, string>> dept_no_list)
    {
        try
        {
            CFB2DI0300DAO wfb2di = new CFB2DI0300DAO();

            BeginTransaction();
            foreach (var item in dept_no_list)
            {
                wfb2di.deleteOVERTIME_TARGET_EMP(item);
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

    public string addOVERTIME_TARGET_EMP(CFB2DI0300DAO wfb2di, List<string> pjob_cd_list)
    {
        try
        {
            for (int i = 0; i < pjob_cd_list.Count; i++)
            {
                wfb2di.PJOB_CD = pjob_cd_list[i];
                //取得現有資料
                DataTable tmp = wfb2di.getExistData2();

                if (tmp.Rows.Count > 0)
                    return "年度+管理類別+職種(W/S區分)+工數區分+職務代號 資料重覆";
            }

            BeginTransaction();
            for (int i = 0; i < pjob_cd_list.Count; i++)
            {
                wfb2di.PJOB_CD = pjob_cd_list[i];

                wfb2di.addOVERTIME_TARGET_EMP();
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

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            CFB2DI0300DAO wfb2di = new CFB2DI0300DAO();
            return wfb2di.getDEPT_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}