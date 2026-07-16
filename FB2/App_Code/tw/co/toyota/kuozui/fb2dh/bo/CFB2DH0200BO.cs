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
/// CFB2DH0200BO 的摘要描述
/// </summary>
public class CFB2DH0200BO : BaseService
{
	public CFB2DH0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string deleteUNION_PJOB(List<string> union_pjob_cd)
    {
        try
        {
            CFB2DH0200DAO wfb2dh = new CFB2DH0200DAO();
            string msg = "";
            foreach (string item in union_pjob_cd)
            {
                //取得員工人事主檔資料
                DataTable tmp = wfb2dh.getDEPT_ORG(item);
                if (tmp.Rows.Count > 0)
                {
                    msg += "工會職務:" + item + ",該工會職務已存在員工人事主檔 !\\n";
                    continue;
                }
            }

            if (msg != "")
                return msg;

            BeginTransaction();
            foreach (string item in union_pjob_cd)
            {
                wfb2dh.deleteUNION_PJOB(item);
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

    public string addUNION_PJOB(CFB2DH0200DAO wfb2dh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dh.getExistData();

            if (tmp.Rows.Count > 0)
                return "工會職務代碼重覆!";
            else
            {
                BeginTransaction();
                wfb2dh.addUNION_PJOB();
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

    public string updateUNION_PJOB(CFB2DH0200DAO wfb2dh)
    {
        try
        {
            BeginTransaction();

            wfb2dh.updateUNION_PJOB();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getUNION_PJOB_CD()
    {
        try
        {
            CFB2DH0200DAO wfb2dh = new CFB2DH0200DAO();
            return wfb2dh.getUNION_PJOB_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
}