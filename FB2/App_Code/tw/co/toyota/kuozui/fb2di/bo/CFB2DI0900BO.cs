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
/// CFB2DI0900BO 的摘要描述
/// </summary>
public class CFB2DI0900BO : BaseService
{
	public CFB2DI0900BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string deleteOVERTIME_TYPE(List<string> start_dt_list)
    {
        try
        {
            CFB2DI0900DAO dao = new CFB2DI0900DAO();
            BeginTransaction();
            for (int i = 0; i < start_dt_list.Count; i++)
            {
                dao.deleteDISASTER_DT(start_dt_list[i].ToString());
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

    public DataTable getDefaultData(string start_dt)
    {
        try
        {
            CFB2DI0900DAO wfb2di = new CFB2DI0900DAO();
            return wfb2di.getDefaultData(start_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveDISASTER_DT(CFB2DI0900DAO wfb2di, string mod)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2di.getDISASTER_DT(wfb2di.START_DT);

            BeginTransaction();

            //更新模式
            if (mod == "mod")
            {
                //更新
                wfb2di.updateDISASTER_DT();
            }
            else
            {
                //新增模式
                if (tmp.Rows.Count > 0)
                {
                    //不新增
                    return "開始日期 資料重覆 !";
                }
                else
                {
                    //不存在資料直接新增
                    wfb2di.addDISASTER_DT();
                }
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

    public string checkOVERTIME_APPLY(string start_dt, string end_dt)
    {
        try
        {
            string result = "";
            int rtn = 0;
            CFB2DI0900DAO dao = new CFB2DI0900DAO();
            rtn = dao.chk_OVERTIME_APPLY_BEFORE(start_dt, end_dt);
            if (rtn > 0)
            {
                result = "加班檔(勤前)已有讓時段申請資料，無法刪除。\\n";
            }

            if (result != "")
                return result;

            rtn = dao.chk_OVERTIME_APPLY_AFTER(start_dt, end_dt);
            if (rtn > 0)
            {
                result = "加班檔(勤後)已有讓時段申請資料，無法刪除。\\n";
            }


            return result;
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    /*
    public string checkOVERTIME_APPLY2(string start_dt, string end_dt)
    {
        try
        {
            string result = "";
            DataTable dt = new DataTable();
            CFB2DI0900DAO dao = new CFB2DI0900DAO();
            dt = dao.chk_OVERTIME_APPLY_AFTER(start_dt, end_dt);
            if (dt.Rows.Count > 0)
            {
                result = "加班檔(勤後)已有讓時段申請資料，無法刪除。\\n";
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
    */


    public string checkOVERTIME_FLOW(string start_dt, string end_dt)
    {
        try
        {
            string result = "";
            DataTable dt = new DataTable();
            CFB2DI0900DAO dao = new CFB2DI0900DAO();
            dt = dao.getOVERTIME_FLOW(start_dt, end_dt);
            if (dt.Rows.Count > 0)
            {
                result = "加班檔(在途申請VW)已有讓時段申請資料，無法刪除。\\n";
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
}