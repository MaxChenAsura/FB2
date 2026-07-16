using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// utilities 的摘要描述
/// </summary>
public static class utilities
{
    public static string connstr = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    public static string Oleconnstr = ConfigurationManager.ConnectionStrings["Oledbconnection"].ConnectionString;
    public static string ODBCconnstr = ConfigurationManager.ConnectionStrings["ODBCconnection"].ConnectionString;
    public static string ACESconnstr = ConfigurationManager.ConnectionStrings["ACESdbConnection"].ConnectionString;
	public static string FF1connstr = ConfigurationManager.ConnectionStrings["FF1dbConnection"].ConnectionString;
	//public static string FB3connstr = ConfigurationManager.ConnectionStrings["FB3dbConnection"].ConnectionString;

    //IFLOW 連接的伺服器名稱
    public static string IFLOWName = ConfigurationManager.AppSettings["IFLOWName"];

    //ORACLE 連接的伺服器名稱
    public static string ORACLEName = ConfigurationManager.AppSettings["ORACLEName"];

    //AS400連接的伺服器名稱
    public static string AS400ServerName = ConfigurationManager.AppSettings["AS400ServerName"];
    public static string AS400RDBName = ConfigurationManager.AppSettings["AS400RDBName"];
	
	//FF1連接的伺服器名稱
    public static string FF1ServerName = ConfigurationManager.AppSettings["FF1Server"];
	//FB3連接的伺服器名稱
    public static string FB3ServerName = ConfigurationManager.AppSettings["FB3Server"];

    /// <summary>
    /// 取得共用代碼檔資料
    /// </summary>
    /// <param name="main_cd">代碼code</param>
    /// <param name="CODE_VAL1">大分類</param>
    /// <param name="CODE_VAL2">小分類</param>
    /// <returns></returns>
    public static DataTable getCommCode(string main_cd, string CODE_VAL1, string CODE_VAL2)
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc From TB_9_M_COMM_D Where main_cd = @main_cd ");
            ht.Add("@main_cd", main_cd);
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }
            sb.Append(" order by ORDER_SEQ");
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
    /// <summary>
    /// 取得公司代碼檔資料 2014/8/22
    /// </summary>
    /// <param name="wheresql">過濾條件值</param>
    /// <returns></returns>
    public static DataTable getCompany(string wheresql = "")
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COMPANY_CD as CODE ,COMPANY_CD+'-'+COMPANY_SNAME as CODE_NAME from TB_H_M_COMPANY ");
            if (wheresql != "")
            {
                sb.Append(" Where  " + wheresql);
            }

            sb.Append(" order by COMPANY_CD");
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// 取得共用代碼檔資料
    /// </summary>
    /// <param name="sys_cd">子作業</param>
    /// <param name="main_cd">代碼code</param>
    /// <param name="CODE_VAL1">大分類</param>
    /// <param name="CODE_VAL2">小分類</param>
    /// <returns></returns>
    public static DataTable getCommCode(string sys_cd, string main_cd, string CODE_VAL1, string CODE_VAL2, string IS_VALID = "Y")
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc, sub_desc as sub_desc2  From TB_9_M_COMM_D ");
            sb.Append(" Where main_cd = @main_cd and sys_cd = @sys_cd ");
            ht.Add("@main_cd", main_cd);
            ht.Add("@sys_cd", sys_cd);
            if (IS_VALID != "")
            {
                sb.Append("and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", IS_VALID);

            }
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }

            sb.Append(" order by ORDER_SEQ");
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
    /// <summary>
    /// 取得共用代碼檔資料
    /// </summary>
    /// <param name="sys_cd">子作業</param>
    /// <param name="main_cd">類別code</param>
    /// <param name="SUB_CD">代碼</param>
    /// <returns></returns>
    public static DataTable getCommCodeVal(string sys_cd, string main_cd, string SUB_CD, string IS_VALID = "Y")
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc, SUB_CD , SUB_DESC as SUB_DESC2 ,CODE_VAL1, CODE_VAL2  From TB_9_M_COMM_D ");
            sb.Append(" Where main_cd = @main_cd and sys_cd = @sys_cd ");
            ht.Add("@main_cd", main_cd);
            ht.Add("@sys_cd", sys_cd);
            if (SUB_CD != "")
            {
                sb.Append(" and SUB_CD = @SUB_CD ");
                ht.Add("@SUB_CD", SUB_CD);
            }
            if (IS_VALID != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", IS_VALID);

            }
            sb.Append(" order by ORDER_SEQ");
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// 取得參數檔資料
    /// </summary>
    /// <param name="sys_cd">子作業</param>
    /// <param name="main_cd">參數別</param>
    /// <returns></returns>
    public static DataTable getParameter(string sys_cd, string main_cd)
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1, MAIN_DESC, REMARK from  TB_9_M_PARAMETER ");
            sb.Append(" Where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }


    }

    /// <summary>
    /// 更改日勤務班表3
    /// </summary>
    /// <param name="dbConn">連線</param>
    /// <param name="EmpId">工號</param>
    /// <param name="ShiftCd">班別</param>
    /// <param name="WorkShiftCd">輪值表代碼</param>
    /// <param name="CALENDAR_DT_S">勤務日期起</param>
    /// <param name="CALENDAR_DT_E">勤務日期迄</param>
    /// <param name="UPDATED_BY">修改者工號</param>
    public static void UPD_EMP_DAY_DUTY3(DBConnector dbConn, string EmpId, string ShiftCd, string WorkShiftCd, DateTime CALENDAR_DT_S, DateTime CALENDAR_DT_E, string UPDATED_BY)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //1.依照輪值表的班別賦予
        if (string.IsNullOrEmpty(EmpId))
        {
            //1.1.更新日勤務班表資料檔
            sb.AppendLine(" UPDATE  TB_D_M_EMP_DAY_DUTY                                                                                ");
            sb.AppendLine(" Set [SHIFT_CD] = b.SHIFT_CD                                                                                ");
            sb.AppendLine("    ,[SHIFT_TIME_CD] = b.SHIFT_TIME_CD                                                                      ");
            sb.AppendLine("    ,[WORK_HOUR] = b.[WORK_HOUR]                                                                            ");
            sb.AppendLine("    ,[WORK_PERIOD_HOUR] = b.[WORK_PERIOD_HOUR]                                                              ");
            sb.AppendLine("    ,[DUTY_STIME] = b.[DUTY_STIME]                                                                          ");
            sb.AppendLine("    ,[DUTY_ETIME] = b.[DUTY_ETIME]                                                                          ");
            sb.AppendLine("    ,[WORK_SHIFT_ALLOWANCE_TYPE] = b.[WORK_SHIFT_ALLOWANCE_TYPE]				                               ");
            sb.AppendLine("    ,[UPDATED_BY] = @UPDATED_BY                                                                             ");
            sb.AppendLine("    ,[UPDATED_DT] = getDate()                                                                               ");
            sb.AppendLine("    ,[FUNC_ID] = @FuncID                                                                                    ");
            sb.AppendLine(" From TB_D_M_EMP_DAY_DUTY a                                                                                 ");
            sb.AppendLine(" Inner Join (                                                                                               ");
            sb.AppendLine(" 	select   A.CALENDAR_DT,C.WORK_DAY_CD,D.SHIFT_TIME_CD                                                   ");
            sb.AppendLine(" 			,D.WORK_HOUR,D.WORK_PERIOD_HOUR,dbo.FN_D_GEN_DATETIME(A.CALENDAR_DT,D.DUTY_STIME) DUTY_STIME   ");
            sb.AppendLine(" 			,dbo.FN_D_GEN_DATETIME(A.CALENDAR_DT,D.DUTY_ETIME) DUTY_ETIME                                  ");
            sb.AppendLine(" 			,D.WORK_SHIFT_ALLOWANCE_TYPE,B.CALENDAR_CD,A.WORK_SHIFT_CD                                     ");
            sb.AppendLine(" 			,D.SHIFT_CD                                                                                    ");
            sb.AppendLine(" 		from TB_D_M_WORK_SHIFT_D A                                                                         ");
            sb.AppendLine(" 	inner join TB_D_M_WORK_SHIFT_H B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD                                  ");
            sb.AppendLine(" 		left join [TB_D_M_CALENDAR_D] C on B.CALENDAR_CD = C.CALENDAR_CD and A.CALENDAR_DT = C.CALENDAR_DT ");
            sb.AppendLine(" 		left join TB_D_M_SHIFT_H D on A.SHIFT_CD = D.SHIFT_CD and A.CALENDAR_DT >= D.START_DT and          ");
            sb.AppendLine(" 				A.CALENDAR_DT <= D.END_DT                                                                  ");
            sb.AppendLine(" 	where A.WORK_SHIFT_CD = @WorkShiftCd                                                                   ");
            sb.AppendLine(" 		and A.CALENDAR_DT >= @CALENDAR_DT_S                                                                ");
            sb.AppendLine(" 		and A.CALENDAR_DT <= @CALENDAR_DT_E                                                                ");
            sb.AppendLine(" ) b On a.WORK_SHIFT_CD = b.WORK_SHIFT_CD and A.CALENDAR_DT = B.CALENDAR_DT;                                ");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@WorkShiftCd", WorkShiftCd);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            ht.Add("@FuncID", SessionHandle.Current.FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
            ht.Clear();
            sb.Clear();

            //1.2.更新日勤務狀態資料檔	 
            sb.AppendLine(" update TB_D_M_EMP_DUTY_CHECK_STATUS                                                                        ");
            sb.AppendLine("  set [DUTY_CHECK_RESULT] = 'N',                                                                            ");

            sb.AppendLine("  LATE_HOUR= 0,LEAVE_EARLY_HOUR= 0,LACK_HOUR= 0,DUTY_HOUR= 0,LEAVE_HOUR= 0                                 ");
            sb.AppendLine("  ,LEAVE_INFO= '',OVERTIME_HOUR_APPLY= 0,OVERTIME_HOUR_APPROVE= 0,VIOLATE_BEFORE_HOUR= 0                    ");
            sb.AppendLine("  ,VIOLATE_AFTER_HOUR= 0,OVERTIME_INFO= '',SHIFT_CD= '',WORK_SHIFT_ALLOWANCE_TYPE= '',                       ");

            sb.AppendLine("  UPDATED_BY = @UPDATED_BY,                                                                                 ");
            sb.AppendLine("  UPDATED_DT = getDate()                                                                                    ");
            sb.AppendLine(" From  TB_D_M_EMP_DUTY_CHECK_STATUS M                                                                       ");
            sb.AppendLine("    inner join TB_D_M_EMP_DAY_DUTY a                                                                        ");
            sb.AppendLine(" on M.EMP_ID = A.EMP_ID                                                                                     ");
            sb.AppendLine(" and M.CALENDAR_DT = A.CALENDAR_DT                                                                          ");
            sb.AppendLine("    Inner Join (                                                                                            ");
            sb.AppendLine(" 	select   A.CALENDAR_DT,C.WORK_DAY_CD,D.SHIFT_TIME_CD                                                   ");
            sb.AppendLine(" 			,D.WORK_HOUR,D.WORK_PERIOD_HOUR,dbo.FN_D_GEN_DATETIME(A.CALENDAR_DT,D.DUTY_STIME) DUTY_STIME   ");
            sb.AppendLine(" 			,dbo.FN_D_GEN_DATETIME(A.CALENDAR_DT,D.DUTY_ETIME) DUTY_ETIME                                  ");
            sb.AppendLine(" 			,D.WORK_SHIFT_ALLOWANCE_TYPE,B.CALENDAR_CD,A.WORK_SHIFT_CD                                     ");
            sb.AppendLine(" 			,D.SHIFT_CD                                                                                    ");
            sb.AppendLine(" 		from TB_D_M_WORK_SHIFT_D A                                                                         ");
            sb.AppendLine(" 	inner join TB_D_M_WORK_SHIFT_H B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD                                  ");
            sb.AppendLine(" 		left join [TB_D_M_CALENDAR_D] C on B.CALENDAR_CD = C.CALENDAR_CD and A.CALENDAR_DT = C.CALENDAR_DT ");
            sb.AppendLine(" 		left join TB_D_M_SHIFT_H D on A.SHIFT_CD = D.SHIFT_CD and A.CALENDAR_DT >= D.START_DT and          ");
            sb.AppendLine(" 				A.CALENDAR_DT <= D.END_DT                                                                  ");
            sb.AppendLine(" 	where A.WORK_SHIFT_CD = @WorkShiftCd                                                                   ");
            sb.AppendLine(" 		and A.CALENDAR_DT >= @CALENDAR_DT_S                                                                ");
            sb.AppendLine(" 		and A.CALENDAR_DT <= @CALENDAR_DT_E                                                                ");
            sb.AppendLine(" ) b On a.WORK_SHIFT_CD = b.WORK_SHIFT_CD and A.CALENDAR_DT = B.CALENDAR_DT			 		               ");
            sb.AppendLine(" where M.CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')			 		               ");

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@WorkShiftCd", WorkShiftCd);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            dbConn.ExecuteT(sb, ht, true);
        }
        else
        {
            //2.工號+勤務日期 => 指定特定的班別
            //2.1.更新日勤務班表資料檔
            sb.AppendLine(" UPDATE  TB_D_M_EMP_DAY_DUTY                                                                               ");
            sb.AppendLine(" Set [SHIFT_CD] = b.SHIFT_CD                                                                               ");
            sb.AppendLine("    ,[SHIFT_TIME_CD] = b.SHIFT_TIME_CD                                                                     ");
            sb.AppendLine("    ,[WORK_HOUR] = b.[WORK_HOUR]                                                                           ");
            sb.AppendLine("    ,[WORK_PERIOD_HOUR] = b.[WORK_PERIOD_HOUR]                                                             ");
            sb.AppendLine("    ,[DUTY_STIME] = b.[DUTY_STIME]                                                                         ");
            sb.AppendLine("    ,[DUTY_ETIME] = b.[DUTY_ETIME]                                                                         ");
            sb.AppendLine("    ,[WORK_SHIFT_ALLOWANCE_TYPE] = b.[WORK_SHIFT_ALLOWANCE_TYPE]				                              ");
            sb.AppendLine("    ,[UPDATED_BY] = @UPDATED_BY                                                                            ");
            sb.AppendLine("    ,[UPDATED_DT] = getDate()                                                                              ");
            sb.AppendLine("    ,[FUNC_ID] = @FuncID                                                                                   ");
            sb.AppendLine(" From TB_D_M_EMP_DAY_DUTY a                                                                                ");
            sb.AppendLine(" Inner Join (                                                                                              ");
            sb.AppendLine(" 	  select D.SHIFT_TIME_CD                                                                              ");
            sb.AppendLine(" 			,D.WORK_HOUR,D.WORK_PERIOD_HOUR,dbo.FN_D_GEN_DATETIME(@CALENDAR_DT_S,D.DUTY_STIME) DUTY_STIME ");
            sb.AppendLine(" 			,dbo.FN_D_GEN_DATETIME(@CALENDAR_DT_S,D.DUTY_ETIME) DUTY_ETIME                                ");
            sb.AppendLine(" 			,D.WORK_SHIFT_ALLOWANCE_TYPE,D.SHIFT_CD                                                       ");
            sb.AppendLine(" 		from TB_D_M_SHIFT_H D                                                                             ");
            sb.AppendLine("        where @CALENDAR_DT_S >= D.START_DT                                                                 ");
            sb.AppendLine(" 	     and @CALENDAR_DT_E <= D.END_DT                                                                   ");
            sb.AppendLine(" 		 and SHIFT_CD = @ShiftCd                                                                          ");
            sb.AppendLine(" ) b                                                                                                       ");
            sb.AppendLine("  On a.CALENDAR_DT = @CALENDAR_DT_S                                                                        ");
            sb.AppendLine("  and a.EMP_ID = @EmpId                                                                                    ");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            //ht.Add("@WorkShiftCd", WorkShiftCd);
            ht.Add("@ShiftCd", ShiftCd);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            ht.Add("@EmpId", EmpId);
            ht.Add("@FuncID", SessionHandle.Current.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            ht.Clear();
            sb.Clear();

            sb.AppendLine(" --2.2.更新日勤務狀態資料檔	                                                                             ");
            sb.AppendLine(" update TB_D_M_EMP_DUTY_CHECK_STATUS                                                                      ");
            sb.AppendLine("   set [DUTY_CHECK_RESULT] = 'N',                                                                         ");
            sb.AppendLine("  LATE_HOUR= 0,LEAVE_EARLY_HOUR= 0,LACK_HOUR= 0,DUTY_HOUR= 0,LEAVE_HOUR= 0                                 ");
            sb.AppendLine("  ,LEAVE_INFO= '',OVERTIME_HOUR_APPLY= 0,OVERTIME_HOUR_APPROVE= 0,VIOLATE_BEFORE_HOUR= 0                    ");
            sb.AppendLine("  ,VIOLATE_AFTER_HOUR= 0,OVERTIME_INFO= '',SHIFT_CD= '',WORK_SHIFT_ALLOWANCE_TYPE= '',                       ");
            sb.AppendLine(" 	   UPDATED_BY = @UPDATED_BY,                                                                         ");
            sb.AppendLine(" 	   UPDATED_DT = getDate()                                                                            ");
            sb.AppendLine(" where EMP_ID = @EmpId                                                                                   ");
            sb.AppendLine("   and CALENDAR_DT = @CALENDAR_DT_S                                                                       ");
            sb.AppendLine("   and CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')                                                                      ");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@EmpId", EmpId);
            dbConn.ExecuteT(sb, ht, true);

        }
    }

    //更改行事曆時呼叫,修改班表
    public static void UPD_EMP_DAY_DUTY2(DBConnector dbConn, string CALENDAR_CD, DateTime CALENDAR_DT_S, DateTime CALENDAR_DT_E, string UPDATED_BY)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        try
        {
            //1.更新日勤務班表資料檔			  
            sb.AppendLine(" UPDATE A            ");
            sb.AppendLine("    set    ");
            sb.AppendLine("        WORK_DAY_CD = B.WORK_DAY_CD,   ");
            sb.AppendLine("        UPDATED_BY = @UPDATED_BY,      ");
            sb.AppendLine(" 	   UPDATED_DT = getDate()         ");
            sb.AppendLine(" from  ( select * from  TB_D_M_EMP_DAY_DUTY  where CALENDAR_DT >= convert(varchar(10),getdate(),112) and CALENDAR_CD=@CALENDAR_CD  )A            ");
            sb.AppendLine(" inner join TB_D_M_CALENDAR_D B        ");
            sb.AppendLine(" on  A.CALENDAR_DT >= @CALENDAR_DT_S   ");
            sb.AppendLine(" and A.CALENDAR_DT <= @CALENDAR_DT_E   ");
            sb.AppendLine(" and A.CALENDAR_CD = @CALENDAR_CD      ");
            sb.AppendLine(" and A.CALENDAR_CD = B.CALENDAR_CD     ");
            sb.AppendLine(" and A.CALENDAR_DT = B.CALENDAR_DT     ");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            dbConn.ExecuteT(sb, ht, true);
            ht.Clear();
            sb.Clear();
            //2.更新日勤務狀態資料檔              
            sb.AppendLine(" update TB_D_M_EMP_DUTY_CHECK_STATUS   ");
            sb.AppendLine("   set [DUTY_CHECK_RESULT] = 'N',      ");
            sb.AppendLine("   LATE_HOUR= 0,LEAVE_EARLY_HOUR= 0,LACK_HOUR= 0,DUTY_HOUR= 0,LEAVE_HOUR= 0                                 ");
            sb.AppendLine("  ,LEAVE_INFO= '',OVERTIME_HOUR_APPLY= 0,OVERTIME_HOUR_APPROVE= 0,VIOLATE_BEFORE_HOUR= 0                    ");
            sb.AppendLine("  ,VIOLATE_AFTER_HOUR= 0,OVERTIME_INFO= '',SHIFT_CD= '',WORK_SHIFT_ALLOWANCE_TYPE= '',                       ");
            sb.AppendLine("        UPDATED_BY = @UPDATED_BY,      ");
            sb.AppendLine(" 	   UPDATED_DT = getDate()         ");
            sb.AppendLine("  from  TB_D_M_EMP_DUTY_CHECK_STATUS M ");
            sb.AppendLine(" inner join TB_D_M_EMP_DAY_DUTY A      ");
            sb.AppendLine(" on M.EMP_ID = A.EMP_ID                ");
            sb.AppendLine(" and M.CALENDAR_DT = A.CALENDAR_DT     ");
            sb.AppendLine(" inner join TB_D_M_CALENDAR_D B        ");
            sb.AppendLine(" on  A.CALENDAR_DT >= @CALENDAR_DT_S   ");
            sb.AppendLine(" and A.CALENDAR_DT <= @CALENDAR_DT_E   ");
            sb.AppendLine(" and A.CALENDAR_CD = @CALENDAR_CD      ");
            sb.AppendLine(" and A.CALENDAR_CD = B.CALENDAR_CD     ");
            sb.AppendLine(" and A.CALENDAR_DT = B.CALENDAR_DT     ");
            sb.AppendLine(" where M.CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')     ");

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    ///<summary>
    ///字串轉全形
    ///</summary>
    ///<param name="input">任一字元串</param>
    ///<returns>全形字元串</returns>
    public static string toWide(string input)
    {
        //半形轉全形：
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            //全形空格為12288，半形空格為32
            if (c[i] == 32)
            {
                c[i] = (char)12288;
                continue;
            }
            //其他字元半形(33-126)與全形(65281-65374)的對應關係是：均相差65248
            if (c[i] < 127)
                c[i] = (char)(c[i] + 65248);
        }
        return new string(c);
    }

    ///<summary>
    ///字串轉半形
    ///</summary>
    ///<paramname="input">任一字元串</param>
    ///<returns>半形字元串</returns>
    public static string toNarrow(string input)
    {
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }
            if (c[i] > 65280 && c[i] < 65375)
                c[i] = (char)(c[i] - 65248);
        }
        return new string(c);
    }

    ///<summary>
    ///分鐘轉小時
    ///</summary>
    ///<paramname="input">分鐘字串</param>
    ///<returns>HH:MM格式</returns>
    public static string toHourMinute(string input)
    {
        int minute;
        if (int.TryParse(input, out minute))
        {
            int hour = 0;
            if (minute >= 60)
            {
                hour = minute / 60;
                return hour.ToString().PadLeft(2, '0') + ":" + (minute % 60).ToString().PadLeft(2, '0');
            }
            else if (minute > 0)
                return "00:" + minute.ToString().PadLeft(2, '0');
            else
                return "";
        }
        else
        {
            return "";
        }

    }

    ///<summary>
    ///小時轉分鐘
    ///</summary>
    ///<paramname="input">小時字串</param>
    ///<returns>HH:MM格式</returns>
    public static int HourToMinute(string input)
    {
        int hour = 0;
        int minute = 0;
        string[] saTemp;

        if (input != "")
        {
            saTemp = input.Split(':');

            hour = Convert.ToInt32(saTemp[0]) * 60;
            minute = Convert.ToInt32(saTemp[1]);
            minute = minute + hour;
        }
        else
        {
            return 0;
        }
        return minute;
    }

    ///<summary>
    ///數字轉國字
    ///</summary>
    ///<paramname="input">數字</param>
    ///<returns>國字</returns>
    public static string GetChineseNumber(int number)
    {
        string[] chineseNumber = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        string[] unit = { "", "十", "百", "千", "萬", "十萬", "百萬", "千萬", "億", "十億", "百億", "千億", "兆", "十兆", "百兆", "千兆" };
        StringBuilder ret = new StringBuilder();
        string inputNumber = number.ToString();
        int idx = inputNumber.Length;
        bool needAppendZero = false;
        foreach (char c in inputNumber)
        {
            idx--;
            if (c > '0')
            {
                if (needAppendZero)
                {
                    ret.Append(chineseNumber[0]);
                    needAppendZero = false;
                }
                ret.Append(chineseNumber[(int)(c - '0')] + unit[idx]);
            }
            else
                needAppendZero = true;
        }
        return ret.Length == 0 ? chineseNumber[0] : ret.ToString();
    }

    ///<summary>
    ///日期時間轉民國年
    ///</summary>
    ///<paramname="input">西元日期時間(yyyy/MM/dd HH:mm:ss)</param>
    ///<returns>民國年日期時間</returns>
    public static string DateTimeToTw(string datetime, string split = "/")
    {
        DateTime tmp;
        string rtnval = "";
        if (DateTime.TryParse(datetime, out tmp))
        {
            int twyear = tmp.Year - 1911;
            string month = tmp.Month.ToString().PadLeft(2, '0');
            string day = tmp.Day.ToString().PadLeft(2, '0');
            string time = tmp.ToString("HH:mm:ss");

            rtnval = twyear.ToString().PadLeft(3, '0') + split + month + split + day + " " + time;

        }


        return rtnval;
    }
    ///<summary>
    ///日期轉民國年
    ///</summary>
    ///<paramname="input">西元日期(yyyy/MM/dd)</param>
    ///<returns>民國年日期</returns>
    public static string DateToTw(string datetime, string split = "/")
    {
        DateTime tmp;
        string rtnval = "";
        if (DateTime.TryParse(datetime, out tmp))
        {
            int twyear = tmp.Year - 1911;
            string month = tmp.Month.ToString().PadLeft(2, '0');
            string day = tmp.Day.ToString().PadLeft(2, '0');

            rtnval = twyear.ToString().PadLeft(3, '0') + split + month + split + day;

        }


        return rtnval;
    }

    ///<summary>
    ///日期年月轉民國年
    ///</summary>
    ///<paramname="input">西元日期年月(yyyyMM)</param>
    ///<returns>民國年月</returns>
    public static string DateMonthToTw(string datetime, string split = "/")
    {
        string rtnval = "";
        if (datetime.Length == 6)
        {
            int twyear = int.Parse(datetime.Substring(0, 4)) - 1911;
            string month = datetime.Substring(4);
            rtnval = twyear.ToString().PadLeft(3, '0') + split + month;

        }


        return rtnval;
    }
    /// <summary>
    /// sql 組出in語法
    /// </summary>
    /// <param name="comm">sqlcommand</param>
    /// <param name="param">in 裡面的條件，以,串聯</param>
    /// <returns></returns>
    //public static SqlCommand sqlIn(SqlCommand comm, string param)
    //{
    //    try
    //    {
    //        if (param != "")
    //        {
    //             sb.Append("(");
    //            if (param.Contains(','))
    //            {
    //                List<string> tmp = param.Split(',').ToList();
    //                for (int i = 0; i < tmp.Count; i++)
    //                {
    //                     sb.Append("@param" + i.ToString() + ",");
    //                    ht.Add("@param" + i.ToString(), tmp[i]);
    //                }
    //                sb.Append(comm.CommandText.Substring(0, comm.CommandText.Length - 1);
    //            }
    //            else
    //            {
    //                 sb.Append("@param0";
    //                ht.Add("@param0", param);
    //            }
    //             sb.Append(")";
    //        }

    //        return comm;
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }


    //}

    /// <summary>
    /// 寄送email
    /// </summary>
    /// <param name="subject">主旨</param>
    /// <param name="content">內容</param>
    /// <param name="send">寄信者</param>
    /// <param name="mailto">收信者</param>
    /// <param name="html">是否為html</param>
    /// <param name="file_name">附件名稱</param>
    /// <param name="attch">附件</param>
    public static void SendMail2(string subject, string content, string send, List<string> mailto, bool html = false, string file_name = "", Stream attch = null)
    {

        try
        {
            string smtp_server = ConfigurationManager.AppSettings["smtpServer"];

            SmtpClient mySmtp = new SmtpClient(smtp_server);
            //設定smtp帳密

            mySmtp.UseDefaultCredentials = false;
            mySmtp.Credentials = new System.Net.NetworkCredential("", "");//輸入smpt server及密碼

            //}
            //信件內容
            string pcontect = content;
            //設定mail內容
            MailMessage msgMail = new MailMessage();
            //寄件者
            msgMail.From = new MailAddress(send);
            //收件者
            foreach (var item in mailto)
            {
                msgMail.To.Add(item);
            }
            //主旨
            msgMail.Subject = subject;
            if (attch != null)
            {
                Attachment messageAttachment = new Attachment(attch, file_name);
                msgMail.Attachments.Add(messageAttachment);
            }

            //信件內容(含HTML時)
            if (html)
            {
                AlternateView alt = AlternateView.CreateAlternateViewFromString(pcontect, null, "text/html");

                msgMail.AlternateViews.Add(alt);
            }
            else
                msgMail.Body = pcontect;
            //寄mail
            mySmtp.Send(msgMail);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// 寄送email
    /// </summary>
    /// <param name="subject">主旨</param>
    /// <param name="content">內容</param>
    /// <param name="send">寄信者</param>
    /// <param name="mailto">收信者</param>
    /// <param name="html">是否為html</param>
    /// <param name="file_name">附件名稱</param>
    /// <param name="attch">附件</param>
    public static void SendMail(string subject, string content, string send, List<string> mailto, bool html = false, string file_name = "", Stream attch = null)
    {

        try
        {
            string smtp_server = ConfigurationManager.AppSettings["smtpServer"];
            //smtp_server = "smtp.gmail.com";
            SmtpClient mySmtp = new SmtpClient(smtp_server);
            //設定smtp帳密
            //if (smtp_usr_id != "" && smtp_password != "")
            //{

            //mySmtp.EnableSsl = true;
            //mySmtp.UseDefaultCredentials = false;
            //mySmtp.Credentials = new System.Net.NetworkCredential("", "");//輸入smpt server及密碼

            //}
            //信件內容
            string pcontect = content;
            //設定mail內容
            MailMessage msgMail = new MailMessage();
            //寄件者
            msgMail.From = new MailAddress(ConfigurationManager.AppSettings["smtpServerMail"]);
            //收件者
            foreach (var item in mailto)
            {
                msgMail.To.Add(item);
            }
            //主旨
            msgMail.Subject = subject;
            if (attch != null)
            {

                Attachment messageAttachment = new Attachment(attch, file_name);
                msgMail.Attachments.Add(messageAttachment);

            }

            //信件內容(含HTML時)
            if (html)
            {
                AlternateView alt = AlternateView.CreateAlternateViewFromString(pcontect, null, "text/html");

                msgMail.AlternateViews.Add(alt);
            }
            else
                msgMail.Body = pcontect;
            //寄mail
            mySmtp.Send(msgMail);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public static bool IsNatural_Number(string str)
    {

        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");

        return reg1.IsMatch(str);

    }
    //判斷是否為潤年
    public static bool isLeapYear(int year)
    {
        bool result = false;
        if (year % 4 == 0)
        {
            result = true;
        }
        if (year % 100 == 0)
        {
            result = false;
        }
        if (year % 400 == 0)
        {
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 檢查最大長度
    /// </summary>
    /// <param name="cellData">檢核資料</param>
    /// <param name="cellName">要回傳的名稱,如工號,部門代號之類</param>
    /// <param name="cellLength">該欄位的長度</param>
    /// <param name="isEmpty">是否可空白;true 可空白, false 不可空白</param>
    /// <returns>錯誤訊息</returns>
    public static string checkLength(string cellData, string cellName, int cellLength, bool isEmpty)
    {
        string error = "";
        if (isEmpty == false && cellData == "")
        {
            error += cellName + "不可空白,\n";
        }
        else if (cellData != "")
        {
            if (cellData.Trim().Length > cellLength)
            {
                error += cellName + "長度最大為" + cellLength + ", \n";
            }
        }
        return error;
    }


    /// <summary>
    /// 檢查是否為數字(正整數)
    /// </summary>
    /// <param name="cellData">檢核資料</param>
    /// <param name="cellName">要回傳的名稱,如工號,部門代號之類</param>
    /// <param name="cellLength">該欄位的長度</param>
    /// <param name="isEmpty">是否可空白;true 可空白, false 不可空白</param>
    /// <returns>錯誤訊息</returns>
    public static string checkNumber(string cellData, string cellName, int cellLength, bool isEmpty)
    {
        try
        {
            string error = "";
            int numCheckResult = 0;
            cellData = cellData.Replace(",", "");//數字也許會有千分號
            if (isEmpty == false && cellData == "")
                error += cellName + "不可空白,\n";
            else if (cellData != "")
            {
                if (cellData.Trim().Length > cellLength || !int.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error += cellName + "必須為數字, 且長度最大為" + cellLength + ", \n";
                }
            }

            return error;
        }
        catch (Exception)
        {
            return cellName + "必須為數字, 且長度最大為" + cellLength + ", \n";
        }
    }


    /// <summary>
    /// 檢查是否為英數字(固定長度)
    /// </summary>
    /// <param name="cellData">檢核資料</param>
    /// <param name="cellName">要回傳的名稱,如工號,部門代號之類</param>
    /// <param name="cellLength">該欄位的長度</param>
    /// <param name="isEmpty">是否可空白;true 可空白, false 不可空白</param>
    /// <returns>錯誤訊息</returns>
    public static string checkEngNumber_fixLength(string cellData, string cellName, int cellLength, bool isEmpty)
    {
        try
        {
            string error = "";
            if (isEmpty == false && cellData == "")
            {
                error += cellName + "不可空白,\n";
            }
            else if (cellData != "")
            {
                if (cellData.Trim().Length != cellLength || !utilities.IsNatural_Number(cellData))
                {
                    error += cellName + "必須為英數字, 且長度須為" + cellLength + ", \n";
                }
            }
            return error;
        }
        catch (Exception)
        {
            return cellName + "必須為英數字, 且長度須為" + cellLength + ", \n";
        }
    }

    /// <summary>
    /// 檢查是否為日期格式 
    /// </summary>
    /// <param name="cellData">檢核資料</param>
    /// <param name="cellName">要回傳的名稱,如工號,部門代號之類</param>
    /// <param name="isEmpty">是否可空白;true 可空白, false 不可空白</param>
    /// <returns>錯誤訊息</returns>
    public static string checkDateFormat(string cellData, string cellName, bool isEmpty)
    {
        try
        {
            DateTime dt3;
            string error = "";
            if (isEmpty == false && cellData == "")
                error += cellName + "不可空白,\n";
            else if (cellData != "")
            {
                if (DateTime.TryParse(cellData, out dt3) == false)
                {
                    error += cellName + "日期格式錯誤,格式為yyyy/mm/dd, \n";
                }
            }
            return error;
        }
        catch (Exception)
        {
            return cellName + "日期格式錯誤,格式為yyyy/mm/dd, \n";
        }
    }

    //取得已最後已計薪的年月月底
    public static string getSalaryDT()
    {
        string salaryDT = string.Empty;
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT [dbo].[FN_S_SALARY_YM] ()  as salaryYM ");

            DataTable dt = dbConn.Query(sb, ht);
            string salaryYM = dt.Rows[0]["salaryYM"].ToString();
            salaryDT = Convert.ToDateTime(salaryYM.Substring(0, 4) + "/" + salaryYM.Substring(4, 2) + "/01").AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
            return salaryDT;

        }
        catch
        {
            throw;
        }
    }

    #region 權限
    //依部門權限及可管理部門取得管理人員的員工
    public static List<string> getAcesEMP_LIST()
    {
        try
        {
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string super = SessionHandle.Current.is_super;
            string departments = SessionHandle.Current.departments; ;
            if (super == "Y")
            {
                sb.Append(" select V.EMP_ID  from VW_H_EMP_DATA V  with (nolock)  ");
            }
            else
            {
                sb.Append(@"  select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@EMP_ID,@departments)  ");
                ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
                ht.Add("@departments", departments);
            }

            DataTable dt = dbConn.Query(sb, ht);
            List<string> Emps = new List<string>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Emps.Add(dt.Rows[i]["EMP_ID"].ToString());
                }

            }
            return Emps;
            /*
            tmp += "Select e.EMP_ID";
            tmp += " from VW_H_EMP_DATA e ";

            //ACES權限
            ACESLib.ACES aces = new ACESLib.ACES();

            //取得角色資料權限
            String dbRole = aces.GetRoles();
            IList<string> role = dbRole.Split(',');

            foreach (string item in role)
            {
                //取得部門權限聯集
                try
                {
                    string derolecd = item.Trim();
                    string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd)).SysCode; //取得部門權限聯集 「大分類代碼」
                    foreach (string code in SysCode.Split(','))
                    {
                        if (code.Trim().Equals("SUPER"))
                        {
                            super = "Y";
                            break;
                        }
                    }

                }
                catch (Exception)
                {

                }

            }
            if (super == "N")
            {
                List<string> depts = getDepts();
                if (depts != null)
                {
                    string departments = "";
                    foreach (string depart in depts)
                    {
                        departments += ",'" + depart+"'";
                    }

                    tmp += " left join VW_H_DEPT_DATA d on e.DEPT_NO = d.DEPT_NO ";
                    tmp += " WHERE e.dept_no in  (" + departments.Trim(',') + ") ";
                    ht.Add("@emp_id", SessionHandle.Current.emp_id);
                }
                else
                {
                    tmp += " WHERE e.EMP_ID= @emp_id";
                    ht.Add("@emp_id", SessionHandle.Current.emp_id);
                }
            }
            sb.Append(tmp);
            */


        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷查詢工號是否可查詢
    public static bool checkAuth(string emp_id)
    {
        try
        {
            bool result = false;
            List<string> Emps = getAcesEMP_LIST();
            if (Emps.Contains(emp_id.Trim()))
                result = true;
            if (emp_id.Trim() == "")
                result = true;
            return result;
        }
        catch
        {
            return false;
        }

    }
    /*
    public static List<string> getDepts()
    {
        try
        {
            Dept_Search tv = new Dept_Search();
            string sp_dept = "";
            string header = "N";
            List<string> dept;
            List<string> spDept;
            //ACES權限
            ACESLib.ACES aces = new ACESLib.ACES();

            //取得角色資料權限
            String dbRole = aces.GetRoles();
            IList<string> role = dbRole.Split(',');

            foreach (string item in role)
            {
                //取得部門權限聯集
                try
                {
                    string derolecd = item.Trim();
                    string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd)).SysCode; //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        if (code.Trim().Equals("SUPER"))
                        {
                            header = "Y";
                            break;
                        }
                    }

                    ACESLib.DEPTBean deptbean = (ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd);
                    sp_dept += deptbean.Departments; //取得 「使用其它部門權限」

                }
                catch (Exception)
                {

                }


            }

            spDept = new List<string>();
            dept = new List<string>();

            dept = tv.getHead_Dept(SessionHandle.Current.emp_id);
            if (dept.Count() == 0)  //無可選部門則只能選擇自己部門及特殊部門
                header = "N";
            else
                header = "Y";

            spDept = sp_dept.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            dept.AddRange(spDept);
            dept.Add(SessionHandle.Current.dept_no);

            return dept;
        }
        catch (Exception)
        {

            throw;
        }
    }
    */

    #endregion

    /// <summary>
    /// 判斷字串是否為正確的電子郵件地址
    /// </summary>
    /// <param name="mailAddress">電子郵件地址</param>
    /// <returns>傳回布林值</returns>
    public static bool IsMailAddress(string mailAddress)
    {
        bool result = false;
        try
        {
            System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(mailAddress);
            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }

    //取得該員工View的資料
    public static DataTable getEmpData(string emp_id)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from VW_H_EMP_DATA  with (nolock)  ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id.Trim());
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得SP的錯誤訊息
    public static string getSPLOG(string proc_id)
    {
        try
        {
            string rtnmessage = "";
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select proc_status, proc_log from TB_H_R_SP_LOG  ");
            sb.Append(" where PROC_ID= @PROC_ID ");
            sb.Append(" and PROC_DT=(select max(PROC_DT)  maxb from TB_H_R_SP_LOG  where PROC_ID= @PROC_ID    ) ");
            ht.Add("@PROC_ID", proc_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["proc_status"].ToString() != "Y")
                {
                    rtnmessage += dt.Rows[0]["proc_log"].ToString() + " \\n";
                }
            }
            else
            {
                rtnmessage = "";
            }
            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
    }


    //utf8轉big5
    public static string convertBig5(string strUtf)
    {
        string result = "";
        Encoding uft81 = Encoding.GetEncoding("UTF-8");
        Encoding big51 = Encoding.GetEncoding("BIG5");

        byte[] strUtf81 = uft81.GetBytes(strUtf.Trim());
        byte[] strBig51 = Encoding.Convert(uft81, big51, strUtf81);
        char[] big5Chars1 = new char[big51.GetCharCount(strBig51, 0, strBig51.Length)];
        big51.GetChars(strBig51, 0, strBig51.Length, big5Chars1, 0);
        result = new string(big5Chars1);
        return result;
    }

    //比較日期是否已計薪-已計薪true, 未計薪false
    public static bool isSalaryDate(string date)
    {
        try
        {

            bool rtnResult = false;
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @startDT datetime=@DATE; 
                        if  dbo.FN_S_SALARY_YM() >=CONVERT(VARCHAR(6),@startDT,112)  
                        BEGIN
	                        select 1 resultCount, dbo.FN_S_SALARY_YM() salaryYM 
                        END
                        else 
                        BEGIN
	                        select 0 resultCount, dbo.FN_S_SALARY_YM() salaryYM 
                        END
                        ");
            ht.Add("@DATE", date);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                if ((int)dt.Rows[0]["resultCount"] == 1)
                {
                    rtnResult = true;
                }
            }
            else
            {
                rtnResult = false;
            }

            return rtnResult;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //依不同的功能給予角色權限設定
    public static void setAuthData()
    {
        try
        {
            string is_super = "N";
            string is_dept = "N";     //取得 「是否含部門以下」
            string departments_result = "";
            SessionHandle.Current.is_super = is_super;
            SessionHandle.Current.departments = departments_result;
            SessionHandle.Current.is_dept = is_dept;

            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            List<string> all_departments = new List<string>();
            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                try
                {
                    string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                    if (string.IsNullOrEmpty(derolecd))
                    {
                        continue;
                    }
                    ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                    string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                    string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                    string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                        if (code.Trim().Equals("SUPER"))
                        {
                            is_super = "Y";
                            break;
                        }
                    }
                    if (dept == "Y")
                        is_dept = "Y";
                    all_departments.Add(departments);
                }
                catch (Exception)
                {

                }
            }

            if (all_departments.Count > 0)
            {
                string final_departments = "";
                List<string> departments = new List<string>();
                for (int i = 0; i < all_departments.Count; i++)
                {
                    for (int k = 0; k < all_departments[i].Split(',').Length; k++)
                    {
                        string temp = all_departments[i].Split(',')[k].Trim();
                        if (departments.Contains(temp))
                            continue;

                        departments.Add(temp);
                    }
                }

                for (int i = 0; i < departments.Count; i++)
                {
                    if (i == 0)
                    {
                        final_departments = departments[i];
                        continue;
                    }
                    final_departments += "," + departments[i];
                }

                departments_result = final_departments;
            }
            SessionHandle.Current.is_super = is_super;
            SessionHandle.Current.departments = departments_result;
            SessionHandle.Current.is_dept = is_dept;
        }
        catch (Exception ex)
        {
        }

    }

    //依傳入日期,取得全部班表的資料
    public static DataTable getShiftCD(string calendar_dt)
    {
        DBConnector dbConn = new DBConnector();
        if (string.IsNullOrEmpty(calendar_dt))
        {
            calendar_dt = DateTime.Now.ToString("yyyy/MM/dd");
        }

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SHIFT_CD,SHIFT_CD+'-'+SHIFT_DESC as SHIFT_DESC,DUTY_STIME+'-'+DUTY_ETIME as DUTY_TIME   ");
            sb.Append(" from TB_D_M_SHIFT_H H ");
            sb.Append(" WHERE  @calendar_dt between H.START_DT and H.END_DT ");
            ht.Add("@calendar_dt", calendar_dt);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //判斷該員工的資格是否 >= 要比較的資格,如'10003','2B',10003的資格是否在2B(含)以上
    public static bool is_LEVEL_UP(string EMP_ID, string c_LEVEL_CD) {
        try
        {
            bool rtnResult = false;
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select dbo.FN_H_CHECK_LEVEL(@EMP_ID,@c_LEVEL_CD) as resultCount
                        ");
            ht.Add("EMP_ID", EMP_ID);
            ht.Add("c_LEVEL_CD", c_LEVEL_CD);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                if ((int)dt.Rows[0]["resultCount"] == 1)
                {
                    rtnResult = true;
                }
            }
            else
            {
                rtnResult = false;
            }

            return rtnResult;
        }
        catch (Exception)
        {

            throw;
        }
    }


    /// <summary>
    /// 檢查是否為數字(含小數位)
    /// </summary>
    /// <param name="cellData">檢核資料</param>
    /// <param name="cellName">資料名稱,如費用之類</param>
    /// <param name="cellLength">整數長度</param>
    /// <param name="dotLength">小數長度</param>
    /// <returns></returns>
    public static string checkNumberWithPoint(string cellData, string cellName, int cellLength, int dotLength)
    {
        try
        {
            String error = "";
            string errorStr = "";
            double numCheckResult = 0;
            cellData = cellData.Replace(",", "");         //去除數字的,
            double maxValue = Math.Pow(10, cellLength);  //10^長度 

            int pointIndex = cellData.IndexOf(".");       //小數點的位置
            string dotData = "";                          //小數的資料
            int cellDotLength = 0;                        //檢核資料的小數點長度
            if (pointIndex > -1)
            {
                dotData = cellData.Substring(pointIndex+1); //取得該小數點之後所有的資料
                cellDotLength = dotData.Length;
            }


            if (cellData == "")
                error += cellName + "不可空白\n";
            else
            {
                errorStr = cellName + "必須為數字, 且必須為整數" + cellLength + "位，小數" + dotLength + "位, \n";
                
                //檢查是否為數字
                if (!double.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error = errorStr;
                }
                else
                {
                    //檢查整數位的最大值
                    if (Math.Abs(double.Parse(cellData.Trim())) > maxValue)
                    {
                        error = errorStr;
                    }
                    
                    //檢查小數位的長度
                    if (cellDotLength > dotLength)
                    {
                        error = errorStr;
                    }
                }
            }

            return error;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取單號
    public static SqlParameterCollection SP_PROC_GET_DOC_NO(string docId, string userId, string funcId)
    {
        try
        {
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            Hashtable htOut = new Hashtable();

            sb.Append("SP_PROC_GET_DOC_NO");

            ht.Add("@P_DOC_ID", docId);
            ht.Add("@P_USER_ID", userId);
            ht.Add("@P_FUNC_ID", funcId);
            htOut.Add("@P_DOC_NO", "");
            htOut.Add("@P_ERR_MSG", "");

            return dbConn.ExecuteSP(sb, ht, htOut, true);
        }
        catch
        {
            throw;
        }
    }
















}