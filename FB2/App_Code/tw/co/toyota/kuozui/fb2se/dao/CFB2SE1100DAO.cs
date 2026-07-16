using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SE1100DAO 的摘要描述
/// </summary>
public class CFB2SE1100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string FUNC_ID { get; set; }
    public string LICENSE_ID { get; set; }


    public CFB2SE1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

   
   //調薪計算
    public void SalaryUpComputer_dao(string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_FB2SE110");
            ht.Add("@qry_EFFECT_YM", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getNoDataEmp_Id(string year, string firDay, string midDay)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.EMP_ID from VW_H_EMP_DATA a");
            sb.Append(" where a.EMP_CD='1'");
            sb.Append(" and ((a.EMP_STATUS in ('01','02') and a.JOIN_DT<= @firDay) or (a.EMP_STATUS='99'");
            sb.Append(" and a.LEAVE_DT> @midDay )) and a.PJOB_CD <>'PJ50' and a.PJOB_CD <>'PJ60'");
            sb.Append(" and a.EMP_ID not in (");
            sb.Append(" select b.EMP_ID from TB_S_M_ASSESS b");
            sb.Append(" where b.ASSESS_YEAR = @YM and b.SCORE_1H <> '')");

            ht.Add("@YM", year);
            ht.Add("@firDay", firDay);
            ht.Add("@midDay", midDay);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
    //檢查計算保費之種類是否已被薪資擔當鎖定,若已鎖定不允重新計算
    internal DataTable getS_M_SALARY_ADJ_H(string p_effect_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) RESULTCOUNT from TB_S_M_SALARY_ADJ_H");
            sb.Append(" where isnull(RELEASE_BY,'') <>'' and  EFFECT_YM=@p_effect_ym");
            ht.Add("@p_effect_ym", p_effect_ym);
            // dbConn.ExecuteT(sb, ht, true);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查3A以下調薪金額主檔是否已設定
    internal DataTable get_TB_S_M_SALARYSET_H(string p_effect_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) RESULTCOUNT from TB_S_M_SALARYSET_H");
            sb.Append(" where EFFECT_YM=@p_effect_ym");
            ht.Add("@p_effect_ym", p_effect_ym);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查2B以上本薪調整主檔是否已設定
    internal DataTable get_TB_S_M_2BSALARY_SET_H(string p_effect_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) RESULTCOUNT from TB_S_M_2BSALARY_SET_H");
            sb.Append(" where EFFECT_YM=@p_effect_ym");
            ht.Add("@p_effect_ym", p_effect_ym);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

 
}