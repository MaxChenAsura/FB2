using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DL0300DAO 的摘要描述
/// </summary>
public class CFB2DL0300DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string LICENSE_ID_FIRST { get; set; }

    public CFB2DL0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public bool isManager(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from TB_H_R_HEAD_DEPT");
            sb.AppendLine(" where EMP_ID = @EMP_ID  ");
            ht.Add("@EMP_ID", emp_id);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }
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
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //public string getDepartment()
    //{
    //    ACESLib.ACES aces = new ACESLib.ACES();
    //    string departments = "";
    //    //foreach (string DB_ROLE_CD in aces.GetRoles().Split(',')) //取得「資料角色代碼」
    //    //{
    //        string DB_ROLE_CD = "FB2DBADMIN";
    //        departments = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD)).Departments; //取得「使用其它部門權限」
    //    //}
    //    return departments;
    //}
    public string getCompany_target(string leave_plan_year)
    {
        try
        {
            string company_plan_target = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select CODE_VAL1 as COMPANY_PLAN_TARGET,SUB_CD ");
            sb.AppendLine("	  from TB_9_M_COMM_D ");
            sb.AppendLine("	 where SYS_CD='DL' and MAIN_CD = 'LEAVE_PLAN_TARGET' and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", leave_plan_year);

            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
                company_plan_target = Convert.ToString(dt.Rows[0]["COMPANY_PLAN_TARGET"]);
            return company_plan_target;
        }
        catch
        {
            throw;
        }
    }

    #region "grid1"
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string leave_plan_year, string emp_id, string dept_no
                                    , string is_super, string is_dept, string departments)
    {
        try
        {

            if (sortExpression == null || sortExpression == "")
                sortExpression = "TARGET_MINUS DESC,ORI_DEPT_NO";

            DateTime leave_plan_yearStartDate = new DateTime(Convert.ToInt16(leave_plan_year), 1, 1);
            DateTime leave_plan_yearEndDate ;
            if (Convert.ToInt32(leave_plan_year) >= DateTime.Now.Year)
            {
                leave_plan_yearEndDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1);
                leave_plan_yearEndDate = leave_plan_yearEndDate.AddDays(-1);
            }
            else
                leave_plan_yearEndDate = new DateTime(Convert.ToInt16(leave_plan_year), 12, 31);

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,*  from ( ");
            sb.AppendLine("       select H.EMP_ID, H.ORI_DEPT_NO, H.ORI_DEPT_FULL_NAME, H.ORI_LEVEL_CD, H.LEAVE_PLAN_YEAR                                     ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round(H.EMP_LEAVE_TARGET/60.0,2)) as EMP_LEAVE_TARGET                                           ");
            sb.AppendLine(" 	   ,E.EMP_NAME, A.AVAILABLE_VALUE,L.TOTAL_TIME_APPROVE as TOTAL_TIME_APPROVE                                            ");
            sb.AppendLine(" 	   ,H.ORI_DEPT_NO +'-'+H.ORI_DEPT_FULL_NAME as DEPT                                                                     ");
            sb.AppendLine("        ,case when isnull(ML.MONTH_LEAVED_PLAN,0) - isnull(MA.MONTH_TIME_APPROVE,0) <=0 then '0' else (ML.MONTH_LEAVED_PLAN - MA.MONTH_TIME_APPROVE)  end as TARGET_MINUS ");
            sb.AppendLine("   from TB_D_M_EMP_LEAVE_PLAN_H H                                                                                            ");
            sb.AppendLine("   left join TB_H_M_EMP E on E.EMP_ID = H.EMP_ID                                                                             ");
            //計畫時數
            sb.AppendLine("   left join (select LEAVE_PLAN_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as LEAVED_PLAN_HRS       ");
            sb.AppendLine(" 			   from TB_D_M_EMP_LEAVE_PLAN_D                                                                                 ");
            sb.AppendLine(" 			  where  LEAVE_PLAN_DT >= @LEAVE_PLAN_YEARSTARTDATE and LEAVE_PLAN_DT <= @LEAVE_PLAN_YEARENDDATE                ");
            sb.AppendLine(" 			 group by LEAVE_PLAN_YEAR,EMP_ID                                                                                ");
            sb.AppendLine(" 			 ) D                                                                           ");
            sb.AppendLine(" 	     on D.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and D.EMP_ID = H.EMP_ID                                                   ");
            //可用時數
            sb.AppendLine("   left join (select BASE_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(AVAILABLE_VALUE/60.0),2)) as AVAILABLE_VALUE            ");
            sb.AppendLine(" 		       from TB_D_M_EMP_AVAILABLE_LEAVE 			                                                                    ");
            sb.AppendLine(" 	          where 1=1                                                                                              ");
            sb.AppendLine(" 	             and SUB_LEAVE_CD in('D0','D2','M0')                                                                   ");
            sb.AppendLine(" 			 group by BASE_YEAR,EMP_ID                                                                                ");
            sb.AppendLine(" 		     ) A                                                                                  ");
            sb.AppendLine(" 		 on A.BASE_YEAR = H.LEAVE_PLAN_YEAR and A.EMP_ID = H.EMP_ID                                                         ");
            //已休時數
            sb.AppendLine("   left join (select EMP_ID,convert(Decimal(8,2),ROUND((SUM(TOTAL_TIME_APPROVE)/60.0), 2)) as TOTAL_TIME_APPROVE             ");
            sb.AppendLine(" 			   from TB_D_M_LEAVE_APPLY_DAY                                                                                  ");
            sb.AppendLine(" 			  where 1=1                                                        ");
            sb.AppendLine(" 	            and SUB_LEAVE_CD in('D0','D2','M0')                                                        ");
            sb.AppendLine("                 and APPLY_LEAVE_SDT >= @LEAVE_PLAN_YEARSTARTDATE and APPLY_LEAVE_EDT <= @LEAVE_PLAN_YEARENDDATE             ");
            sb.AppendLine("                 and FORM_STATUS NOT IN ('N','D')                                                                    ");
            sb.AppendLine("                  and CHECK_STATUS ='Y'                                                                               ");
            sb.AppendLine(" 			 group by EMP_ID                                                                                ");
            sb.AppendLine(" 	       ) L                                                                                                              ");
            sb.AppendLine(" 	     on L.EMP_ID = H.EMP_ID                                                                                             ");
            //by月份 計畫時數
            sb.AppendLine("   left join (select LEAVE_PLAN_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as MONTH_LEAVED_PLAN     ");
            sb.AppendLine(" 			   from TB_D_M_EMP_LEAVE_PLAN_D                                                                                 ");
            sb.AppendLine(" 			  where  LEAVE_PLAN_DT >= @LEAVE_PLAN_YEARSTARTDATE and LEAVE_PLAN_DT <= @LEAVE_PLAN_YEARENDDATE                ");
            sb.AppendLine(" 			  group by LEAVE_PLAN_YEAR,EMP_ID                                                                               ");
            sb.AppendLine(" 			) ML                                                                                                            ");
            sb.AppendLine(" 	     on ML.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and ML.EMP_ID = H.EMP_ID                                                 ");
            //by月份 已休時數
            sb.AppendLine("   left join (select EMP_ID,convert(Decimal(8,2),ROUND((SUM(TOTAL_TIME_APPROVE)/60.0), 2)) as MONTH_TIME_APPROVE             ");
            sb.AppendLine(" 			   from TB_D_M_LEAVE_APPLY_DAY                                                                                  ");
            sb.AppendLine(" 			  where 1=1                                                                                                      ");
            sb.AppendLine(" 	            and SUB_LEAVE_CD in('D0','D2','M0')                                                        ");
            sb.AppendLine("                 and APPLY_LEAVE_SDT >= @LEAVE_PLAN_YEARSTARTDATE and APPLY_LEAVE_SDT <= @LEAVE_PLAN_YEARENDDATE             ");
            sb.AppendLine("                and FORM_STATUS NOT IN ('N','D')                                                                                      ");
            sb.AppendLine("                and CHECK_STATUS ='Y'                                                                                                 ");
            sb.AppendLine(" 			  group by EMP_ID                                                                                               ");
            sb.AppendLine(" 	        ) MA                                                                                                            ");
            sb.AppendLine(" 	     on MA.EMP_ID = H.EMP_ID                                                                                            ");
            sb.AppendLine("  where 1=1 ");
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }


            ht.Add("@LEAVE_PLAN_YEARSTARTDATE", leave_plan_yearStartDate.ToString("yyyy/MM/dd"));
            ht.Add("@LEAVE_PLAN_YEARENDDATE", leave_plan_yearEndDate.ToString("yyyy/MM/dd"));
            if (leave_plan_year != "")
            {
                sb.AppendLine(" and H.LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
                ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and H.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (dept_no != "")
            {
                sb.AppendLine(" and H.ORI_DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            

            sb.AppendLine(" )M )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string leave_plan_year, string emp_id, string dept_no
                             , string is_super, string is_dept, string departments)
    {
        try
        {
            DateTime leave_plan_yearStartDate = new DateTime(Convert.ToInt16(leave_plan_year), 1, 1);
            DateTime leave_plan_yearEndDate;

            if (Convert.ToInt32(leave_plan_year) >= DateTime.Now.Year)
            {
                leave_plan_yearEndDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1);
                leave_plan_yearEndDate = leave_plan_yearEndDate.AddDays(-1);
            }
            else
                leave_plan_yearEndDate = new DateTime(Convert.ToInt16(leave_plan_year), 12, 31);

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_D_M_EMP_LEAVE_PLAN_H H                                                                                           ");
            sb.AppendLine("   left join TB_H_M_EMP E on E.EMP_ID = H.EMP_ID                                                                             ");
            sb.AppendLine("   left join (select EMP_ID,LEAVE_PLAN_YEAR,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as LEAVED_PLAN_HRS       ");
            sb.AppendLine(" 			   from TB_D_M_EMP_LEAVE_PLAN_D                                                                                 ");
            sb.AppendLine(" 			  group by EMP_ID,LEAVE_PLAN_YEAR ) D                                                                           ");
            sb.AppendLine(" 	     on D.LEAVE_PLAN_YEAR = H.LEAVE_PLAN_YEAR and D.EMP_ID = H.EMP_ID                                                   ");
            sb.AppendLine("   left join (select EMP_ID,BASE_YEAR,convert(Decimal(8,2),Round(SUM(AVAILABLE_VALUE/60.0),2)) as AVAILABLE_VALUE            ");
            sb.AppendLine(" 		       from TB_D_M_EMP_AVAILABLE_LEAVE 			                                                                    ");
            sb.AppendLine(" 	          where 1=1                                                          ");
            sb.AppendLine(" 	             and SUB_LEAVE_CD in('D0','D2','M0')                                                             ");
            sb.AppendLine(" 		      group by EMP_ID,BASE_YEAR) A                                                                                  ");
            sb.AppendLine(" 		 on A.BASE_YEAR = H.LEAVE_PLAN_YEAR and A.EMP_ID = H.EMP_ID                                                         ");
            sb.AppendLine("   left join (select EMP_ID,convert(Decimal(8,2),ROUND((SUM(TOTAL_TIME_APPROVE)/60.0), 2)) as TOTAL_TIME_APPROVE             ");
            sb.AppendLine(" 			   from TB_D_M_LEAVE_APPLY_DAY                                                                                  ");
            sb.AppendLine(" 			  where 1=1                                                         ");
            sb.AppendLine(" 	              and SUB_LEAVE_CD in('D0','D2','M0')                                                                 ");
            sb.AppendLine("                 and FORM_STATUS NOT IN ('N','D')                                                                    ");
            sb.AppendLine("                 and CHECK_STATUS ='Y'                                                                               ");
            sb.AppendLine("                 and APPLY_LEAVE_SDT >= @LEAVE_PLAN_YEARSTARTDATE and APPLY_LEAVE_SDT <= @LEAVE_PLAN_YEARENDDATE                           ");
            sb.AppendLine(" 	          group by EMP_ID) L                                                                                            ");
            sb.AppendLine(" 	     on L.EMP_ID = H.EMP_ID                                                                                             ");
            sb.AppendLine("  where 1=1 ");
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            ht.Add("@LEAVE_PLAN_YEARSTARTDATE", leave_plan_yearStartDate.ToString("yyyy/MM/dd"));
            ht.Add("@LEAVE_PLAN_YEARENDDATE", leave_plan_yearEndDate.ToString("yyyy/MM/dd"));
            if (leave_plan_year != "")
            {
                sb.AppendLine(" and H.LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR ");
                ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and H.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and H.ORI_DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
           
            DataTable dt = dbConn.Query(sb, ht);
            int t = 0;
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
    
    #endregion


    #region "grid2"
    //排休計劃
    public DataTable getGrid2Row1(string leave_plan_year, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID                                                                           ");
            sb.AppendLine("       ,isnull([01],0) as JAN,isnull([02],0) as FEB,isnull([03],0) as MAR                ");
            sb.AppendLine("       ,isnull([04],0) as APR,isnull([05],0) as MAY,isnull([06],0) as JUN                ");
            sb.AppendLine("       ,isnull([07],0) as JUL,isnull([08],0) as AUG,isnull([09],0) as SEP                ");
            sb.AppendLine("       ,isnull([10],0) as OCT,isnull([11],0) as NOV,isnull([12],0) as DECE               ");
            sb.AppendLine(" from (                                                                                  ");
            sb.AppendLine(" 	select LEAVE_PLAN_YEAR,EMP_ID,                                                      ");
            sb.AppendLine(" 			substring(Convert(varchar,LEAVE_PLAN_DT,112),5,2) as MONTH_SORT             ");
            sb.AppendLine("           ,convert(Decimal(8,2),Round(SUM(LEAVE_PLAN_HRS/60.0),2)) as LEAVE_PLAN_HRS    ");
            sb.AppendLine(" 	from  TB_D_M_EMP_LEAVE_PLAN_D                                                       ");
            sb.AppendLine(" 	where LEAVE_PLAN_YEAR = @LEAVE_PLAN_YEAR                                            ");
            sb.AppendLine(" 	  and EMP_ID = @EMP_ID                                    	                        ");
            sb.AppendLine(" 	group by LEAVE_PLAN_YEAR,EMP_ID, substring(Convert(varchar,LEAVE_PLAN_DT,112),5,2)  ");
            sb.AppendLine(" ) as A1                                                                                 ");
            sb.AppendLine(" pivot                                                                                   ");
            sb.AppendLine(" (                                                                                       ");
            sb.AppendLine(" 	sum(LEAVE_PLAN_HRS)                                                                 ");
            sb.AppendLine(" 	for                                                                                 ");
            sb.AppendLine(" 	MONTH_SORT in ([01],[02],[03],[04],[05],[06],[07],[08],[09],[10],[11],[12])         ");
            sb.AppendLine(" ) As B1                                                                                 ");

            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //實際已休
    public DataTable getGrid2Row2(string leave_plan_year, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select EMP_ID                                                                                             ");
            sb.AppendLine(" ,isnull([01],0) as JAN,isnull([02],0) as FEB,isnull([03],0) as MAR                                        ");
            sb.AppendLine(" ,isnull([04],0) as APR,isnull([05],0) as MAY,isnull([06],0) as JUN                                        ");
            sb.AppendLine(" ,isnull([07],0) as JUL,isnull([08],0) as AUG,isnull([09],0) as SEP                                        ");
            sb.AppendLine(" ,isnull([10],0) as OCT,isnull([11],0) as NOV,isnull([12],0) as DECE                                       ");
            sb.AppendLine(" From (                                                                                                    ");
            sb.AppendLine(" 	select EMP_ID,                                                                                        ");
            sb.AppendLine(" 			substring(Convert(varchar,APPLY_LEAVE_SDT,112),5,2) as MONTH_SORT                            ");
            sb.AppendLine("           ,convert(Decimal(8,2),Round(SUM(TOTAL_TIME_APPROVE/60.0),2)) as TOTAL_TIME_APPROVE            ");
            sb.AppendLine(" 	from  TB_D_M_LEAVE_APPLY_DAY a                                                                        ");
            sb.AppendLine(" 	where EMP_ID = @EMP_ID                                                                                ");
            sb.AppendLine(" 	  and substring(Convert(varchar,APPLY_LEAVE_SDT,112),1,4) = @LEAVE_PLAN_YEAR                          ");
            sb.AppendLine("      and SUB_LEAVE_CD in('D0','D2','M0')                                                                 ");
            sb.AppendLine("       and FORM_STATUS NOT IN ('N','D')                                                                    ");
            sb.AppendLine("       and CHECK_STATUS ='Y'                                                                               ");
            sb.AppendLine(" 	group by EMP_ID, substring(Convert(varchar,APPLY_LEAVE_SDT,112),5,2)                                  ");
            sb.AppendLine(" ) as A1                                                                                                   ");
            sb.AppendLine(" pivot                                                                                                     ");
            sb.AppendLine(" (                                                                                                         ");
            sb.AppendLine(" 	SUM(TOTAL_TIME_APPROVE)                                                                               ");
            sb.AppendLine(" 	for                                                                                                   ");
            sb.AppendLine(" 	MONTH_SORT in ([01],[02],[03],[04],[05],[06],[07],[08],[09],[10],[11],[12])                           ");
            sb.AppendLine(" ) As B1                                                                                                   ");

            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable get3DV_LEAVE_PLAN(string emp_id, string leave_plan_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from dbo.FN_D_3DV_LEAVE_PLAN(@EMP_ID,@LEAVE_PLAN_YEAR) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable get3DV_LEAVE_REAL(string emp_id, string leave_plan_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from dbo.FN_D_3DV_LEAVE_REAL(@EMP_ID,@LEAVE_PLAN_YEAR) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "呼叫 Function"
    public string get_plan_3month(string leave_plan_year, string emp_id)
    {
        try
        {
            string plan_hour = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select isnull(dbo.FN_D_3DV_LEAVE_PLAN( @EMP_ID, @LEAVE_PLAN_YEAR),0) as CONTINUE_THREE_MONTH ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            DataTable dt = dbConn.Query(sb, ht, true);
            plan_hour = Convert.ToString(dt.Rows[0]["CONTINUE_THREE_MONTH"]);
            return plan_hour;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string get_actually_3month(string leave_plan_year, string emp_id)
    {
        try
        {
            string actually_hour = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select isnull(dbo.FN_D_3DV_LEAVE_REAL( @EMP_ID, @LEAVE_PLAN_YEAR),'') as CONTINUE_THREE_MONTH ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LEAVE_PLAN_YEAR", leave_plan_year);
            DataTable dt = dbConn.Query(sb, ht, true);
            actually_hour = Convert.ToString(dt.Rows[0]["CONTINUE_THREE_MONTH"]);
            return actually_hour;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}