using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// WFB2SC2100 的摘要描述
/// </summary>
[Serializable]
public class WFB2SC2100Dateil2_UI_Data
{
    public string SALARY_YM;
    public DateTime? SALARY_DT;
    public string SALARY_TYPE;
    public string OPERATION_ID;
    public string OPERATION_NAME;
    public string SALARY_REQ;
    public string PROC_SOUCE;
    public string SALARY_LOCKED;
    public DateTime? PROCESS_DT;
    public DateTime? START_DT;
    public DateTime? END_DT;
    public string LoginUser;
}

public class WFB2SC2100DAO : BaseDAO
{
    public WFB2SC2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public int GetDateil2GridDataCount(int startRowIndex, int maximumRows, string SALARY_TYPE, string SALARY_YM, string SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select COUNT(1) total_record ");
        sb.AppendLine(" from(select t1.SALARY_TYPE ");
        sb.AppendLine("        	   ,t1.OPERATION_ID ");
        sb.AppendLine("        	   ,t1.OPERATION_NAME ");
        sb.AppendLine("        	   ,t1.SALARY_REQ ");
        sb.AppendLine("        	   ,t1.PROC_SOUCE  ");
        sb.AppendLine("        	   ,t2.SALARY_LOCKED ");
        sb.AppendLine("        	   ,t2.PROCESS_DT ");
        sb.AppendLine("        	   ,t2.START_DT,t2.END_DT	 ");
        sb.AppendLine("      from TB_S_M_SALARY_CTRL t1 		 ");
        sb.AppendLine("      left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE  ");
        sb.AppendLine("                                           and t1.OPERATION_ID = t2.OPERATION_ID  ");
        sb.AppendLine("                                           and t2.SALARY_YM = @SALARY_YM		 ");
        sb.AppendLine("      where t1.SALARY_TYPE = @SALARY_TYPE  ");
        sb.AppendLine("       and  t1.OPERATION_ID <> 'B01' 	 ");
        sb.AppendLine("        UNION    	 ");
        sb.AppendLine("        select t1.SALARY_TYPE ");
        sb.AppendLine("        	  ,t1.OPERATION_ID ");
        sb.AppendLine("        	  ,t1.OPERATION_NAME ");
        sb.AppendLine("        	  ,t1.SALARY_REQ ");
        sb.AppendLine("        	  ,t1.PROC_SOUCE  ");
        sb.AppendLine("        	  ,t2.SALARY_LOCKED ");
        sb.AppendLine("        	  ,t2.PROCESS_DT ");
        sb.AppendLine("        	  ,t2.START_DT ");
        sb.AppendLine("        	  ,t2.END_DT	 ");
        sb.AppendLine("        from TB_S_M_SALARY_CTRL t1  ");
        sb.AppendLine("        left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE  ");
        sb.AppendLine("                                             and t1.OPERATION_ID = t2.OPERATION_ID  ");
        sb.AppendLine("                                             and t2.SALARY_DT = @SALARY_DT    ");
        sb.AppendLine("        where t1.SALARY_TYPE = @SALARY_TYPE   ");
        sb.AppendLine("          and t1.OPERATION_ID = 'B01') A	 ");

        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);

        Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        return ReturnValue;
    }

    public DataTable GetDateil2GridData(int startRowIndex, int maximumRows, string SALARY_TYPE, string SALARY_YM, string SALARY_DT, string sortExpression)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select t1.SALARY_TYPE ");
        sb.AppendLine(" 	  ,t1.OPERATION_ID ");
        sb.AppendLine(" 	  ,t1.OPERATION_NAME ");
        sb.AppendLine(" 	  ,t1.SALARY_REQ ");
        sb.AppendLine(" 	  ,t1.PROC_SOUCE  ");
        sb.AppendLine(" 	  ,t2.SALARY_LOCKED ");
        sb.AppendLine(" 	  ,t2.PROCESS_DT ");
        sb.AppendLine(" 	  ,t2.START_DT	 ");
        sb.AppendLine(" 	  ,t2.END_DT	 ");
        sb.AppendLine(" 	  ,t2.SALARY_DT	 ");
        sb.AppendLine(" from TB_S_M_SALARY_CTRL t1 		 ");
        sb.AppendLine(" left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE  ");
        sb.AppendLine("                                      and t1.OPERATION_ID = t2.OPERATION_ID  ");
        sb.AppendLine("                                      and t2.SALARY_YM = @SALARY_YM		 ");
        sb.AppendLine(" where t1.SALARY_TYPE = @SALARY_TYPE  ");
        sb.AppendLine("  and  t1.OPERATION_ID <> 'B01' 	 ");
        sb.AppendLine(" UNION    	 ");
        sb.AppendLine(" select t1.SALARY_TYPE ");
        sb.AppendLine(" 	  ,t1.OPERATION_ID ");
        sb.AppendLine(" 	  ,t1.OPERATION_NAME ");
        sb.AppendLine(" 	  ,t1.SALARY_REQ ");
        sb.AppendLine(" 	  ,t1.PROC_SOUCE  ");
        sb.AppendLine(" 	  ,t2.SALARY_LOCKED ");
        sb.AppendLine(" 	  ,t2.PROCESS_DT ");
        sb.AppendLine(" 	  ,t2.START_DT ");
        sb.AppendLine(" 	  ,t2.END_DT	 ");
        sb.AppendLine(" 	  ,t2.SALARY_DT	 ");
        sb.AppendLine(" from TB_S_M_SALARY_CTRL t1  ");
        sb.AppendLine(" left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE  ");
        sb.AppendLine("                                      and t1.OPERATION_ID = t2.OPERATION_ID  ");
        sb.AppendLine("                                      and t2.SALARY_DT = @SALARY_DT    ");
        sb.AppendLine(" where t1.SALARY_TYPE = @SALARY_TYPE   ");
        sb.AppendLine("   and t1.OPERATION_ID = 'B01' 		 ");
        sb.AppendLine(" order By t1.OPERATION_ID	 ");

        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public void UpdateTB_S_M_SALARY_CAL_HByUnLock(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" update TB_S_M_SALARY_CAL_H ");
        sb.AppendLine("   set PROCESS_STATUS=@PROCESS_STATUS ");
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_DT=@SALARY_DT ");

        ht.Add("@PROCESS_STATUS", 1);
        ht.Add("@UPDATED_BY", dao.LoginUser);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));

        dbConn.ExecuteT(sb, ht);
    }

    public void DeleteTB_S_S_SALARY_PAYByUnLock(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" delete TB_S_S_SALARY_PAY ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_DT=@SALARY_DT ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));

        dbConn.ExecuteT(sb, ht);

    }
    public void DeleteTB_S_M_SALARY_MONTH_CTRL_ByUnLock(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" delete TB_S_M_SALARY_MONTH_CTRL ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_DT=@SALARY_DT ");
        sb.AppendLine("   and OPERATION_ID=@OPERATION_ID ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@OPERATION_ID", dao.OPERATION_ID);

        dbConn.ExecuteT(sb, ht);

    }
    public string GetTB_S_M_SALARY_CAL_H_PROCESS_STATUS(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select PROCESS_STATUS  ");
        sb.AppendLine(" from TB_S_M_SALARY_CAL_H  ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_DT=@SALARY_DT ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        DataTable Dt = dbConn.QueryT(sb, ht);
        if (Dt.Rows.Count > 0)
            return Convert.ToString(Dt.Rows[0]["PROCESS_STATUS"]);
        else
            return string.Empty;

    }

    public int CheckTB_S_M_SALARY_CAL_H(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count('1') CheckData  ");
        sb.AppendLine(" from TB_S_M_SALARY_CAL_H  ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_YM=@SALARY_YM ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_YM", dao.SALARY_YM);

        return Convert.ToInt16(dbConn.QueryT(sb, ht).Rows[0]["CheckData"]);

    }

    public void UpdateTB_S_M_SUBSIDY_DEDUCTIONS_1_Dateial2(WFB2SC2100Dateil2_UI_Data dao, bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" update TB_S_M_SUBSIDY_DEDUCTIONS_1 ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");
        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate()");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where 1=1 ");  //DATA_YM=@DATA_YM
        if (!Lock)
        {
            sb.AppendLine("   and SALARY_STATUS='Y' ");
            sb.AppendLine("   and SALARY_DT=@SALARY_DT ");
        }
        else
            sb.AppendLine("   and SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@UPDATED_BY", dao.LoginUser);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@DATA_YM", dao.SALARY_YM);

        dbConn.ExecuteT(sb, ht);
    }
    public void UpdateTB_S_M_SUBSIDY_DEDUCTIONS_D_Dateial2(WFB2SC2100Dateil2_UI_Data dao, bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" update TB_S_M_SUBSIDY_DEDUCTIONS_D ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");

        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate()");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where DATA_YM=@DATA_YM ");
        if (!Lock)
            sb.AppendLine("   and SALARY_STATUS='Y' ");
        else
            sb.AppendLine("   and SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@UPDATED_BY", dao.LoginUser);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@DATA_YM", dao.SALARY_YM);

        dbConn.ExecuteT(sb, ht);
    }
    public void UpdateTB_S_OTHER_BOUNS_D_Dateial2(WFB2SC2100Dateil2_UI_Data dao, bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" update TB_S_M_OTHER_BOUNS_D ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");
        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate() ");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        if (!Lock)
            sb.AppendLine(" where SALARY_STATUS='Y' and SALARY_DT = @SALARY_DT");
        else
            sb.AppendLine(" where SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@UPDATED_BY", dao.LoginUser);
        ht.Add("@FUNC_ID", "FB2SC210");

        dbConn.ExecuteT(sb, ht);
    }

    public void UpdateTB_S_M_SALARY_MONTH_CTRL_Dateial2(WFB2SC2100Dateil2_UI_Data dao, bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" update TB_S_M_SALARY_MONTH_CTRL ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_LOCKED='N' ");
            sb.AppendLine("      ,LOCK_DT=null ");
        }
        else
        {
            sb.AppendLine("   set SALARY_LOCKED='Y' ");
            sb.AppendLine("      ,LOCK_DT=getdate() ");
        }
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_DT=@SALARY_DT ");
        sb.AppendLine("   and OPERATION_ID=@OPERATION_ID ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@UPDATED_BY", dao.LoginUser);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@OPERATION_ID", dao.OPERATION_ID);

        dbConn.ExecuteT(sb, ht);
    }
    public void InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(WFB2SC2100Dateil2_UI_Data dao, string operation_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" insert into TB_S_M_SALARY_MONTH_CTRL (SALARY_TYPE,SALARY_YM,SALARY_DT,START_DT,END_DT,PROCESS_DT,OPERATION_ID,SALARY_LOCKED,LOCK_DT,FUNC_ID)  ");
        sb.AppendLine("   values( @SALARY_TYPE,@SALARY_YM,@SALARY_DT,@START_DT,@END_DT,GETDATE(),@OPERATION_ID,@SALARY_LOCKED,GETDATE(),@FUNC_ID) ");

        ht.Add("@SALARY_TYPE", dao.SALARY_TYPE);
        ht.Add("@SALARY_YM", dao.SALARY_YM);
        ht.Add("@SALARY_DT", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@START_DT", dao.START_DT);
        ht.Add("@END_DT", dao.END_DT);
        ht.Add("@OPERATION_ID", operation_id);
        ht.Add("@SALARY_LOCKED", "Y");
        ht.Add("@FUNC_ID", "FB2SC210");
        dbConn.ExecuteT(sb, ht);
    }
    public void ExecSPByLock(WFB2SC2100Dateil2_UI_Data dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("SP_S_EMP_DATA_MONTH_EXEC");

        ht.Add("@pSalaryType", dao.SALARY_TYPE);
        ht.Add("@pSalaryDate", Convert.ToDateTime(dao.SALARY_DT).ToString("yyyy/MM/dd"));
        ht.Add("@pSalaryYM", dao.SALARY_YM);
        ht.Add("@pUserID", dao.LoginUser);
        ht.Add("@pFuncID", "FB2SC210");

        dbConn.ExecuteSPT(sb, ht, true);
    }
    public DataTable checkSP(string proc_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}