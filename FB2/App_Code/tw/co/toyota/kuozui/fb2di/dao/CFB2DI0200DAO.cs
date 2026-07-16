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
/// CFB2DI0200DAO 的摘要描述
/// </summary>
public class CFB2DI0200DAO : BaseDAO
{
    public string DEPT_NO { get; set; }
    public string WORK_CD { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

	public CFB2DI0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string work_cd, string is_valid)
    {
        try
        {
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");

            if (sortExpression.Contains("WORK_CD"))
                sortExpression = sortExpression.Replace("WORK_CD", "a.WORK_CD");

            if (sortExpression.Contains("START_DT"))
                sortExpression = sortExpression.Replace("START_DT", "a.START_DT");

            if (sortExpression.Contains("END_DT"))
                sortExpression = sortExpression.Replace("END_DT", "a.END_DT");

            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
          
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.DEPT_NO+'-'+isnull(b.DEPT_NAME,'') DEPT_NO,c.SUB_CD+'-'+c.SUB_DESC WORK_CD,a.START_DT,a.END_DT,a.REMARK ");
            sb.Append(" from TB_D_M_OVERTIME_SPECIAL_HOUR a ");
            sb.Append(" left join TB_H_M_DEPT b on b.DEPT_NO=a.DEPT_NO and GETDATE() >= b.START_DT and GETDATE()  <= b.END_DT ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD = 'HB' and c.MAIN_CD = 'WORK_CD' and c.IS_VALID='Y' and c.SUB_CD=a.WORK_CD ");
            sb.Append(" where 1=1 ");

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }

            if (work_cd != "-1" && work_cd != null)
            {
                sb.Append(" and a.WORK_CD = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }

            //if (start_dt != "")
            //{
            //    if (end_dt != "")
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt) and a.END_DT <= CONVERT(datetime,@end_dt)");
            //        ht.Add("@start_dt", start_dt);
            //        ht.Add("@end_dt", end_dt);
            //    }
            //    else
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt) ");
            //        ht.Add("@start_dt", start_dt);
            //    }
            //}
            //else if (end_dt != "")
            //{
            //    sb.Append(" and a.END_DT <= CONVERT(datetime,@end_dt) ");
            //    ht.Add("@end_dt", end_dt);
            //}

            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= a.START_DT and GETDATE()  <= a.END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= a.END_DT    ");
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string dept_no, string work_cd, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_SPECIAL_HOUR a ");
            sb.Append(" left join TB_H_M_DEPT b on b.DEPT_NO=a.DEPT_NO and GETDATE() >= b.START_DT and GETDATE()  <= b.END_DT  ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD = 'HB' and c.MAIN_CD = 'WORK_CD' and c.IS_VALID='Y' and c.SUB_CD=a.WORK_CD ");
            sb.Append(" where 1=1 ");

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }

            if (work_cd != "-1" && work_cd != null)
            {
                sb.Append(" and a.WORK_CD = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }

            //if (start_dt != "")
            //{
            //    if (end_dt != "")
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt) and a.END_DT <= CONVERT(datetime,@end_dt)");
            //        ht.Add("@start_dt", start_dt);
            //        ht.Add("@end_dt", end_dt);
            //    }
            //    else
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt) ");
            //        ht.Add("@start_dt", start_dt);
            //    }
            //}
            //else if (end_dt != "")
            //{
            //    sb.Append(" and a.END_DT <= CONVERT(datetime,@end_dt) ");
            //    ht.Add("@end_dt", end_dt);
            //}

            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= a.START_DT and GETDATE()  <= a.END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= a.END_DT    ");
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

    public void deleteOVERTIME_SPECIAL_HOUR(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_SPECIAL_HOUR set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI020' ");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and WORK_CD = @WORK_CD and START_DT=@START_DT;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_SPECIAL_HOUR");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and WORK_CD = @WORK_CD and START_DT=@START_DT;");
            ht.Add("@DEPT_NO", item.Item1);
            ht.Add("@WORK_CD", item.Item2);
            ht.Add("@START_DT", item.Item3);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_APPLY(string dept_no, string work_cd, string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY");
            sb.Append(" where EMP_ID in ( select EMP_ID from TB_H_M_EMP ");
            sb.Append(" where DEPT_NO=@DEPT_NO and WORK_CD=@WORK_CD) ");
            sb.Append(" and APPLY_OVERTIME_DT >= CONVERT(datetime,@start_dt) and  APPLY_OVERTIME_DT <= CONVERT(datetime,@end_dt)");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@WORK_CD", work_cd);
            ht.Add("@start_dt", start_dt);
            ht.Add("@end_dt", end_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_IFLOW(string dept_no, string work_cd, string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.EMP_ID ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a");
            sb.Append(" left join TB_D_M_OVERTIME_APPLY b on a.EMP_ID=b.EMP_ID and a.IFLOW_NO=b.IFLOW_NO ");
            sb.Append(" where a.EMP_ID in ( select EMP_ID from TB_H_M_EMP ");
            sb.Append(" where DEPT_NO=@DEPT_NO and WORK_CD=@WORK_CD) ");
            sb.Append(" and b.APPLY_OVERTIME_DT >= CONVERT(datetime,@start_dt) and b.APPLY_OVERTIME_DT <= CONVERT(datetime,@end_dt)");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@WORK_CD", work_cd);
            ht.Add("@start_dt", start_dt);
            ht.Add("@end_dt", end_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getSalaryYM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @startDT datetime=@START_DT; 
                        if  CONVERT(VARCHAR(6),dbo.FN_D_DUTY_CLOSE_DT(-1),112)  >=CONVERT(VARCHAR(6),@startDT,112)  
                        BEGIN
	                        select 1 resultCount, CONVERT(VARCHAR(6),dbo.FN_D_DUTY_CLOSE_DT(-1),112)  salaryYM 
                        END
                        else 
                        BEGIN
	                        select 0 resultCount, CONVERT(VARCHAR(6),dbo.FN_D_DUTY_CLOSE_DT(-1),112)  salaryYM 
                        END
                        ");
            ht.Add("@START_DT", START_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO from TB_D_M_OVERTIME_SPECIAL_HOUR");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and WORK_CD = @WORK_CD ");
            sb.Append(" and  ((START_DT <= @START_DT and END_DT >= @START_DT) ");
            sb.Append(" or (START_DT <= @END_DT and END_DT >= @END_DT) ");
            sb.Append(" or (START_DT >= @START_DT and END_DT <= @END_DT)) ");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addOVERTIME_SPECIAL_HOUR()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_SPECIAL_HOUR( ");
            sb.Append(" DEPT_NO,WORK_CD,START_DT,END_DT,REMARK, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values ( ");
            sb.Append(" @DEPT_NO,@WORK_CD,@START_DT,@END_DT,@REMARK, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@START_DT", START_DT);
            if(END_DT =="")
                ht.Add("@END_DT", DBNull.Value);
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void emp_duty_check_status_reopen()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"UPDATE TB_D_M_EMP_DUTY_CHECK_STATUS
                        set DUTY_CHECK_RESULT = 'N' 
                            ,LATE_HOUR = 0
                            ,LEAVE_EARLY_HOUR = 0
                            ,LACK_HOUR = 0
                            ,DUTY_HOUR = 0
                            ,LEAVE_HOUR = 0
                            ,LEAVE_INFO = ''
                            ,OVERTIME_HOUR_APPLY = 0
                            ,OVERTIME_HOUR_APPROVE = 0
                            ,VIOLATE_BEFORE_HOUR = 0
                            ,VIOLATE_AFTER_HOUR = 0
                            ,OVERTIME_INFO = ''
                            ,SHIFT_CD = ''
                            ,WORK_SHIFT_ALLOWANCE_TYPE = ''
                            ,UPDATED_BY=@UPDATED_BY
	                        ,UPDATED_DT=GETDATE()
	                        ,FUNC_ID=@FUNC_ID
                        where emp_id in (select EMP_ID from VW_H_EMP_DATA where DEPT_NO=@DEPT_NO and WORK_CD=@WORK_CD and EMP_STATUS<>'99'  )
                        and CALENDAR_DT>=@START_DT and CALENDAR_DT<=@END_DT
                        and CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')
                        ");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", DBNull.Value);
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public void updateOVERTIME_SPECIAL_HOUR()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_SPECIAL_HOUR ");
            sb.Append(" set END_DT=@END_DT,REMARK=@REMARK, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and WORK_CD = @WORK_CD and START_DT=@START_DT");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", DBNull.Value);
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO=@DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}