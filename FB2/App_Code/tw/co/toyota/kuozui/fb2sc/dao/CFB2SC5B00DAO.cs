using System;
using System.Collections;
using System.Data;
using System.Text;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SC5B00DAO 的摘要描述
/// </summary>
public class CFB2SC5B00DAO : BaseDAO
{
    public CFB2SC5B00DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_sdt, string salary_edt, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_YM desc,SALARY_TYPE,SALARY_DT"))
                sortExpression = sortExpression.Replace("SALARY_YM desc,SALARY_TYPE,SALARY_DT", "t.SALARY_YM desc,t.SALARY_TYPE,p.SALARY_DT");
            else
            {
                if (sortExpression.Contains("SALARY_TYPE"))
                    sortExpression = sortExpression.Replace("SALARY_TYPE", "t.SALARY_TYPE");
                if (sortExpression.Contains("SALARY_YM"))
                    sortExpression = sortExpression.Replace("SALARY_YM", "t.SALARY_YM");
                if (sortExpression.Contains("SALARY_DT"))
                    sortExpression = sortExpression.Replace("SALARY_DT", "t.SALARY_DT");
                if (sortExpression.Contains("SALARY_SDT"))
                    sortExpression = sortExpression.Replace("SALARY_SDT", "t.SALARY_SDT");
                if (sortExpression.Contains("SALARY_EDT"))
                    sortExpression = sortExpression.Replace("SALARY_EDT", "t.SALARY_EDT");
                if (sortExpression.Contains("SALARY_NAME"))
                    sortExpression = sortExpression.Replace("SALARY_NAME", "m.SALARY_NAME");
                if (sortExpression.Contains("PROCESS_STATUS"))
                    sortExpression = sortExpression.Replace("PROCESS_STATUS", "t.PROCESS_STATUS");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, "); //t.SALARY_YM desc,t.SALARY_TYPE,p.SALARY_DT
            sb.Append(" CONVERT(VARCHAR(10), t.SALARY_DT,111 )as SALARY_DT,t.SALARY_YM,CONCAT('',e.SUB_DESC)SALARY_TYPE,CONVERT(VARCHAR(10) , t.SALARY_SDT, 111 )as SALARY_SDT ,CONVERT(VARCHAR(10) , t.SALARY_EDT, 111 )as SALARY_EDT, ");
            sb.Append(" d.SUB_DESC as PROCESS_STATUS,t.SALARY_TYPE as hid_SALARY_TYPE,t.PROCESS_STATUS as hid_PROCESS_STATUS,m.SALARY_NAME,t.PAY_KIND as hid_PAY_KIND ");
            sb.Append(" from TB_S_M_SALARY_CAL_H t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE  ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.Append(" left join TB_S_M_SALARY_ITEM m on m.SALARY_ID=t.PAY_KIND");
            sb.Append(" where 1=1 and t.PROCESS_STATUS<>'4' ");

            if (salary_sdt != "")
            {
                sb.Append(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.Append(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_type != "-1")
            {
                sb.Append(" and t.SALARY_TYPE =@SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string salary_sdt, string salary_edt, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_CAL_H t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE  ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.Append(" left join TB_S_M_SALARY_ITEM m on m.SALARY_ID=t.PAY_KIND");
            sb.Append(" where 1=1 and t.PROCESS_STATUS<>'4' ");

            if (salary_sdt != "")
            {
                sb.Append(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.Append(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_type != "-1")
            {
                sb.Append(" and t.SALARY_TYPE =@SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
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
    public DataTable getSALARY_TYPE(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_desc From TB_9_M_COMM_D ");
            sb.Append("Where sys_cd = @sys_cd ");
            sb.Append("and main_cd = @main_cd ");
            sb.Append("and is_valid = @is_valid ");
            ht.Add("@sys_cd", sys_cd);
            ht.Add("@main_cd", main_cd);
            ht.Add("@is_valid", is_valid);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal void deleteData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" truncate table  TB_S_S_EMP_DUTY_LACK_TMP  ");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void ins3DayTmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_FB2SC5B0_INS_TB_S_S_EMP_DUTYLACK_3DAYTMP");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable selectEmp(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct t.* from ( ");
            sb.Append(" Select a.EMP_ID,c.LACK_HOUR ");
            sb.Append(" From TB_S_M_EMP_RESULT a  ");
            sb.Append(" left join TB_S_S_EMP_DUTYLACK_3DAYTMP b on b.EMP_ID=a.EMP_ID ");
            sb.Append(" left join TB_D_M_EMP_DUTY_CHECK_STATUS c on b.EMP_ID=c.emp_id and b.CALENDAR_DT= c.CALENDAR_DT  ");
            sb.Append("WHERE a.SALARY_YM=@SALARY_YM ");
            sb.Append(" ) t where t.lack_hour >= 480  /*480 分表示欠勤1天*/ ");
            ht.Add("@SALARY_YM", SALARY_YM);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable selectEmpTmp(string SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct t.* from ( ");
            sb.Append(" Select a.EMP_ID,ISNULL(c.LACK_HOUR,0)LACK_HOUR ");
            sb.Append(" From TB_S_M_EMP_RESULT_TMP a  ");
            sb.Append(" left join TB_S_S_EMP_DUTYLACK_3DAYTMP b on b.EMP_ID=a.EMP_ID ");
            sb.Append(" left join TB_D_M_EMP_DUTY_CHECK_STATUS c on b.EMP_ID=c.emp_id and b.CALENDAR_DT= c.CALENDAR_DT  ");
            sb.Append("WHERE a.PAY_KIND=@PAY_KIND AND a.salary_type=@SALARY_TYPE and a.salary_dt=@SALARY_DT ");
            sb.Append(" ) t where t.lack_hour >= 480  /*480 分表示欠勤1天*/  ");
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

  
    internal void insertEmp(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO  TB_S_S_EMP_DUTY_LACK_TMP(EMP_ID,CALENDAR_DT,LACK_HOUR)  ");
            sb.Append(" SELECT TOP(3) EMP_ID,CALENDAR_DT,LACK_HOUR ");
            sb.Append(" FROM TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" where WORK_DAY_CD ='1' and EMP_ID = @EMP_ID ");
            sb.Append(" order by CALENDAR_DT desc ");
            ht.Add("@EMP_ID", emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getData1(string salary_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(varchar(12) , B2.CALENDAR_DT, 111 )CALENDAR_DT,B1.EMP_ID,B3.EMP_NAME,B3.DEPT_NAME,B3.DEPT_NO                                   ");
	        sb.Append("       ,B3.PJOB_CD,B3.PJOB_DESC,B3.LEAVE_DT, B4.SHIFT_DESC,'' as SUB_LEAVE_CD                                                                  ");
	        sb.Append("       ,REPLACE(CONVERT(char(16), B2.CLOCK_IN_DT, 120),'-','/') CLOCK_IN_DT                                                                    ");
	        sb.Append("       ,REPLACE(CONVERT(char(16), B2.CLOCK_OUT_DT, 120),'-','/')CLOCK_OUT_DT,'E3-欠勤' as MSG                                                  ");
            sb.Append("       ,CAST(ROUND(B2.LACK_HOUR/60,0)as int) as LACK_HOUR,CAST(ROUND(B5.TOTAL_LACK_HOUR  /60,0)as int) as TOTAL_LACK_HOUR                      ");
            sb.Append(" from TB_S_S_EMP_DUTYLACK_3DAYTMP B1                                                                                                              ");
            sb.Append(" left join TB_D_M_EMP_DUTY_CHECK_STATUS B2 on B1.EMP_ID = B2.EMP_ID and B1.CALENDAR_DT =B2.CALENDAR_DT                                         ");
            sb.Append(" left join VW_H_EMP_DATA B3 on B1.EMP_ID = B3.EMP_ID                                                                                           ");
            sb.Append(" left join TB_D_M_SHIFT_H B4 on B2.SHIFT_CD = B4.SHIFT_CD and B4.START_DT <= GETDATE() and B4.END_DT >= GETDATE()                              ");
            sb.Append(" left join (select EMP_ID,SUM(LACK_HOUR)as TOTAL_LACK_HOUR                                                                                     ");
            sb.Append("            from TB_S_S_EMP_DUTY_LACK_TMP GROUP BY EMP_ID) B5 on B1.EMP_ID = B5.EMP_ID                                                         ");
            sb.Append(" UNION                                                                                                                                         ");
            sb.Append(" select CONVERT(varchar(12) , B2.APPLY_LEAVE_SDT, 111 ) CALENDAR_DT,B1.EMP_ID,B3.EMP_NAME,B3.DEPT_NAME,B3.DEPT_NO                              ");
            sb.Append("       ,B3.PJOB_CD,B3.PJOB_DESC,B3.LEAVE_DT,'' as SHIFT_DESC,t.SUB_LEAVE_desc as SUB_LEAVE_CD,NULL as CLOCK_IN_DT                               ");
            sb.Append("       ,NULL as CLOCK_OUT_DT,'請假' as MSG,CAST(ROUND(B2.TOTAL_TIME_APPROVE/60,0)as int) as LACK_HOUR                                          ");
            sb.Append("       ,CAST(ROUND(B5.TOTAL_TIME_APPROVE/60,0)as int)  as TOTAL_LACK_HOUR                                                                      ");
            sb.Append(" from TB_S_M_EMP_RESULT B1                                                                                                                     ");
            sb.Append(" left join VW_H_EMP_DATA B3 on B1.EMP_ID = B3.EMP_ID                                                                                           ");
            sb.Append(" left join (select EMP_ID,SUM(TOTAL_TIME_APPROVE)as TOTAL_TIME_APPROVE                                                                         ");
            sb.Append("            from TB_D_M_LEAVE_APPLY_DAY                                                                                                        ");
            sb.Append("            WHERE left(CONVERT(varchar(12) , APPLY_LEAVE_STIME, 112 ),6) =@SALARY_YM   ");
            sb.Append("              and FORM_STATUS not in('N','D') and (MAIN_LEAVE_CD in ('A','Q') or SUB_LEAVE_CD in ('C0','B0'))                                                   ");
            sb.Append("            GROUP BY EMP_ID) B5 on B1.EMP_ID = B5.EMP_ID                                                                                       ");
            sb.Append(" left join TB_D_M_LEAVE_APPLY_DAY B2 on left(CONVERT(varchar(12) , APPLY_LEAVE_STIME, 112 ),6) =@SALARY_YM   ");
            sb.Append("       and FORM_STATUS not in('N','D') and (MAIN_LEAVE_CD in ('A','Q') or  SUB_LEAVE_CD in ('C0','B0')) and B5.EMP_ID = B2.EMP_ID                                 ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D t on t.sub_leave_cd=B2.sub_leave_cd ");
            sb.Append(" where (ROUND(B5.TOTAL_TIME_APPROVE/60,0) >=40 or (B2.MAIN_LEAVE_CD ='Q' and ROUND(B5.TOTAL_TIME_APPROVE/60,0) >=8 )) and B1.SALARY_YM = @SALARY_YM  ");
            sb.Append(" order by B1.EMP_ID,CALENDAR_DT,B3.DEPT_NO                                                                                                     ");

            ht.Add("@SALARY_YM", salary_ym.Replace("/",""));
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getData2(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(varchar(12) , B2.CALENDAR_DT, 111 )CALENDAR_DT,B1.EMP_ID,B3.EMP_NAME,B3.DEPT_NAME,B3.DEPT_NO    ");
            sb.Append("       ,B3.PJOB_CD,B3.PJOB_DESC,B3.LEAVE_DT, B4.SHIFT_DESC,'' as SUB_LEAVE_CD                                   ");
            sb.Append("       ,REPLACE(CONVERT(char(16), B2.CLOCK_IN_DT, 120),'-','/') CLOCK_IN_DT                                     ");
            sb.Append("       ,REPLACE(CONVERT(char(16), B2.CLOCK_OUT_DT, 120),'-','/')CLOCK_OUT_DT,'E3-欠勤' as MSG                   ");
            sb.Append("       ,CAST(ROUND(B2.LACK_HOUR/60,0)as int) as LACK_HOUR,CAST(ROUND(B5.TOTAL_LACK_HOUR  /60,0)as int) as TOTAL_LACK_HOUR ");
            sb.Append(" from TB_S_S_EMP_DUTYLACK_3DAYTMP B1 ");
            sb.Append(" left join TB_D_M_EMP_DUTY_CHECK_STATUS B2 on B1.EMP_ID = B2.EMP_ID and B1.CALENDAR_DT =B2.CALENDAR_DT ");
            sb.Append(" left join VW_H_EMP_DATA B3 on B1.EMP_ID = B3.EMP_ID");
            sb.Append(" left join TB_D_M_SHIFT_H B4 on B2.SHIFT_CD = B4.SHIFT_CD and B4.START_DT <= GETDATE() and B4.END_DT >= GETDATE() ");
            sb.Append(" left join (select EMP_ID,SUM(LACK_HOUR)as TOTAL_LACK_HOUR ");
            sb.Append("            from TB_S_S_EMP_DUTY_LACK_TMP GROUP BY EMP_ID) B5 on B1.EMP_ID = B5.EMP_ID ");
            sb.Append(" UNION ");
            sb.Append(" select CONVERT(varchar(12) , B2.APPLY_LEAVE_SDT, 111 ) CALENDAR_DT,B1.EMP_ID,B3.EMP_NAME,B3.DEPT_NAME,B3.DEPT_NO ");
            sb.Append("       ,B3.PJOB_CD,B3.PJOB_DESC,B3.LEAVE_DT,'' as SHIFT_DESC,t.SUB_LEAVE_desc as SUB_LEAVE_CD,NULL as CLOCK_IN_DT ");
            sb.Append("       ,NULL as CLOCK_OUT_DT,'請假' as MSG,CAST(ROUND(B2.TOTAL_TIME_APPROVE/60,0)as int) as LACK_HOUR ");
            sb.Append("       ,CAST(ROUND(B5.TOTAL_TIME_APPROVE/60,0)as int)  as TOTAL_LACK_HOUR  ");
            sb.Append(" from TB_S_M_EMP_RESULT_TMP B1 ");
            sb.Append(" left join VW_H_EMP_DATA B3 on B1.EMP_ID = B3.EMP_ID  ");
            sb.Append(" left join (select EMP_ID,SUM(TOTAL_TIME_APPROVE)as TOTAL_TIME_APPROVE ");
            sb.Append("            from TB_D_M_LEAVE_APPLY_DAY  ");
            sb.Append("            WHERE left(CONVERT(varchar(12) , APPLY_LEAVE_STIME,112 ),6) =left(CONVERT(varchar(12) , @SALARY_DT, 112 ),6) ");
            sb.Append("            and FORM_STATUS not in('N','D') and (MAIN_LEAVE_CD in ('A','Q') or SUB_LEAVE_CD in ('C0','B0')) ");
            sb.Append("            GROUP BY EMP_ID) B5 on B1.EMP_ID = B5.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_APPLY_DAY B2 on left(CONVERT(varchar(12) ,APPLY_LEAVE_STIME, 112 ),6) =left(CONVERT(varchar(12) , @SALARY_DT, 112 ),6) ");
            sb.Append("       and FORM_STATUS not in('N','D') and (MAIN_LEAVE_CD in ('A','Q') or SUB_LEAVE_CD in ('C0','B0')) and B5.EMP_ID = B2.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D t on t.sub_leave_cd=B2.sub_leave_cd ");
            sb.Append(" where  (ROUND(B5.TOTAL_TIME_APPROVE/60,0) >=40 or (B2.MAIN_LEAVE_CD ='Q' and ROUND(B5.TOTAL_TIME_APPROVE/60,0) >=8 )) ");
            sb.Append(" and B1.SALARY_DT = @SALARY_DT and B1.SALARY_TYPE =@SALARY_TYPE and B1.PAY_KIND = @PAY_KIND ");
            sb.Append(" order by B1.EMP_ID,CALENDAR_DT,B3.DEPT_NO ");

            ht.Add("@SALARY_DT", salary_dt.Replace("/","") );
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

}