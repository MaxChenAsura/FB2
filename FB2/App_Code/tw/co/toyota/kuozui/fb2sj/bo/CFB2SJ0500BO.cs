using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

/// <summary>
/// WFB2SJ0500Service 的摘要描述
/// </summary>
public class CFB2SJ0500BO : BaseService
{
    public CFB2SJ0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getAssessBaseData()
    {
        try
        {
            CFB2SJ0500DAO wfb2sj = new CFB2SJ0500DAO();
            return wfb2sj.getAssessData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工明細資料
    public DataTable getEmpDtlData(CFB2SJ0500DAO dao)
    {
        try
        {
            return dao.getEmpDtlData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得員工明細資料
    public DataTable getEmpTargetData(CFB2SJ0500DAO dao)
    {
        try
        {
            return dao.getEmpTargetData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工評分範圍資料
    public DataTable getEmpAssessRateData(CFB2SJ0500DAO dao)
    {
        try
        {
            return dao.getEmpAssessRateData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEmpAssessScoreData(CFB2SJ0500DAO dao)
    {
        try
        {
            return dao.getEmpAssessScore(0,100,"ASSESS_YEAR",dao.ASSESS_YEAR,dao.ASSESS_TYPE,dao.EMP_ID);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_ASSESS_SCORE
    public string updateSCORE(CFB2SJ0500DAO wfb2sj)
    {
        try
        {
           
            BeginTransaction();

            wfb2sj.updateSCORE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //更新 TB_S_M_ASSESS_TARGET
    public string updateTARGET(CFB2SJ0500DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateTARGET();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //處理提出簽核
    public string approve(CFB2SJ0500DAO wfb2sj)
    {
        try
        {
            string rtnmessage = "";//存在檢查後的訊息
            //檢查已全部評核
            DataTable tmp = wfb2sj.checkComplete();
            if (tmp.Rows.Count > 0)
                return "尚有未評核完成的員工,無法提出簽核!";
           

            //BeginTransaction();
            //wfb2sj.updateDIRECTORH();
           // wfb2sj.addAssessLog_Batch();

            wfb2sj.execSP_S_ASSESS_DIREC_APPROVE();
            rtnmessage += utilities.getSPLOG("SP_S_ASSESS_DIREC_APPROVE");
            if (rtnmessage != "")
            {
                //RollBack();
                return rtnmessage;
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
    //檢查該部門是否完成簽核
    public DataTable getAssessDircH(string assess_year,string assess_type, string dept_no, string direc_emp_id)
    {
        try
        {
            CFB2SJ0500DAO dao = new CFB2SJ0500DAO();

            return dao.getAssessDircH(assess_year, assess_type, dept_no, direc_emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public IWorkbook createExcel(CFB2SJ0500DAO dao, string type)
    {
        try
        {

            DataTable dt = dao.selectData();

            if (dt.Rows.Count == 0) return null;

            if (dt.Rows.Count > 0)
            {
                CFB2SJCOMMBO styleBO = new CFB2SJCOMMBO();
                return styleBO.createReferExcel(dt, "SJ050");
                
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string chtdate(string str)
    {
        //TaiwanCalendar twC = new TaiwanCalendar();
        String st = DateTime.Parse(str).ToString("yyyy");
        string st1 = DateTime.Parse(str).ToString("MMdd");
        string tdate = Convert.ToString(Convert.ToString(Convert.ToInt32(st) - 1911)) + st1;
        return tdate;
    }
}