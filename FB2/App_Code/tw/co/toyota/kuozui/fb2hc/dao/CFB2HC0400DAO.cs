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
/// CFB2HC0400DAO 的摘要描述
/// </summary>
public class CFB2HC0400DAO : BaseDAO
{
    public CFB2HC0400DAO()
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

    public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression,
                        string pay_ym)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            --【發放人數】頁籤：
                                        select
	                                          A.COMPANY_CD 
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.START_DT
	                                        , case when A.END_DT is not null then A.END_DT
		                                           else A.PLAN_END_DT
	                                          end END_DT
	                                        , MEMBER_CNT 
	                                        , COUNT(B.EMP_ID) REAL_CNT	
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join 
                                        (
	                                        select START_DT,count(*) MEMBER_CNT
	                                        from TB_H_M_BONUS_PLAN_H
	                                        group by START_DT
                                        ) PH
	                                        on PH.START_DT = A.START_DT
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            sb.AppendLine(@" group by   A.COMPANY_CD 
	                                   ,C.COMPANY_SNAME, A.START_DT
	                                   ,case when A.END_DT is not null then A.END_DT
		                                     else A.PLAN_END_DT
	                                    end
	                                   ,MEMBER_CNT ");
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
                        string pay_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            --【發放人數】頁籤：
                                        select
	                                          A.COMPANY_CD 
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.START_DT
	                                        , case when A.END_DT is not null then A.END_DT
		                                           else A.PLAN_END_DT
	                                          end END_DT
	                                        , MEMBER_CNT 
	                                        , COUNT(B.EMP_ID) REAL_CNT	
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join 
                                        (
	                                        select START_DT,count(*) MEMBER_CNT
	                                        from TB_H_M_BONUS_PLAN_H
	                                        group by START_DT
                                        ) PH
	                                        on PH.START_DT = A.START_DT
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            sb.AppendLine(@" group by   A.COMPANY_CD 
	                                   ,C.COMPANY_SNAME, A.START_DT
	                                   ,case when A.END_DT is not null then A.END_DT
		                                     else A.PLAN_END_DT
	                                    end
	                                   ,MEMBER_CNT ");

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

    public string getSALARY_DT(string pay_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"select
                                  B.SALARY_DT                                 
                            from TB_H_M_BONUS_PLAN_D B
                            where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["SALARY_DT"].ToString();
            else
                return "";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getData1Head(string pay_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"--發放及在籍人數的加總
                            select sum(TOTAL_MEMBER_CNT) as TOTAL_MEMBER_CNT,sum(TOTAL_REAL_CNT) as TOTAL_REAL_CNT
                            from (
                                select
                                      MEMBER_CNT TOTAL_MEMBER_CNT
	                                , COUNT(B.EMP_ID) TOTAL_REAL_CNT	
                                from TB_H_M_BONUS_PLAN_D B
                                inner join TB_H_M_BONUS_PLAN_H A         
	                                on B.EMP_ID = A.EMP_ID
	                                and B.START_DT = A.START_DT
                                left join TB_H_M_EMP E
	                                on B.EMP_ID = E.EMP_ID
                                left join TB_H_M_COMPANY C 
	                                on A.COMPANY_CD = C.COMPANY_CD
                                left join 
                                (
	                                select START_DT,count(*) MEMBER_CNT
	                                from TB_H_M_BONUS_PLAN_H
	                                group by START_DT
                                ) PH
	                                on PH.START_DT = A.START_DT
                                where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }

            sb.AppendLine(" group by A.START_DT, MEMBER_CNT) a ");

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression,
                        string pay_ym)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            select 
	                                          A.COMPANY_CD
	                                        , A.COMPANY_CD + '-' + C1.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , B.BONUS_TYPE
	                                        , B.BONUS_TYPE + '-' +  C2.SUB_DESC as BONUS_TYPE_DESC
	                                        , COUNT(B.EMP_ID) CNT
	                                        , SUM(B.BONUS_AMT) BONUS_AMT	
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_COMPANY C1 
	                                        on A.COMPANY_CD = C1.COMPANY_CD
                                        left join TB_9_M_COMM_D C2 
	                                        on C2.SYS_CD ='HC' 
	                                        and C2.MAIN_CD='BONUS_TYPE' 
	                                        and B.BONUS_TYPE = C2.SUB_CD
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM ");
                ht.Add("@PAY_YM", pay_ym);
            }

            sb.AppendLine(@"group by A.COMPANY_CD
	                                , C1.COMPANY_SNAME 
	                                , B.BONUS_TYPE
	                                , C2.SUB_DESC");

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
                        string pay_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            select 
	                                          A.COMPANY_CD
	                                        , A.COMPANY_CD + '-' + C1.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , B.BONUS_TYPE
	                                        , B.BONUS_TYPE + '-' +  C2.SUB_DESC as BONUS_TYPE_DESC
	                                        , COUNT(B.EMP_ID) CNT
	                                        , SUM(B.BONUS_AMT) BONUS_AMT	
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_COMPANY C1 
	                                        on A.COMPANY_CD = C1.COMPANY_CD
                                        left join TB_9_M_COMM_D C2 
	                                        on C2.SYS_CD ='HC' 
	                                        and C2.MAIN_CD='BONUS_TYPE' 
	                                        and B.BONUS_TYPE = C2.SUB_CD
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM ");
                ht.Add("@PAY_YM", pay_ym);
            }

            sb.AppendLine(@"group by A.COMPANY_CD
	                                , C1.COMPANY_SNAME 
	                                , B.BONUS_TYPE
	                                , C2.SUB_DESC");

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

    public DataTable getData2Head(string pay_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"--聘用單位為'K'者，加總人數及金額為KZ的內容；A.聘用單位為'T'者，加總人數及金額為派遣的內容，兩者相加為合計的內容	
                            select 
	                              A.COMPANY_CD
	                            , A.COMPANY_CD + '-' + C1.COMPANY_SNAME as COMPANY_CD_DESC
	                            , COUNT(B.EMP_ID) CNT
	                            , SUM(B.BONUS_AMT) BONUS_AMT	
                            from TB_H_M_BONUS_PLAN_D B
                            inner join TB_H_M_BONUS_PLAN_H A         
	                            on B.EMP_ID = A.EMP_ID
	                            and B.START_DT = A.START_DT
                            left join TB_H_M_COMPANY C1 
	                            on A.COMPANY_CD = C1.COMPANY_CD
                            where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }

            sb.AppendLine(@"group by A.COMPANY_CD
	                            , C1.COMPANY_SNAME");

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
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

    public DataTable getData2_d1(int startRowIndex, int maximumRows, string sortExpression,
                        string pay_ym, string company_cd, string bonus_type)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "JOIN_DT, EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            --[查詢明細] 獎金類型為 1-先發金
                                        select
	                                          A.COMPANY_CD 
                                            , B.BONUS_TYPE
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.ORI_DEPT_NO + ' ' + A.ORI_DIV_DEPT_FULL_NAME ORI_DEPT_DESC
	                                        , A.EMP_ID
	                                        , E.EMP_NAME
	                                        , E.JOIN_DT 
	                                        , E.LEAVE_DT
	                                        , A.START_DT
	                                        , B.BONUS_AMT
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD  ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (bonus_type != "" && bonus_type != null)
            {
                sb.AppendLine(" and B.BONUS_TYPE = @BONUS_TYPE  ");
                ht.Add("@BONUS_TYPE", bonus_type);
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

    public int getCount2_d1(int startRowIndex, int maximumRows,
                        string pay_ym, string company_cd, string bonus_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            --[查詢明細] 獎金類型為 1-先發金
                                        select
	                                          A.COMPANY_CD 
                                            , B.BONUS_TYPE 
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.ORI_DEPT_NO + ' ' + A.ORI_DIV_DEPT_FULL_NAME ORI_DEPT_DESC
	                                        , A.EMP_ID
	                                        , E.EMP_NAME
	                                        , E.JOIN_DT 
	                                        , E.LEAVE_DT
	                                        , A.START_DT
	                                        , B.BONUS_AMT
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD  ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (bonus_type != "" && bonus_type != null)
            {
                sb.AppendLine(" and B.BONUS_TYPE = @BONUS_TYPE  ");
                ht.Add("@BONUS_TYPE", bonus_type);
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

    public DataTable getData2_d2(int startRowIndex, int maximumRows, string sortExpression,
                        string pay_ym, string company_cd, string bonus_type)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "JOIN_DT, EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            --[查詢明細] 獎金類型為 2-期滿金 或 3-期滿金(非自願離社) 
                                        select
	                                          A.COMPANY_CD
                                            , B.BONUS_TYPE 
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.ORI_DEPT_NO + ' ' + A.ORI_DIV_DEPT_FULL_NAME ORI_DEPT_DESC
	                                        , A.EMP_ID
	                                        , E.EMP_NAME
	                                        , E.JOIN_DT 
	                                        , case when CH.PLAN_END_DT is null then ''
		                                           else Convert(varchar,DATEADD(dd,1,CH.PLAN_END_DT),111)
	                                          end PLAN_END_DT
	                                        , E.LEAVE_DT
	                                        , A.START_DT
	                                        , B.BONUS_AMT
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join TB_H_M_EMP_HR_CHANGE_H CH
	                                        on CH.HR_CHG_NO = A.HR_CHG_NO
	                                        and CH.EMP_ID = A.EMP_ID
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD  ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (bonus_type != "" && bonus_type != null)
            {
                sb.AppendLine(" and B.BONUS_TYPE = @BONUS_TYPE  ");
                ht.Add("@BONUS_TYPE", bonus_type);
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

    public int getCount2_d2(int startRowIndex, int maximumRows,
                        string pay_ym, string company_cd, string bonus_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            --[查詢明細] 獎金類型為 2-期滿金 或 3-期滿金(非自願離社) 
                                        select
	                                          A.COMPANY_CD 
	                                        , A.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY_CD_DESC
	                                        , A.ORI_DEPT_NO + ' ' + A.ORI_DIV_DEPT_FULL_NAME ORI_DEPT_DESC
	                                        , A.EMP_ID
	                                        , E.EMP_NAME
	                                        , E.JOIN_DT 
	                                        , case when CH.PLAN_END_DT is null then ''
		                                           else Convert(varchar,DATEADD(dd,1,CH.PLAN_END_DT),111)
	                                          end PLAN_END_DT
	                                        , E.LEAVE_DT
	                                        , A.START_DT
	                                        , B.BONUS_AMT
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join TB_H_M_EMP_HR_CHANGE_H CH
	                                        on CH.HR_CHG_NO = A.HR_CHG_NO
	                                        and CH.EMP_ID = A.EMP_ID
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD  ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (bonus_type != "" && bonus_type != null)
            {
                sb.AppendLine(" and B.BONUS_TYPE = @BONUS_TYPE  ");
                ht.Add("@BONUS_TYPE", bonus_type);
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

    public DataTable getData2_d2_1(string pay_ym, string company_cd, string bonus_type, string emp_id, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"            --【期滿金明細畫面】[金額明細]  
                                        select P1.REMARK P1_DESC
	                                        , B.BOUNS_WORK_DAYS
	                                        , A.START_DT
	                                        , A.END_DT
	                                        , B.WORK_DAYS
	                                        , P2.REMARK P2_DESC 
	                                        , B.LEAVE_B_DAYS
	                                        , B.LEAVE_A_HRS
	                                        , B.LEAVE_B_HRS
	                                        , P3.REMARK P3_DESC
	                                        , B.LEAVE_Q_DAYS
	                                        , B.LEAVE_Q_HRS
	                                        , P4.REMARK P4_DESC
	                                        , B.JUDGEMENT_DAYS
	                                        , B.THIRD_CNT_REWARD
	                                        , B.SECOND_CNT_REWARD
	                                        , B.FIRST_CNT_REWARD
	                                        , B.THIRD_CNT_PUNISH
	                                        , B.SECOND_CNT_PUNISH
	                                        , B.FIRST_CNT_PUNISH
	                                        , PLAST.REMARK PLAST_DESC
	                                        , B.PLAN_BONUS_AMT
	                                        , B.PLAN_BONUS_DAYS
	                                        , B.BASIC_SALARY
	                                        , B.PAID_AMT
	                                        , B.PAID_CNT
	                                        , B.BONUS_AMT
                                        from TB_H_M_BONUS_PLAN_D B
                                        inner join TB_H_M_BONUS_PLAN_H A         
	                                        on B.EMP_ID = A.EMP_ID
	                                        and B.START_DT = A.START_DT
                                        left join TB_H_M_EMP E
	                                        on B.EMP_ID = E.EMP_ID
                                        left join TB_H_M_COMPANY C 
	                                        on A.COMPANY_CD = C.COMPANY_CD
                                        left join TB_H_M_EMP_HR_CHANGE_H CH
	                                        on CH.HR_CHG_NO = A.HR_CHG_NO
	                                        and CH.EMP_ID = A.EMP_ID
                                        left join TB_9_M_PARAMETER P1
	                                        on (A.WS_CD = 'S' and P1.SYS_CD = 'HC' and P1.MAIN_CD = 'S_BONUS_1_DESC')
	                                        or (A.WS_CD = 'W' and P1.SYS_CD = 'HC' and P1.MAIN_CD = 'W_BONUS_1_DESC')
                                        left join TB_9_M_PARAMETER P2
	                                        on (A.WS_CD = 'S' and P2.SYS_CD = 'HC' and P2.MAIN_CD = 'S_BONUS_2_DESC')
	                                        or (A.WS_CD = 'W' and P2.SYS_CD = 'HC' and P2.MAIN_CD = 'W_BONUS_2_DESC')
                                        left join TB_9_M_PARAMETER P3
	                                        on (A.WS_CD = 'S' and P3.SYS_CD = 'HC' and P3.MAIN_CD = 'S_BONUS_3_DESC')
	                                        or (A.WS_CD = 'W' and P3.SYS_CD = 'HC' and P3.MAIN_CD = 'W_BONUS_3_DESC')
                                        left join TB_9_M_PARAMETER P4
	                                        on (A.WS_CD = 'S' and P4.SYS_CD = 'HC' and P4.MAIN_CD = 'S_BONUS_4_DESC')
	                                        or (A.WS_CD = 'W' and P4.SYS_CD = 'HC' and P4.MAIN_CD = 'W_BONUS_4_DESC')
                                        left join TB_9_M_PARAMETER PLAST
	                                        on (A.WS_CD = 'S' and PLAST.SYS_CD = 'HC' and PLAST.MAIN_CD = 'S_BONUS_LAST_DESC')
	                                        or (A.WS_CD = 'W' and PLAST.SYS_CD = 'HC' and PLAST.MAIN_CD = 'W_BONUS_LAST_DESC')
                                        where 1 = 1 ");

            if (pay_ym != "" && pay_ym != null)
            {
                sb.AppendLine(" and B.PAY_YM = @PAY_YM  ");
                ht.Add("@PAY_YM", pay_ym);
            }
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and A.COMPANY_CD = @COMPANY_CD  ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (bonus_type != "" && bonus_type != null)
            {
                sb.AppendLine(" and B.BONUS_TYPE = @BONUS_TYPE  ");
                ht.Add("@BONUS_TYPE", bonus_type);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and A.EMP_ID = @EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_dt != "" && start_dt != null)
            {
                sb.AppendLine(" and A.START_DT = @START_DT  ");
                ht.Add("@START_DT", start_dt.Split(' ')[0]);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string WFB2HC0400StlAmt_proc(string pay_ym, string emp_id)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select SALARY_STATUS 
                    FROM TB_H_M_BONUS_H 
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@PAY_YM", pay_ym);

            dt = dbConn.QueryT(sb, ht, false);
            if (dt.Rows.Count > 0)
                if (dt.Rows[0]["SALARY_STATUS"].ToString() == "Y")
                    throw new Exception(Resources.Resource.wfb2hc_WFB2HC0400StlAmt_check_Message);

            sb = new StringBuilder();
            ht = new Hashtable();
            if (dt.Rows.Count > 0)
            {
                sb.Append(@"update TB_H_M_BONUS_H set 
                          STL_STATUS = 'P'
                        , UPDATED_BY = @UPDATED_BY
                        , UPDATED_DT = getdate()
                        , FUNC_ID = 'FB2HC040'
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
                ht.Add("@UPDATED_BY", emp_id);
                ht.Add("@PAY_YM", pay_ym);
            }
            else
            {
                sb.Append(@"declare @PAY_DT datetime
                        declare @START_DT datetime
                        declare @END_DATE datetime
                        set @PAY_DT=@PAY_YM+'01'
                        set @START_DT = (select dateadd(month,datediff(month,0,@PAY_DT),0))
                        set @END_DATE = (select dateadd(month,1+datediff(month,0,@PAY_DT),0)-1)
                        select @START_DT, @END_DATE
                        insert into TB_H_M_BONUS_H (PAY_YM
                            , START_DT
                            , END_DATE
                            , STL_STATUS
                            , SALARY_STATUS
                            , SALARY_DT
                            , CREATED_BY
                            , CREATED_DT
                            , UPDATED_BY
                            , UPDATED_DT
                            , FUNC_ID
                        ) values (@PAY_YM
                            , @START_DT
                            , @END_DATE
                            , 'P'
                            , 'N'
                            , null
                            , @EMP_ID
                            , getdate()
                            , @EMP_ID
                            , getdate()
                            , 'FB2HC040'
                        )");
                ht.Add("@EMP_ID", emp_id);
                ht.Add("@PAY_YM", pay_ym);
            }
            dbConn.ExecuteT(sb, ht, false);

            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append("SP_H_CONTRACT_BONUS_STL");
            ht.Add("@pYM", pay_ym);
            ht.Add("@pUserID", emp_id);
            ht.Add("@pFuncID", "FB2HC040");

            dbConn.ExecuteSPT(sb, ht, false);
            return Resources.Resource.wfb2hc_WFB2HC0400StlAmt_proc_ok;
        }
        catch
        {
            throw;
        }

    }

    public string WFB2HC0400StlLock_proc_step1(string pay_ym, string emp_id)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select SALARY_STATUS 
                    FROM TB_H_M_BONUS_H 
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@PAY_YM", pay_ym);

            dt = dbConn.Query(sb, ht, false);
            if (dt.Rows.Count > 0)
                if (dt.Rows[0]["SALARY_STATUS"].ToString() == "Y")
                    throw new Exception(Resources.Resource.wfb2hc_WFB2HC0400StlLock_check_Message);

            return Resources.Resource.wfb2hc_WFB2HC0400StlLock_confirm_Message;
        }
        catch
        {
            throw;
        }
    }

    public string WFB2HC0400StlLock_proc_step2(string pay_ym, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(@"update TB_H_M_BONUS_H set 
                          STL_STATUS = 'Y'
                        , UPDATED_BY = @UPDATED_BY
                        , UPDATED_DT = getdate()
                        , FUNC_ID = 'FB2HC040'
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@PAY_YM", pay_ym);
            dbConn.ExecuteT(sb, ht, false);

            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(@"update TB_H_M_BONUS_PLAN_D set 
                          STL_STATUS = 'Y'
                        , UPDATED_BY = @UPDATED_BY
                        , UPDATED_DT = getdate()
                        , FUNC_ID = 'FB2HC040'
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@PAY_YM", pay_ym);
            dbConn.ExecuteT(sb, ht, false);

            return Resources.Resource.wfb2hc_WFB2HC0400StlLock_proc_ok;
        }
        catch
        {
            throw;
        }
    }

    public string WFB2HC0400StlUnLock_proc(string pay_ym, string emp_id)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select SALARY_STATUS,STL_STATUS 
                    FROM TB_H_M_BONUS_H 
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@PAY_YM", pay_ym);

            dt = dbConn.QueryT(sb, ht, false);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SALARY_STATUS"].ToString() == "Y")
                    throw new Exception(Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_check_Message1);
                if (dt.Rows[0]["STL_STATUS"].ToString() == "N" || dt.Rows[0]["STL_STATUS"].ToString() == "P")
                    throw new Exception(Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_check_Message2);
            }

            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(@"update TB_H_M_BONUS_H set 
                          STL_STATUS = 'P'
                        , UPDATED_BY = @UPDATED_BY
                        , UPDATED_DT = getdate()
                        , FUNC_ID = 'FB2HC040'
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@PAY_YM", pay_ym);
            dbConn.ExecuteT(sb, ht, false);

            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(@"update TB_H_M_BONUS_PLAN_D set 
                          STL_STATUS = 'P'
                        , UPDATED_BY = @UPDATED_BY
                        , UPDATED_DT = getdate()
                        , FUNC_ID = 'FB2HC040'
                    where 1 = 1
                    and PAY_YM = @PAY_YM");
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@PAY_YM", pay_ym);
            dbConn.ExecuteT(sb, ht, false);

            return Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_proc_ok;
        }
        catch
        {
            throw;
        }

    }

}