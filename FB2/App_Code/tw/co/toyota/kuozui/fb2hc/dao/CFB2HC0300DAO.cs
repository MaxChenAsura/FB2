using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;


/// <summary>
/// CFB2HC0300DAO 的摘要描述
/// </summary>
public class CFB2HC0300DAO : BaseDAO
{
    public CFB2HC0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "" || is_valid != null)
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public string getDept_name(string dept_no)
    {
        try
        {
            string rtnValue = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NAME ");
            sb.Append(" from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                rtnValue = dt.Rows[0]["DEPT_NAME"].ToString();
            return rtnValue;
        }
        catch
        {
            throw;
        }
    }

    public string getEmp_name(string emp_id)
    {
        try
        {
            string rtnValue = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_NAME ");
            sb.Append(" from TB_H_M_EMP ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                rtnValue = dt.Rows[0]["EMP_NAME"].ToString();
            return rtnValue;
        }
        catch
        {
            throw;
        }
    }

    #region Qry

    public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression,
                        string start_sym, string start_eym, string ori_dept_no, string emp_id, string company_cd, string ws_cd)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "START_DT, ORI_DEPT_NO, EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            select	 A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
                                                ,A.ORI_DEPT_NO
		                                        ,A.ORI_DEPT_NO + ' ' + A.ORI_DEPT_FULL_NAME as ORI_DEPT_INFO
                                                ,A.WS_CD
		                                        ,A.WS_CD + '-' + WS.SUB_DESC as WS_DESC
		                                        ,A.EMP_ID
		                                        ,rtrim(E.EMP_NAME) AS EMP_NAME
		                                        ,A.START_DT
		                                        ,A.PLAN_END_DT
		                                        ,A.END_DT
		                                        ,A.HR_CHG_CD + '-' + G.HR_CHG_DESC as HR_CHG_DESC
                                                ,E.BIRTH_DT
                                        from TB_H_M_BONUS_PLAN_H A
                                        left join TB_H_M_EMP E
	                                        on A.EMP_ID = E.EMP_ID
                                        left join TB_H_M_HR_CHANGE_CODE G
	                                        on A.HR_CHG_CD = G.HR_CHG_CD
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join TB_9_M_COMM_D WS
                                            on WS.SYS_CD = 'HB'
                                            and WS.MAIN_CD = 'WS_CD'
                                            and WS.SUB_CD = A.WS_CD
                                        Where 1 = 1 ");

            if (start_sym != "" && start_sym != null)
            {
                sb.AppendLine(" and Convert(varchar(6),A.START_DT,112) >= @START_SYM  ");
                ht.Add("@START_SYM", start_sym);
            }
            if (start_eym != "" && start_eym != null)
            {
                sb.AppendLine(" and Convert(varchar(6),A.START_DT,112) <= @START_EYM  ");
                ht.Add("@START_EYM", start_eym);
            }
            if (ori_dept_no != "" && ori_dept_no != null)
            {
                sb.AppendLine(" and A.ORI_DEPT_NO like @ORI_DEPT_NO ");
                ht.Add("@ORI_DEPT_NO", ori_dept_no + "%");
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and A.EMP_ID LIKE @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (ws_cd != "" && ws_cd != null)
            {
                sb.AppendLine(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            sb.AppendLine("         )alltb ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public int getCount1(int startRowIndex, int maximumRows,
                        string start_sym, string start_eym, string ori_dept_no, string emp_id, string company_cd, string ws_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            select	 A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
                                                ,A.ORI_DEPT_NO
		                                        ,A.ORI_DEPT_NO + '-' + A.ORI_DEPT_FULL_NAME as ORI_DEPT_INFO
                                                ,A.WS_CD
		                                        ,A.WS_CD + '-' + WS.SUB_DESC as WS_DESC
		                                        ,A.EMP_ID
		                                        ,rtrim(E.EMP_NAME) AS EMP_NAME
		                                        ,A.START_DT
		                                        ,A.PLAN_END_DT
		                                        ,A.END_DT
		                                        ,A.HR_CHG_CD + '-' + G.HR_CHG_DESC as HR_CHG_DESC
                                        from TB_H_M_BONUS_PLAN_H A
                                        left join TB_H_M_EMP E
	                                        on A.EMP_ID = E.EMP_ID
                                        left join TB_H_M_HR_CHANGE_CODE G
	                                        on A.HR_CHG_CD = G.HR_CHG_CD
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join TB_9_M_COMM_D WS
                                            on WS.SYS_CD = 'HB'
                                            and WS.MAIN_CD = 'WS_CD'
                                            and WS.SUB_CD = A.WS_CD
                                        Where 1 = 1 ");

            if (start_sym != "" && start_sym != null)
            {
                sb.AppendLine(" and Convert(varchar(6),A.START_DT,112) >= @START_SYM  ");
                ht.Add("@START_SYM", start_sym);
            }
            if (start_eym != "" && start_eym != null)
            {
                sb.AppendLine(" and Convert(varchar(6),A.START_DT,112) <= @START_EYM  ");
                ht.Add("@START_EYM", start_eym);
            }
            if (ori_dept_no != "" && ori_dept_no != null)
            {
                sb.AppendLine(" and A.ORI_DEPT_NO like @ORI_DEPT_NO ");
                ht.Add("@ORI_DEPT_NO", ori_dept_no + "%");
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and A.EMP_ID LIKE @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (ws_cd != "" && ws_cd != null)
            {
                sb.AppendLine(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            
            sb.AppendLine("  )alltb ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }

    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression,
                        string emp_id, string start_dt)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "PAY_YM";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            select	 B.EMP_ID, B.START_DT, B.BONUS_TYPE
                                                ,B.BONUS_TYPE + '-' + C.SUB_DESC AS BONUS_TYPE_DESC
		                                        ,B.PAY_YM
		                                        ,B.BONUS_AMT
                                                ,B.SALARY_STATUS as SALARY_STATUS_CD
		                                        ,CASE WHEN B.SALARY_STATUS = 'Y' THEN 'Y-轉薪資' 
		                                              WHEN B.SALARY_STATUS = 'N' THEN 'N-薪資未處理'
		                                              ELSE ''
		                                         END AS SALARY_STATUS    
		                                        ,B.SALARY_DT
                                        from TB_H_M_BONUS_PLAN_D B
                                        left join TB_9_M_COMM_D C 
	                                        on C.SYS_CD ='HC' 
	                                        and C.MAIN_CD='BONUS_TYPE' and B.BONUS_TYPE = C.SUB_CD
                                        where 1 = 1 ");

            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and B.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_dt != "" && start_dt != null)
            {
                sb.AppendLine(" and B.START_DT = @START_DT ");
                ht.Add("@START_DT", start_dt);
            }

            sb.AppendLine("         )alltb ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public int getCount2(int startRowIndex, int maximumRows,
                        string emp_id, string start_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            select	 C.SUB_DESC AS BONUS_TYPE_DESC
		                                        ,B.PAY_YM
		                                        ,B.BONUS_AMT
		                                        ,CASE WHEN B.SALARY_STATUS = 'Y' THEN '轉薪資' 
		                                              WHEN B.SALARY_STATUS = 'N' THEN '薪資未處理'
		                                              ELSE ''
		                                         END AS SALARY_STATUS    
		                                        ,B.SALARY_DT
                                        from TB_H_M_BONUS_PLAN_D B
                                        left join TB_9_M_COMM_D C 
	                                        on C.SYS_CD ='HC' 
	                                        and C.MAIN_CD='BONUS_TYPE' and B.BONUS_TYPE = C.SUB_CD
                                        where 1 = 1 ");

            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and B.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_dt != "" && start_dt != null)
            {
                sb.AppendLine(" and B.START_DT = @START_DT ");
                ht.Add("@START_DT", start_dt);
            }

            sb.AppendLine("  )alltb ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }

    public string getData2_Total_Bonus_Amt(string emp_id, string start_dt)
    {
        try
        {
            string rtnvalue = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"    select	 SUM(B.BONUS_AMT) AS TOTAL_BONUS_AMT
                                from TB_H_M_BONUS_PLAN_D B
                                where 1 = 1 ");

            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and B.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_dt != "" && start_dt != null)
            {
                sb.AppendLine(" and B.START_DT = @START_DT ");
                ht.Add("@START_DT", start_dt);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                rtnvalue = dt.Rows[0]["TOTAL_BONUS_AMT"].ToString();
            return rtnvalue;
        }
        catch
        {
            throw;
        }
    }

    //刪除 年獎明細維護檔
    public void deleteData(string EMP_ID, string START_DT, string BONUS_TYPE, string PAY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  TB_H_M_BONUS_PLAN_D " );
            sb.Append(" where EMP_ID = @EMP_ID  ");
            sb.Append("  and START_DT = @START_DT ");
            sb.Append("  and BONUS_TYPE = @BONUS_TYPE ");
            sb.Append("  and PAY_YM = @PAY_YM   AND SALARY_DT IS null ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@BONUS_TYPE", BONUS_TYPE);
            ht.Add("@PAY_YM", PAY_YM);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
}