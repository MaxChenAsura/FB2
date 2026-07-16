using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SF1100BO 的摘要描述
/// </summary>
public class CFB2SF1100BO : BaseService
{
    public CFB2SF1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //檢核
    public string Check(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            CFB2SF1100DAO fb2sf = new CFB2SF1100DAO();

            DataTable dt = fb2sf.Check_SALARY_CLOSED(SALARY_DT, SALARY_TYPE);
            DataTable dt2 = fb2sf.Check_TB_S_M_ALLOCATION_D(SALARY_DT, SALARY_TYPE);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["CLOSED_DT"]) == null || Convert.ToString(dt.Rows[i]["CLOSED_DT"])=="")
                        return Resources.Resource.wfb2sf_WFB2SF1100ExecuteMessage1;//指定查詢條件的資料,未月結不允執行此功能 
                }
            }
            else
            {
                return Resources.Resource.wfb2sf_WFB2SF1100ExecuteMessage2; //未有指定查詢條件的法扣資料,無法執行此功能
            }
            if (Convert.ToInt32(dt2.Rows[0]["cnt"]) > 0)
            {
                return Resources.Resource.wfb2sf_WFB2SF1100ExecuteMessage3; //指定查詢條件的資料,已轉入,不允重新執行此功能
            }
            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            return ex.Message;
        }
    }
    //轉入薪資
    public string Execute(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            CFB2SF1100DAO fb2sf = new CFB2SF1100DAO();

            BeginTransaction();
            fb2sf.Del_TB_S_M_ARREARS_COURT_D(SALARY_DT, SALARY_TYPE);
            DataTable dt = fb2sf.Get_TempT3052(SALARY_DT, SALARY_TYPE);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    fb2sf.SALARY_DT = SALARY_DT;
                    fb2sf.SALARY_TYPE = SALARY_TYPE;
                    fb2sf.EMP_ID = Convert.ToString(dr["EMP_ID"]);
                    fb2sf.PAY_KIND = Convert.ToString(dr["PAY_KIND"]);
                    fb2sf.TOT_AMOUNT = Convert.ToString(dr["TOT_AMOUNT"]);
                    fb2sf.AMOUNT3052 = Convert.ToString(dr["AMOUNT3052"]);
                    fb2sf.Add_TB_S_M_ARREARS_COURT_D();
                }
            }
            else {
                return Resources.Resource.wfb2sf_WFB2SF1100ExecuteMessage4;// "指定查詢條件,無代扣法扣資料"
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