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
/// CFB2SI0200DAO 的摘要描述
/// </summary>
public class CFB2SI0200DAO : BaseDAO
{
    public string BONUS_YEAR { get; set; }
    public string BONUS_YEAR_H { get; set; }
    public string BONUS_YEAR_D { get; set; }
    public string BONUS_ROUND { get; set; }
    public decimal BONUS_DAYS { get; set; }
    public string GEN_DT { get; set; }
    public decimal LEVEL_PAY { get; set; }
    public decimal ABILITY_PAY { get; set; }
    public decimal PJOB_PAY { get; set; }
    public decimal PROFESSION_PAY { get; set; }
    public decimal FOOD_SUBSIDY { get; set; }
    public decimal WORK_DAYS { get; set; }
    public decimal ATTEND_DAYS { get; set; }
    public decimal REWARD_DAYS { get; set; }
    public decimal DISCIPLINE_DAYS { get; set; }
    public string EMP_ID { get; set; }
    public string FREEZE_FLAG { get; set; }
    //GetNum
    public string B_LEAVE_UC { get; set; }
    public string B_LEAVE_B { get; set; }
    public string B_LEAVE_Q { get; set; }
    public string B_LEAVE_OP { get; set; }
    public string B_FIRST_CNT_P { get; set; }
    public string B_SECOND_CNT_P { get; set; }
    public string B_THIRD_CNT_P { get; set; }
    public string B_FIRST_CNT_M { get; set; }
    public string B_SECOND_CNT_M { get; set; }
    public string B_THIRD_CNT_M { get; set; }
    //GetResult
    public string BONUS_TOTAL_DECIMAL { get; set; }
    public string BONUS_TOTAL_AMOUNT { get; set; }

    public CFB2SI0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public void GetData_H(string bonus_year, string bonus_days)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select BONUS_YEAR,GEN_DT,FREEZE_FLAG from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @bonus_year");
            ht.Add("@bonus_year", bonus_year);
            ht.Add("@bonus_days", bonus_days);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_YEAR_H = Convert.ToString(dr["BONUS_YEAR"]);
                this.GEN_DT = Convert.ToString(dr["GEN_DT"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
            }
        }
        catch
        {
            throw;
        }
    }
    public void GetData_D(string bonus_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select BONUS_YEAR from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @bonus_year");
            ht.Add("@bonus_year", bonus_year);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_YEAR_D = Convert.ToString(dr["BONUS_YEAR"]);
            }
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_ID from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            //DataTable dt = dbConn.Query(sb, ht);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void Update(string tableName, string BONUS_ITEM_RP, string BONUS_ITEM_AL, string BONUS_ITEM_D, string yearDays, DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //紅利金額
            sb.Append(" Update " + tableName);
            sb.Append(" Set UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID = @FUNC_ID,");
             
            sb.Append(" BONUS_AMT =");
            sb.Append(" (select");
            sb.Append("  (LEVEL_PAY+ABILITY_PAY+PJOB_PAY+PROFESSION_PAY+FOOD_SUBSIDY)*");
            //sb.Append(" (  (select BONUS_DAYS from TB_S_M_BONUS_H where BONUS_YEAR=@BONUS_YEAR) + ");
            sb.Append(" " + this.BONUS_DAYS + " *");
            //反映項目-勤怠
            if (BONUS_ITEM_AL == "T")
            {
                sb.Append(" (WORK_DAYS+ATTEND_DAYS)/ (" + yearDays + "*30 ) +  ");
            }
            else
            {
                sb.Append(" (WORK_DAYS-0)/(" + yearDays + "*30 ) +   ");
            }


            //反映項目-獎懲  &&　反映項目-紀律
            if (BONUS_ITEM_RP == "T" && BONUS_ITEM_D == "T")
            {
                sb.Append(" (REWARD_DAYS+DISCIPLINE_DAYS) ");
            }
            else if (BONUS_ITEM_RP == "F" && BONUS_ITEM_D == "T")
            {
                sb.Append(" (0+DISCIPLINE_DAYS)  ");
            }
            else if (BONUS_ITEM_RP == "T" && BONUS_ITEM_D == "F")
            {
                sb.Append(" (REWARD_DAYS+0) ");
            }
            else
            {
                sb.Append("(0) ");
            }
            sb.Append("*(LEVEL_PAY+ABILITY_PAY+PJOB_PAY+PROFESSION_PAY+FOOD_SUBSIDY)/30  ");



            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and EMP_ID=@EMP_ID)");

            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SI020");
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();
            //紅利稅額
            sb.Append(" Update " + tableName);
            sb.Append(" Set BONUS_TAX =");
            sb.Append(" (select case");
            sb.Append(" when BONUS_AMT < 2000 then BONUS_AMT*0");
            sb.Append(" else BONUS_AMT *");
            sb.Append(" CAST(");
            sb.Append(" (select CODE_VAL1");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where MAIN_CD = 'BOUNS_TAX_RATE' and SYS_CD='SL' )");
            sb.Append(" AS decimal(18, 2))");
            sb.Append(" end");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and EMP_ID=@EMP_ID)");

            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");

            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();
            //紅利實額
            sb.Append(" Update " + tableName);
            sb.Append(" Set BONUS_AMT_R =");
            sb.Append(" (select BONUS_AMT-BONUS_TAX");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and EMP_ID=@EMP_ID)");

            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void UpdateDefault(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
             
            //紅利金額
            sb.Append(" Update TB_S_S_BONUS_D ");
            sb.Append(" Set UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID = @FUNC_ID");
            sb.Append(" ,BONUS_AMT   = ( select BONUS_AMT from TB_S_M_BONUS_D WHERE BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID) " );
            sb.Append(" ,BONUS_TAX   = ( select BONUS_TAX from TB_S_M_BONUS_D WHERE BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID) ");
            sb.Append(" ,BONUS_AMT_R = ( select BONUS_AMT_R from TB_S_M_BONUS_D WHERE BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID) ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SI020");
            dbConn.ExecuteT(sb, ht, true);
           
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void UpdateMain(string BONUS_ITEM_RP, string BONUS_ITEM_AL, string BONUS_ITEM_D, string yearDays, DateTime now)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //主檔的紅利總額及總人數
            sb.Append("Update TB_S_M_BONUS_H");
            sb.Append(" Set RELEASE_DT=@RELEASE_DT,RELEASE_BY=@RELEASE_BY,APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,APPROVE_STATUS=@APPROVE_STATUS");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID = @FUNC_ID,GEN_DT=@GEN_DT,");
            sb.Append(" BONUS_TOTAL_DECIMAL=");
            sb.Append(" (select count(*)");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and CHG_STATUS<>'D'),");
            sb.Append(" BONUS_TOTAL_AMOUNT=");
            sb.Append(" (select sum(BONUS_AMT)");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and CHG_STATUS<>'D'),");
            sb.Append(" BONUS_DAYS=@BONUS_DAYS,");
            if (BONUS_ITEM_AL == "T")
            {
                sb.Append(" BONUS_ITEM_AL='Y',");
            }
            else
            {
                sb.Append(" BONUS_ITEM_AL='N',");
            }

            if (BONUS_ITEM_RP == "T")
            {
                sb.Append(" BONUS_ITEM_RP='Y',");
            }
            else
            {
                sb.Append(" BONUS_ITEM_RP='N',");
            }

            if (BONUS_ITEM_D == "T")
            {
                sb.Append(" BONUS_ITEM_D='Y'");
            }
            else
            {
                sb.Append(" BONUS_ITEM_D='N'");
            }
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_DAYS", BONUS_DAYS);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@GEN_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@FUNC_ID", "FB2SI020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public void GetResult(string bonus_year, string bonus_days)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select BONUS_TOTAL_DECIMAL, BONUS_TOTAL_AMOUNT from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @bonus_year and BONUS_DAYS = @bonus_days");
            ht.Add("@bonus_year", bonus_year);
            ht.Add("@bonus_days", bonus_days);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_TOTAL_DECIMAL = Convert.ToString(dr["BONUS_TOTAL_DECIMAL"]);
                this.BONUS_TOTAL_AMOUNT = Convert.ToString(dr["BONUS_TOTAL_AMOUNT"]);
            }
        }
        catch
        {
            throw;
        }
    }
}