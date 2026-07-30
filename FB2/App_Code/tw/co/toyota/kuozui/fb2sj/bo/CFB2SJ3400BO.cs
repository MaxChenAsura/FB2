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
/// WFB2SJ3400Service 的摘要描述
/// </summary>
public class CFB2SJ3400BO : BaseService
{
    public CFB2SJ3400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //取得修改資料
    public DataTable getUpdData(CFB2SJ3400DAO dao)
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
    
    //更新 TB_S_M_ASSESS_ITEM
    public string updateData(CFB2SJ3400DAO wfb2sj)
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
   
    
    public DataTable getAssessData(string assess_year, string assess_type)
    {
        try
        {
            CFB2SJ3400DAO wfb2sj = new CFB2SJ3400DAO();
            wfb2sj.ASSESS_YEAR = assess_year;
            wfb2sj.ASSESS_TYPE = assess_type;
            return wfb2sj.getAssessData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    
       
    //REGen 協理/二階理事人數配置檔生成
    public string reGenData(CFB2SJ3400DAO wfb2sj)
    {
        try
        {
            DataTable dt = wfb2sj.getPEODeptNo();
            //BeginTransaction();
            String msg = "";
            wfb2sj.execSP_S_ASSESS_GEN_DEP20_PEO();
            msg = utilities.getSPLOG("SP_S_FOREIGN_GEN_DEP20_PEO");
            if (msg != "") return msg;
            if (dt.Rows.Count > 0)
            {
                //CFB2SJ3400DAO daoObj;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    wfb2sj.DEPT_NO_20 = dt.Rows[i]["DEPT_NO_20"].ToString();
                    wfb2sj.execSP_S_ASSESS_UPD_RO_DEP20_PEO();
                    msg = msg + utilities.getSPLOG("SP_S_FOREIGN_GEN_DEP20_PEO");                    
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
    //REGen L2生成
    public string reGenL2Data(CFB2SJ3400DAO wfb2sj)
    {
        try
        {
            //BeginTransaction();
            String msg = "";
            wfb2sj.execSP_S_ASSESS_GEN_L2_DATA();
            msg = utilities.getSPLOG("SP_S_FOREIGN_GEN_L2_DATA");
            if (msg != "") return msg;
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