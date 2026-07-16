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
/// CFB2DL0200DAO 的摘要描述
/// </summary>
public class CFB2DL0200DAO : BaseDAO
{
    public CFB2DL0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string LEAVE_PLAN_YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_LEAVE_TARGET { get; set; }
    public string IFLOW_NO { get; set; }
    public string ORI_DEPT_NO { get; set; }
    public string ORI_DEPT_FULL_NAME { get; set; }
    public string ORI_LEVEL_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string FUNC_ID { get; set; }

    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
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
            if (is_valid != "")
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
    public DataTable getCommCode2(string sys_cd, string main_cd, string is_valid)
    {
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
            if (is_valid != "")
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

    #region Qry
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getDept_name(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NAME ");
            sb.Append(" from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string leave_plan_year, string dept_no
                             , string emp_id, string iflow_no, string leaveHour_notEnough)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "H.EMP_ID");
            if (sortExpression == "DEPT_NO")
                sortExpression = sortExpression.Replace("DEPT_NO", "E.DEPT_NO");
            if (sortExpression.Contains("LEAVE_PLAN_YEAR"))
                sortExpression = sortExpression.Replace("LEAVE_PLAN_YEAR", "H.LEAVE_PLAN_YEAR");
            if (sortExpression.Contains("ORI_DEPT_NO"))
                sortExpression = sortExpression.Replace("ORI_DEPT_NO", "H.ORI_DEPT_NO");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("       H.LEAVE_PLAN_YEAR, H.EMP_ID, E.EMP_NAME, H.ORI_DEPT_NO, H.ORI_DEPT_FULL_NAME ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2)) as EMP_LEAVE_TARGET ");
            sb.AppendLine("	      , H.IFLOW_NO, COMM.COMPANY_PLAN_TARGET, D.SUM_LEAVE_PLAN_HRS as SUM_LEAVE_PLAN_HRS ");
            sb.AppendLine("       , H.LEAVE_PLAN_YEAR + H.EMP_ID as qdatakey ");
            sb.AppendLine("       ,case when SUM_LEAVE_PLAN_HRS >= convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2)) ");
            sb.AppendLine("             then '0' ");
            sb.AppendLine("             else convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2))-SUM_LEAVE_PLAN_HRS end as NOT_ENOUGH_HOUR ");
            sb.AppendLine("   from TB_D_M_EMP_LEAVE_PLAN_H H ");
            sb.AppendLine("   left join VW_H_EMP_DATA E on H.EMP_ID = E.EMP_ID ");
            sb.AppendLine("   left join (select LEAVE_PLAN_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as SUM_LEAVE_PLAN_HRS ");
            sb.AppendLine("			       from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine("			      group by LEAVE_PLAN_YEAR,EMP_ID)D ");
            sb.AppendLine("		     on D.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and D.EMP_ID = H.EMP_ID ");
            sb.AppendLine("	  left join (select CODE_VAL1 as COMPANY_PLAN_TARGET,SUB_CD ");
            sb.AppendLine("	               from TB_9_M_COMM_D ");
            sb.AppendLine("		          where SYS_CD='DL' and MAIN_CD = 'LEAVE_PLAN_TARGET')COMM ");
            sb.AppendLine("	         on COMM.SUB_CD = H.LEAVE_PLAN_YEAR ");
            sb.AppendLine("  where 1=1 ");

            if (leave_plan_year != "")
            {
                sb.AppendLine(" and H.LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
                ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and H.ORI_DEPT_NO like '%'+ @DEPT_NO +'%' ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and H.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (iflow_no != "")
            {
                sb.AppendLine(" and H.IFLOW_NO = @IFLOW_NO ");
                ht.Add("@IFLOW_NO", iflow_no);
            }
            if (leaveHour_notEnough == "Y")
            {
                sb.AppendLine(" and D.SUM_LEAVE_PLAN_HRS <  convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2)) ");
            }

            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string leave_plan_year, string dept_no
                        , string emp_id, string iflow_no, string leaveHour_notEnough)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from TB_D_M_EMP_LEAVE_PLAN_H H ");
            sb.AppendLine("   left join VW_H_EMP_DATA E on H.EMP_ID = E.EMP_ID ");
            sb.AppendLine("   left join (select LEAVE_PLAN_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as SUM_LEAVE_PLAN_HRS ");
            sb.AppendLine("			       from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine("			      group by LEAVE_PLAN_YEAR,EMP_ID)D ");
            sb.AppendLine("		     on D.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and D.EMP_ID = H.EMP_ID ");
            sb.AppendLine("	  left join (select CODE_VAL1 as COMPANY_PLAN_TARGET,SUB_CD ");
            sb.AppendLine("	               from TB_9_M_COMM_D ");
            sb.AppendLine("		          where SYS_CD='DL' and MAIN_CD = 'LEAVE_PLAN_TARGET')COMM ");
            sb.AppendLine("	         on COMM.SUB_CD = H.LEAVE_PLAN_YEAR ");
            sb.AppendLine("  where 1=1 ");

            if (leave_plan_year != "")
            {
                sb.AppendLine(" and H.LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
                ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and H.ORI_DEPT_NO like '%'+ @DEPT_NO +'%' ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and H.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (iflow_no != "")
            {
                sb.AppendLine(" and H.IFLOW_NO = @IFLOW_NO ");
                ht.Add("@IFLOW_NO", iflow_no);
            }
            if (leaveHour_notEnough == "Y")
            {
                sb.AppendLine(" and D.SUM_LEAVE_PLAN_HRS <  convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2)) ");
            }

            DataTable dt = dbConn.Query(sb, ht, true);
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
    //刪除主檔
    public string deleteData(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_EMP_LEAVE_PLAN_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DL020' ");
            sb.Append(" where LEAVE_PLAN_YEAR + EMP_ID = @qdatakey; ");
           
            sb.AppendLine(" Delete from TB_D_M_EMP_LEAVE_PLAN_H ");
            sb.AppendLine(" where LEAVE_PLAN_YEAR + EMP_ID = @qdatakey; ");
            ht.Add("@qdatakey", deleteitem);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    //刪除主檔後 連帶明細檔刪除
    internal string deleteData_D(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_EMP_LEAVE_PLAN_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DL020' ");
            sb.Append(" where LEAVE_PLAN_YEAR + EMP_ID = @qdatakey; ");

            sb.AppendLine(" Delete from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine(" where LEAVE_PLAN_YEAR + EMP_ID = @qdatakey; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@qdatakey", deleteitem);

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select  H.LEAVE_PLAN_YEAR, H.EMP_ID, E.EMP_NAME,H.ORI_DEPT_NO as DEPT_NO, H.ORI_DEPT_FULL_NAME as DEPT_NAME ");
            sb.AppendLine("	      , H.ORI_LEVEL_CD as LEVEL_CD, H.IFLOW_NO, D.SUM_LEAVE_PLAN_HRS, COMM.COMPANY_PLAN_TARGET ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( H.EMP_LEAVE_TARGET/60.0,2)) as EMP_LEAVE_TARGET ");
            sb.AppendLine("   from TB_D_M_EMP_LEAVE_PLAN_H H");
            sb.AppendLine("   left join VW_H_EMP_DATA E on H.EMP_ID = E.EMP_ID ");
            sb.AppendLine("   left join (select LEAVE_PLAN_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as SUM_LEAVE_PLAN_HRS ");
            sb.AppendLine("			       from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine("			      group by LEAVE_PLAN_YEAR,EMP_ID)D ");
            sb.AppendLine("		     on D.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and D.EMP_ID = H.EMP_ID ");
            sb.AppendLine("	  left join (select CODE_VAL1 as COMPANY_PLAN_TARGET,SUB_CD ");
            sb.AppendLine("	               from TB_9_M_COMM_D ");
            sb.AppendLine("		          where SYS_CD='DL' and MAIN_CD = 'LEAVE_PLAN_TARGET')COMM ");
            sb.AppendLine("	         on COMM.SUB_CD = H.LEAVE_PLAN_YEAR ");
            sb.AppendLine("  where H.LEAVE_PLAN_YEAR + H.EMP_ID = @QDATAKEY");
            ht.Add("@QDATAKEY", qdatakey);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string leave_plan_year, string emp_id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "LEAVE_PLAN_DT";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("         LEAVE_PLAN_DT , CONVERT(varchar(100), LEAVE_PLAN_DT, 111) + LEAVE_PLAN_CD as dtldatakey ");
            sb.AppendLine("         ,LEAVE_PLAN_CD--LEAVE_PLAN_CD + '-' + b.SUB_DESC LEAVE_PLAN_CD ");
            sb.AppendLine("        ,convert(Decimal(8,2),Round(LEAVE_PLAN_HRS/60.0,2)) as LEAVE_PLAN_HRS ");
            sb.AppendLine("    from TB_D_M_EMP_LEAVE_PLAN_D a ");
            sb.AppendLine("  left join TB_9_M_COMM_D b on  a.LEAVE_PLAN_CD = b.SUB_CD and b.MAIN_CD = 'LEAVE_PLAN_CD'  and b.IS_VALID='Y'  and b.SYS_CD='DL'  ");
            sb.AppendLine("   where LEAVE_PLAN_YEAR + EMP_ID = @LEAVE_PLAN_YEAR + @EMP_ID ");
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            ht.Add("@EMP_ID", emp_id);

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }

    }
    public int getDtlCount(string leave_plan_year, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine(" from TB_D_M_EMP_LEAVE_PLAN_D");
            sb.AppendLine(" where LEAVE_PLAN_YEAR + EMP_ID = @LEAVE_PLAN_YEAR + @EMP_ID ");
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            ht.Add("@EMP_ID", emp_id);

            DataTable dt = dbConn.Query(sb, ht, true);
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

    #region "Private Functions/Methods"
    //取得員工資訊
    public DataTable getEmpData(string emp_id, string leave_plan_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select E.EMP_NAME,E.DEPT_NO,E.DEPT_FULL_NAME,E.WS_CD,E.LEVEL_CD,E.CALENDAR_CD,A.AVAILABLE_VALUE,P.PJOB_FLOW_LEVEL ");
            sb.AppendLine("   from VW_H_EMP_DATA E ");
            sb.AppendLine("   left join TB_H_M_PJOB P on P.PJOB_CD = E.PJOB_CD ");
            sb.AppendLine("   left join (select convert(Decimal(8,2),Round(SUM(AVAILABLE_VALUE/60.0),2)) as AVAILABLE_VALUE,EMP_ID,BASE_YEAR ");
            sb.AppendLine(" 			   from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine(" 		      where BASE_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine(" 		        and PAY_DT IS NULL ");
            sb.AppendLine(" 		        and ((MAIN_LEAVE_CD = 'D' and SUB_LEAVE_CD ='D0') ");
            sb.AppendLine(" 		         or (MAIN_LEAVE_CD = 'M' and SUB_LEAVE_CD ='M0')) ");
            sb.AppendLine(" 			  group by BASE_YEAR,EMP_ID)A ");
            sb.AppendLine(" 		 on A.EMP_ID = E.EMP_ID ");
            sb.AppendLine("  where E.EMP_ID = @EMP_ID ");
            sb.AppendLine(" group by E.EMP_ID, E.EMP_NAME,E.DEPT_NO,E.DEPT_FULL_NAME,E.WS_CD,E.LEVEL_CD,E.CALENDAR_CD,A.AVAILABLE_VALUE,P.PJOB_FLOW_LEVEL ");
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getME10_pjob_flow_level()
    {
        try
        {
            int pjob_level = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select PJOB_FLOW_LEVEL ");
            sb.AppendLine("   from TB_H_M_PJOB E ");
            //sb.AppendLine("  where PJOB_CD ='ME10' "); //工長
            sb.AppendLine("  where PJOB_CD ='ME20' "); //工長代 2014/12/31 湯姊說 工長代同工長
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
                pjob_level = Convert.ToInt16(dt.Rows[0]["PJOB_FLOW_LEVEL"]);
            return pjob_level;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getTargetDay(string leave_plan_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select CODE_VAL1 as COMPANY_PLAN_TARGET,CODE_VAL2 as CONTINUE_THREE_PLAN_TARGET ");
            sb.AppendLine("   from TB_9_M_COMM_D ");
            sb.AppendLine("  where SYS_CD='DL' ");
            sb.AppendLine("    and MAIN_CD = 'LEAVE_PLAN_TARGET' ");
            sb.AppendLine("    and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", leave_plan_year);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getCalender(string levae_plan_dt, string calendar_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select WORK_DAY_CD ");
            sb.AppendLine("   from TB_D_M_CALENDAR_D ");
            sb.AppendLine("  where CALENDAR_CD= @CALENDAR_CD");
            sb.AppendLine("    and CONVERT(varchar(100), CALENDAR_DT, 111) = @CALENDAR_DT ");
            ht.Add("@CALENDAR_CD", calendar_cd);
            ht.Add("@CALENDAR_DT", levae_plan_dt);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getLeavedHour(string emp_id, string leave_plan_dt, string leave_plan_cd)
    {
        try
        {
            leave_plan_dt = leave_plan_dt + " 00:00:00";
            string leaved_hour = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select isnull(dbo.FN_D_WORK_HRS( @EMP_ID, @LEAVE_PLAN_DT, @LEAVE_PLAN_CD),0) as LEAVED_HOUR ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_DT", leave_plan_dt);
            ht.Add("@LEAVE_PLAN_CD", leave_plan_cd);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool get3DV_LEAVE_PLAN(string emp_id, string leave_plan_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) as total from dbo.FN_D_3DV_LEAVE_PLAN( @EMP_ID, @LEAVE_PLAN_YEAR) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            DataTable dt = dbConn.Query(sb, ht, true);
            if (Convert.ToInt32(dt.Rows[0]["total"]) == 0)
                return false;
            else
                return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "Save"
    //新增主檔前先檢查有無重複資料
    internal DataTable getExistDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * from TB_D_M_EMP_LEAVE_PLAN_H ");
            sb.AppendLine(" where LEAVE_PLAN_YEAR + EMP_ID = @LEAVE_PLAN_YEAR + @EMP_ID ");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增主檔
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_D_M_EMP_LEAVE_PLAN_H ");
            sb.AppendLine(" Values (@LEAVE_PLAN_YEAR,@EMP_ID,@EMP_LEAVE_TARGET,@IFLOW_NO,@ORI_DEPT_NO,@ORI_DEPT_FULL_NAME,@ORI_LEVEL_CD,@CREATED_BY ");
            sb.AppendLine(" ,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            if (EMP_LEAVE_TARGET == "")
                ht.Add("@EMP_LEAVE_TARGET", "0");
            else
                ht.Add("@EMP_LEAVE_TARGET", (Convert.ToDouble(EMP_LEAVE_TARGET) * 60).ToString("0"));
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@ORI_DEPT_NO", ORI_DEPT_NO);
            ht.Add("@ORI_DEPT_FULL_NAME", ORI_DEPT_FULL_NAME);
            ht.Add("@ORI_LEVEL_CD", ORI_LEVEL_CD);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL020");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增明細
    internal void addDtlData(DataRow addDtlRow)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_D_M_EMP_LEAVE_PLAN_D  ");
            sb.AppendLine(" (LEAVE_PLAN_YEAR,EMP_ID,LEAVE_PLAN_DT,LEAVE_PLAN_CD,LEAVE_PLAN_HRS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@LEAVE_PLAN_YEAR,@EMP_ID,@LEAVE_PLAN_DT,@LEAVE_PLAN_CD,@LEAVE_PLAN_HRS,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LEAVE_PLAN_DT", addDtlRow["LEAVE_PLAN_DT"]);
            ht.Add("@LEAVE_PLAN_CD", addDtlRow["LEAVE_PLAN_CD"]);
            if (addDtlRow["LEAVE_PLAN_HRS"] == "")
                ht.Add("@LEAVE_PLAN_HRS", "0");
            else
                ht.Add("@LEAVE_PLAN_HRS", (Convert.ToDouble(addDtlRow["LEAVE_PLAN_HRS"]) * 60).ToString("0"));
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL020");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //修改主檔
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_D_M_EMP_LEAVE_PLAN_H ");
            sb.AppendLine(" set EMP_LEAVE_TARGET = @EMP_LEAVE_TARGET, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine("  where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine("    and EMP_ID = @EMP_ID ");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            if (EMP_LEAVE_TARGET == "")
                ht.Add("@EMP_LEAVE_TARGET", "0");
            else
                ht.Add("@EMP_LEAVE_TARGET", (Convert.ToDouble(EMP_LEAVE_TARGET) * 60).ToString("0"));
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //刪除明細
    internal void deleteDtlData(string deleteItem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_EMP_LEAVE_PLAN_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DL020' ");
            sb.AppendLine("  where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine("    and EMP_ID = @EMP_ID ");
            sb.AppendLine("    and CONVERT(varchar(100), LEAVE_PLAN_DT, 111) = @LEAVE_PLAN_DT; ");
           
            sb.AppendLine(" Delete from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine("  where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine("    and EMP_ID = @EMP_ID ");
            sb.AppendLine("    and CONVERT(varchar(100), LEAVE_PLAN_DT, 111) = @LEAVE_PLAN_DT; ");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LEAVE_PLAN_DT", deleteItem);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    internal bool isDtlExist(string leave_plan_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine(" where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine(" and EMP_ID = @EMP_ID ");
            sb.AppendLine(" and CONVERT(varchar(100), LEAVE_PLAN_DT, 111)= @LEAVE_PLAN_DT ");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LEAVE_PLAN_DT", Convert.ToDateTime(leave_plan_dt).ToString("yyyy/MM/dd"));

            DataTable dt = dbConn.QueryT(sb, ht, true);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //修改明細
    internal void updateDtlData(DataRow modDtlRow)
    {
        try
        {//(LEAVE_PLAN_CD,LEAVE_PLAN_HRS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_D_M_EMP_LEAVE_PLAN_D ");
            sb.AppendLine(" set LEAVE_PLAN_CD = @LEAVE_PLAN_CD, LEAVE_PLAN_HRS = @LEAVE_PLAN_HRS, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine("  where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
            sb.AppendLine("    and EMP_ID = @EMP_ID ");
            sb.AppendLine("    and CONVERT(varchar(100), LEAVE_PLAN_DT, 111) = @LEAVE_PLAN_DT ");
            ht.Add("@LEAVE_PLAN_YEAR", LEAVE_PLAN_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LEAVE_PLAN_DT", modDtlRow["LEAVE_PLAN_DT"]);
            ht.Add("@LEAVE_PLAN_CD", modDtlRow["LEAVE_PLAN_CD"]);

            if (modDtlRow["LEAVE_PLAN_HRS"] == "")
                ht.Add("@LEAVE_PLAN_HRS", "0");
            else
                ht.Add("@LEAVE_PLAN_HRS", (Convert.ToDouble(modDtlRow["LEAVE_PLAN_HRS"]) * 60).ToString("0"));

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #endregion
}