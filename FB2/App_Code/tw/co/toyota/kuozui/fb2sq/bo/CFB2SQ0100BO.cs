using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SQ0100BO 的摘要描述
/// </summary>
public class CFB2SQ0100BO : BaseService
{
    public CFB2SQ0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string chkMATERNITY_LEAVE(CFB2SQ0100DAO dao)
    {
        try
        {
            string st = "";
            string msg = "0";
            DataTable dt = dao.chkMATERNITY_LEAVE();
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["cnt"].ToString();
                if (st == "0")
                {
                    msg = "本月無符合產假津貼的員工！";
                }
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string chkIS_CLOSE(CFB2SQ0100DAO dao)
    {
        try
        {
            string st = "";
            string msg = "0";
            DataTable dt = dao.chkIS_CLOSE();
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["cnt"].ToString();
                if (st != "0")
                {
                    msg = "本月已結案無法再執行計算功能！";
                }
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string doExec(CFB2SQ0100DAO dao)
    {

        try
        {
            //call sp
            dao.SP_S_MATERNITY_COMPUTE();

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_S_MATERNITY_COMPUTE");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                    return Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
            }

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}