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
/// CFB2SP0100DAO 的摘要描述
/// </summary>
public class CFB2SP0100DAO : BaseDAO
{
    public string COMPUTER_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string RETIRE_DT { get; set; }
    public string DELEGATE_YN { get; set; }
    public string DELEGATE_DT { get; set; }

    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    public CFB2SP0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //邏輯檢查 找尋此人是否為舊制或舊轉新制	
    public DataTable get3IN1_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_I_M_3IN1_TXN ");
            sb.Append(" where 1=1 ");
            sb.Append(" and INS_TYPE=@INS_TYPE  ");
            sb.Append(" and RC_TYPE=@RC_TYPE   ");
            sb.Append(" and EMP_ID = @EMP_ID  ");

            ht.Add("@INS_TYPE", "C");
            ht.Add("@RC_TYPE", "O");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //邏輯檢查 退休日必須大於TB_9_M_PARAMETER(參數檔) 系統上線日+6個月  
    public DataTable getOnLineDay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"  IF @RETIRE_DT >= (select DATEADD(MONTH , 6,CONVERT(datetime,CODE_VAL1))  as HR_ONLINE_DAY  from TB_9_M_PARAMETER WHERE  SYS_CD='SP' and MAIN_CD='HR_ONLINE_DAY'	) "
                      + " BEGIN  "
                      + " select 1 resultCount "
                      + " END "
                      + " ELSE "
                      + " BEGIN "
                      + " select 0 resultCount "
                      + " ,(select DATEADD(MONTH , 6,CONVERT(datetime,CODE_VAL1))  as HR_ONLINE_DAY  from TB_9_M_PARAMETER WHERE  SYS_CD='SP' and MAIN_CD='HR_ONLINE_DAY'	)  ON_LINE_DAY_ADDSIX_M "
                      + " END "
                      );
            ht.Add("@RETIRE_DT", RETIRE_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //邏輯檢查 退前六個月的薪資起算日 必須大於TB_9_M_PARAMETER(參數檔) 系統上線日 
    public DataTable getStartDayCompareOnLineDay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"  IF ( select dbo.FN_S_RETIRE_START_DT(@COMPUTER_TYPE,@EMP_ID,@RETIRE_DT) )  "
                      + "   >= (select DATEADD(MONTH , 6,CONVERT(datetime,CODE_VAL1))  as HR_ONLINE_DAY  from TB_9_M_PARAMETER WHERE  SYS_CD='SP' and MAIN_CD='HR_ONLINE_DAY'	) "
                      + " BEGIN  "
                      + " select 1 resultCount "
                      + " END "
                      + " ELSE "
                      + " BEGIN "
                      + " select 0 resultCount "
                      + " ,(select DATEADD(MONTH , 6,CONVERT(datetime,CODE_VAL1))  as HR_ONLINE_DAY  from TB_9_M_PARAMETER WHERE  SYS_CD='SP' and MAIN_CD='HR_ONLINE_DAY'	)  ON_LINE_DAY_ADDSIX_M "
                      + " ,(select dbo.FN_S_RETIRE_START_DT(@COMPUTER_TYPE,@EMP_ID,@RETIRE_DT))  as START_DT "
                      + " END "
                      );
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@RETIRE_DT", RETIRE_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //邏輯檢查 .已離職不必 試算
    public DataTable getLeaveDT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where 1=1 ");
            sb.Append(" and LEAVE_DT is not null ");
            sb.Append(" and EMP_ID = @EMP_ID  ");

            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //邏輯檢查 .此員工精算資料已切傳票,不允重新精算
    public DataTable getOLDRETIRE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_S_M_OLDRETIRE_H ");
            sb.Append(" where 1=1 ");
            sb.Append(" and CLOSE_YN = @CLOSE_YN  and EMP_ID = @EMP_ID");

            ht.Add("@CLOSE_YN", "Y");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //呼叫退休金計算的SP
    internal void execSP_S_RETIRE_COMPUTE()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_RETIRE_COMPUTE");
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@RETIRE_DT", Convert.ToDateTime(RETIRE_DT).ToString("yyyy/MM/dd"));
            ht.Add("@DELEGATE_YN", DELEGATE_YN);
            if (DELEGATE_DT != "")
            {
                ht.Add("@DELEGATE_DT", Convert.ToDateTime(DELEGATE_DT).ToString("yyyy/MM/dd"));
            }
            else
            {
                ht.Add("@DELEGATE_DT", DBNull.Value);
            }

            ht.Add("@USERID", CREATED_BY);
            ht.Add("@FUNCID", "FB2SP010");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }


}