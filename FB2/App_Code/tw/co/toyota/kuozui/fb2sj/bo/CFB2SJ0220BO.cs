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
/// WFB2SJ0220Service 的摘要描述
/// </summary>
public class CFB2SJ0220BO : BaseService
{
    public CFB2SJ0220BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_ASSESS_RATE
    public string addRATE(CFB2SJ0220DAO wfb2sj)
    {
        try
        {
            //檢核
            string errMsg = "";
            errMsg = checkRate(wfb2sj);
            if (errMsg != "")
            {
                return errMsg;
            }
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "考核類型+職種+資格 資料重覆!";
           
                BeginTransaction();
                wfb2sj.addRATE();
                Commit();
           

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //取得修改資料
    public DataTable getUpdData(CFB2SJ0220DAO dao)
    {
        try
        {
            return dao.getUpdData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_ASSESS_RATE
    public string updateRATE(CFB2SJ0220DAO wfb2sj)
    {
        try
        {
            //檢核
            string errMsg = "";
            errMsg = checkRate(wfb2sj);
            if (errMsg != "")
            {
                return errMsg;
            }
            BeginTransaction();

            wfb2sj.updateRATE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string checkRate(CFB2SJ0220DAO wfb2sj)
    {
        try
        {
            string errMsg = "";
            if (wfb2sj.WS_CD != "G")
            {
                if ((wfb2sj.RATE_A + wfb2sj.RATE_B + wfb2sj.RATE_C + wfb2sj.RATE_D + wfb2sj.RATE_E) != 100)
                {
                    return "考核比例配置需等於100!";
                }
            }


            return errMsg;


        }
        catch (Exception ex)
        {
            throw;
            //return ex.Message;
        }
    }
    //刪除 TB_S_M_ASSESS_RATE
    public string deleteRATE(List<Tuple<string,string,string>> disting_cd)
    {
        try
        {
            CFB2SJ0220DAO wfb2sj = new CFB2SJ0220DAO();
            BeginTransaction();
            foreach (var item in disting_cd)
            {
                wfb2sj.deleteRATE(item.Item1, item.Item2, item.Item3);
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
}