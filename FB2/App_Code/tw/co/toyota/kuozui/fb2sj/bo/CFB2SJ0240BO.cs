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
/// WFB2SJ0240Service 的摘要描述
/// </summary>
public class CFB2SJ0240BO : BaseService
{
    public CFB2SJ0240BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //取得修改資料
    public DataTable getUpdData(CFB2SJ0240DAO dao)
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

    //更新 TB_S_M_ASSESS_MA_PEO
    public string updateData(CFB2SJ0240DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateData();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //REGen 協理/二階理事人數配置檔生成
    public string reGenData(CFB2SJ0240DAO wfb2sj)
    {
        try
        {
           
            //BeginTransaction();
            String msg = "";
            wfb2sj.execSP_S_ASSESS_GEN_DEPT20_MA();
            msg = utilities.getSPLOG("SP_S_ASSESS_GEN_DEPT20_MA");
            if (msg != "") return msg;            
            DataTable dt = wfb2sj.getPEOMAEmpId();
            if (dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    wfb2sj.MA_EMP_ID = dt.Rows[i]["MA_EMP_ID"].ToString();
                    wfb2sj.execSP_S_ASSESS_UPD_RO_MA_PEO();
                    msg = msg + utilities.getSPLOG("SP_S_ASSESS_UPD_RO_MA_PEO");
                }
                if (msg != "") return msg;
            }
            //Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    
}