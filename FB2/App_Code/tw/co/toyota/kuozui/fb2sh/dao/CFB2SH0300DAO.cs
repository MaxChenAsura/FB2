using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SH0300DAO 的摘要描述
/// </summary>
public class CFB2SH0300DAO : BaseDAO
{

    //年獎維護檔
    public string AWARD_YEAR { get; set; }
    public string AWARD_ROUND { get; set; }
    public string AWARD_DAYS { get; set; }
    public string AWARD_DT { get; set; }
    public string AWARD_STIME { get; set; }
    public string AWARD_ETIME { get; set; }
    public string AWARD_ITEM_A { get; set; } //年獎反映項目-考績
    public string AWARD_ITEM_RP { get; set; }//年獎反映項目-獎懲
    public string AWARD_ITEM_AL { get; set; }//年獎反映項目-勤怠
    public string AWARD_ITEM_D { get; set; } //年獎反映項目-紀律
    public string TARGET_GEN_DT { get; set; }
    public string AWARD_TOTAL_DECIMAL { get; set; }
    public string GEN_DT { get; set; }
    public string AWARD_TOTAL_AMOUNT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string FREEZE_FLAG { get; set; }

    //年獎明細結護檔
    public string AWARD_DAYS_PERSON { get; set; }//年獎發放天數(員工個人)
    public string EMP_ID { get; set; }
    public string LEVEL_PAY { get; set; }
    public string ABILITY_PAY { get; set; }
    public string PJOB_PAY { get; set; }
    public string PROFESSION_PAY { get; set; }
    public string FOOD_SUBSIDY { get; set; }
    public string LEVELUP_FLAG { get; set; }
    public string LEVEL_PAY_BEFORE { get; set; }
    public string ABILITY_PAY_BEFORE { get; set; }
    public string PJOB_PAY_BEFORE { get; set; }
    public string PROFESSION_PAY_BEFORE { get; set; }
    public string FOOD_SUBSIDY_BEFORE { get; set; }
    public string WORK_DAYS { get; set; }
    public string ATTEND_DAYS { get; set; }
    public string REWARD_DAYS { get; set; }
    public string DISCIPLINE_DAYS { get; set; }
    public string AWARD_WORK_DAYS { get; set; }
    public string AWARD_BASE { get; set; }
    public string AWARD_BASE_BEFORE { get; set; }
    public string AWARD_AMT { get; set; }
    public string AWARD_TAX { get; set; }
    public string AWARD_AMT_R { get; set; }
    public string AWARD_AMT_TMEP { get; set; }
    public string AWARD_AMT_LEVEL { get; set; }

    public string IS_LEAVE { get; set; }
    public string IS_RETIE { get; set; }

    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //計算需要
    public string AWARD_AMT_ROUND1 { get; set; }//第一回獎金


    public CFB2SH0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //取得年獎維護檔資料
    public DataTable getData_H(string award_year, string award_round)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select AWARD_YEAR,TARGET_GEN_DT, GEN_DT,FREEZE_FLAG from TB_S_M_AWARD_H ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@AWARD_YEAR", award_year);
            ht.Add("@AWARD_ROUND", award_round);
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    //取得年獎維護檔資料
    public DataTable checkFreeze()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select FREEZE_FLAG from TB_S_M_AWARD_H");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }



    //取得 年獎明細維護檔 員工資料
    internal DataTable getAwardEmpData()
    {
        try
        {
            StringBuilder sb_amt1 = new StringBuilder();
            sb_amt1.Append(@" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   "
                            +" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '1'  "
                            +" and EMP_ID =a.EMP_ID ),0) as amt1                  ");

            StringBuilder sb_amt2 = new StringBuilder();  
            sb_amt1.Append(@" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   "
                          + " where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '2' and PAY_TYPE ='R'  "
                          + " and EMP_ID =a.EMP_ID ),0) as amt2                                     ");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select a.* ");
            sb.Append(@",iif ( charindex(ID_DESC,'非自願離職')>0,'Y','N') IS_LEAVE
                        ,iif ( charindex(ID_DESC,'退休')>0,'Y','N') IS_RETIRE         ");
            sb.Append(sb_amt1);
            sb.Append(" from TB_S_M_AWARD_DM a ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得 第一回年獎金額 
    internal DataTable getRound1AMT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select AWARD_AMT from TB_S_M_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", "1");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //執行-更新年獎明細維護檔/原始檔
    internal void execute_D(DateTime now,string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" update TB_S_M_AWARD_DM              ");

            sb.Append(" update  ");
            sb.Append( tableName );

            sb.Append(" set AWARD_AMT = @AWARD_AMT          ");
            sb.Append(" ,AWARD_DAYS = @AWARD_DAYS             ");
            sb.Append(" ,AWARD_TAX = @AWARD_TAX             ");
            sb.Append(" ,AWARD_AMT_R = @AWARD_AMT_R         ");
            sb.Append(" ,AWARD_AMT_TMEP = @AWARD_AMT_TMEP   ");
            sb.Append(" ,AWARD_AMT_LEVEL = @AWARD_AMT_LEVEL ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY           ");
            //sb.Append(" ,UPDATED_DT = @UPDATED_DT           ");
            sb.Append(" ,FUNC_ID = @FUNC_ID                 ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR      ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND     ");
            sb.Append("  and EMP_ID = @EMP_ID               ");

            //set值
            ht.Add("@AWARD_DAYS", AWARD_DAYS_PERSON);
            ht.Add("@AWARD_AMT", AWARD_AMT);
            ht.Add("@AWARD_TAX", AWARD_TAX);
            ht.Add("@AWARD_AMT_R", AWARD_AMT_R);
            ht.Add("@AWARD_AMT_TMEP", AWARD_AMT_TMEP);
            ht.Add("@AWARD_AMT_LEVEL", AWARD_AMT_LEVEL);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", EMP_ID);


            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
           // ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新還原提出核可、年獎對象及金額、支付狀態(年獎維護檔)
    public void execute_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append(" , AWARD_TOTAL_AMOUNT = ( select isnull( sum(AWARD_AMT),0) from  TB_S_M_AWARD_DM  ");
            sb_AMT.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_AMT.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");


            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,AWARD_DAYS = @AWARD_DAYS");
            sb.Append(" ,GEN_DT = @GEN_DT");
            sb.Append(" ,AWARD_ITEM_A = @AWARD_ITEM_A");
            sb.Append(" ,AWARD_ITEM_RP = @AWARD_ITEM_RP");
            sb.Append(" ,AWARD_ITEM_AL = @AWARD_ITEM_AL");
            sb.Append(" ,AWARD_ITEM_D = @AWARD_ITEM_D");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //set值
            ht.Add("@AWARD_ITEM_A", AWARD_ITEM_A);
            ht.Add("@AWARD_ITEM_RP", AWARD_ITEM_RP);
            ht.Add("@AWARD_ITEM_AL", AWARD_ITEM_AL);
            ht.Add("@AWARD_ITEM_D", AWARD_ITEM_D);
            ht.Add("@AWARD_DAYS", AWARD_DAYS);
            ht.Add("@GEN_DT", DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")));
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@CHG_STATUS", "D");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得執行後的總金額及總人數
    public void GetResult(string award_year, string award_round)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select AWARD_TOTAL_DECIMAL, AWARD_TOTAL_AMOUNT from TB_S_M_AWARD_H");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@AWARD_YEAR", award_year);
            ht.Add("@AWARD_ROUND", award_round);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.AWARD_TOTAL_DECIMAL = Convert.ToString(dr["AWARD_TOTAL_DECIMAL"]);
                this.AWARD_TOTAL_AMOUNT = Convert.ToString(dr["AWARD_TOTAL_AMOUNT"]);
            }
        }
        catch
        {
            throw;
        }
    }
}