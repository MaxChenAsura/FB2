using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DC0800DAO 的摘要描述
/// </summary>
public class CFB2DC0800DAO : BaseDAO
{
    public CFB2DC0800DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string calendar_dt_s, string calendar_dt_e,
                string dept_no, string duty_check_result, string remark, string is_super, string is_dept, string departments)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("WORK_CD"))
            {
                sortExpression = sortExpression.Replace("WORK_CD", "a.WORK_CD");
            }
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");
            }
            //測試用
            
           StringBuilder sb = new StringBuilder();
           Hashtable ht = new Hashtable();
           sb.Append("SP_D_DC0800_GETDATA");
           ht.Add("@startRowIndex", startRowIndex);
           ht.Add("@maximumRows", maximumRows);
           ht.Add("@sortExpression", sortExpression);
           ht.Add("@EMP_ID", emp_id);
           ht.Add("@CALENDAR_DT_S", calendar_dt_s);
           ht.Add("@CALENDAR_DT_E", calendar_dt_e);
           ht.Add("@DEPT_NO", dept_no);
           ht.Add("@DUTY_CHECK_RESULT", duty_check_result);
           ht.Add("@REMARK", remark);
           ht.Add("@IS_SUPER", is_super);
           ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);
           ht.Add("@DEPARTMENTS", departments);
           return dbConn.QuerySP(sb, ht, true);
           

            /*
            StringBuilder sb = new StringBuilder();
           Hashtable ht = new Hashtable();

           sb.Append(" Select * From");
           sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
           sb.Append(" a.EMP_ID,EMP_NAME,REPLACE(CONVERT(char(10), a.CALENDAR_DT, 120),'-','/') CALENDAR_DT,WORK_DAY_CD,WORK_DAY_CD + '-' + c.SUB_DESC WORK_DAY_DESC,");
           sb.Append(" REPLACE(CONVERT(char(16), a.DUTY_STIME, 120),'-','/') DUTY_STIME,REPLACE(CONVERT(char(16), a.DUTY_ETIME, 120),'-','/') DUTY_ETIME,");
           sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_IN_DT, 120),'-','/') CLOCK_IN_DT,IN_DATA_SOURCE_CD,IN_DATA_SOURCE_CD + '-' + d.SUB_DESC IN_DATA_SOURCE_DESC,");
           sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_OUT_DT, 120),'-','/') CLOCK_OUT_DT,OUT_DATA_SOURCE_CD,OUT_DATA_SOURCE_CD + '-' + e.SUB_DESC OUT_DATA_SOURCE_DESC,");
           sb.Append(" DUTY_CHECK_RESULT,DUTY_CHECK_RESULT + '-' + f.SUB_DESC DUTY_CHECK_RESULT_DESC,");
           sb.Append(" LATE_HOUR,LEAVE_EARLY_HOUR,LACK_HOUR,DUTY_HOUR,LEAVE_HOUR,LEAVE_INFO,OVERTIME_HOUR_APPLY,OVERTIME_HOUR_APPROVE,OVERTIME_PAY_HOUR,VIOLATE_BEFORE_HOUR,VIOLATE_AFTER_HOUR,");
           sb.Append(" OVERTIME_INFO,a.REMARK,WORK_SHIFT_ALLOWANCE_TYPE,WORK_SHIFT_ALLOWANCE_TYPE + '-' + g.SUB_DESC WORK_SHIFT_ALLOWANCE_TYPE_DESC");
           sb.Append(" ,a.WORK_CD +'-'+T9MCD_WORK_CD_CD.SUB_DESC WORK_CD_DESC,a.WORK_CD ");
           sb.Append(" ,b.DEPT_NO+'-'+b.DEPT_FULL_NAME as DEPT_FULL_NAME  ");
           sb.Append(" ,a.DT_TYPE,a.DT_TYPE +'-' +h.SUB_DESC as DT_TYPE_DESC ,a.O_APPROVE_OVERTIME_HOUR ");
           sb.Append(" from  (");

           sb.Append(" select a1.* from  ( ");
           sb.Append(" select a.* from TB_D_M_EMP_DUTY_CHECK_STATUS a with (nolock)  ");
            sb.Append(" where 1=1 ");
            
           if (emp_id != "")
           {
               sb.Append(" and a.EMP_ID = @EMP_ID ");
               ht.Add("@EMP_ID", emp_id );
           }
           if (calendar_dt_s != "")
           {
               sb.Append(" and a.CALENDAR_DT >= @calendar_dt_s ");
               ht.Add("@calendar_dt_s", calendar_dt_s);
           }
           if (calendar_dt_e != "")
           {
               sb.Append(" and a.CALENDAR_DT <= @calendar_dt_e ");
               ht.Add("@calendar_dt_e", calendar_dt_e);
           }
           if (dept_no != "")
           {
               sb.Append(" and a.DEPT_NO like @DEPT_NO ");
               ht.Add("@DEPT_NO", dept_no + "%");
           }
           if (duty_check_result != "-1")
           {
               if (duty_check_result.Equals("E0"))
               {
                   sb.Append(" and a.DUTY_CHECK_RESULT like 'E%' ");
               }
               else
               {
                   sb.Append(" and a.DUTY_CHECK_RESULT = @DUTY_CHECK_RESULT ");
                   ht.Add("@DUTY_CHECK_RESULT", duty_check_result);
               }
           }
           if (remark != "")
           {

               sb.Append(" and a.REMARK like @REMARK");
               ht.Add("@REMARK", remark + "%");

           }

           sb.Append(") a1  ");
           //顯示資料權限設定
           if (is_super != "Y")
           {
               sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on A1.EMP_ID=T.EMP_ID ");
               //sb.Append(@" where EMP_ID in  ( select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )  ");
               ht.Add("@loginID", SessionHandle.Current.emp_id);
               ht.Add("@departments", departments);
           }
           
           sb.Append(" ) a ");


           sb.Append(" inner join VW_H_EMP_DATA b with (nolock) on a.EMP_ID = b.EMP_ID");
           sb.Append(" left join TB_9_M_COMM_D c with (nolock) on a.WORK_DAY_CD = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'WORK_DAY_CD' ");
           sb.Append(" left join TB_9_M_COMM_D d with (nolock) on a.IN_DATA_SOURCE_CD = d.SUB_CD and d.SYS_CD = 'DC' and d.MAIN_CD = 'DATA_SOURCE_CD' ");
           sb.Append(" left join TB_9_M_COMM_D e with (nolock) on a.OUT_DATA_SOURCE_CD = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'DATA_SOURCE_CD' ");
           sb.Append(" left join TB_9_M_COMM_D f with (nolock) on a.DUTY_CHECK_RESULT = f.SUB_CD and f.SYS_CD = 'DC' and f.MAIN_CD = 'DUTY_CHECK_RESULT' ");
           sb.Append(" left join TB_9_M_COMM_D g with (nolock) on a.WORK_SHIFT_ALLOWANCE_TYPE = g.SUB_CD and g.SYS_CD = 'SC' and g.MAIN_CD = 'WORK_SHIFT_ALLOWANCE_TYPE' ");
           sb.Append(" left join TB_9_M_COMM_D T9MCD_WORK_CD_CD with (nolock) on T9MCD_WORK_CD_CD.SUB_CD=a.WORK_CD and T9MCD_WORK_CD_CD.MAIN_CD='WORK_CD' and T9MCD_WORK_CD_CD.SYS_CD='HB' ");
           sb.Append(" left join TB_9_M_COMM_D h  with (nolock) on h.main_cd = 'DT_TYPE' and h.sys_cd = 'DA' and h.IS_VALID='Y' and a.DT_TYPE=h.SUB_CD ");

           sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
           sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


           ht.Add("@startRowIndex", startRowIndex);
           ht.Add("@maximumRows", maximumRows);
           return dbConn.Query(sb, ht);
           */

        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string calendar_dt_s, string calendar_dt_e,
                string dept_no, string duty_check_result, string remark,string is_super, string is_dept, string departments)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select convert(int,TOTAL_NUM) as total_record 
                        from TB_9_M_GRID_NUM a  with (nolock)    
                        where CREATED_BY = @CREATED_BY
                        and FUNC_ID = @FUNC_ID");

            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DC0800");

            /*
            sb.Append(" select COUNT(*) total_record from TB_D_M_EMP_DUTY_CHECK_STATUS a  with (nolock)    ");
            sb.Append(" where 1=1 ");

            if (calendar_dt_s != "")
            {
                sb.Append(" and a.CALENDAR_DT >= @calendar_dt_s ");
                ht.Add("@calendar_dt_s", calendar_dt_s);
            }
            if (calendar_dt_e != "")
            {
                sb.Append(" and a.CALENDAR_DT <= @calendar_dt_e ");
                ht.Add("@calendar_dt_e", calendar_dt_e);
            }

            //顯示資料權限設定
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "");
            }
             
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no+"%");
            }
            if (duty_check_result != "-1")
            {
                if (duty_check_result.Equals("E0"))
                {
                    sb.Append(" and a.DUTY_CHECK_RESULT like 'E%' ");
                }
                else
                {
                    sb.Append(" and a.DUTY_CHECK_RESULT = @DUTY_CHECK_RESULT ");
                    ht.Add("@DUTY_CHECK_RESULT", duty_check_result);
                }
            }
            if (remark != "")
            {

                sb.Append(" and a.REMARK like @REMARK");
                ht.Add("@REMARK", remark + "%");
            }

            if (is_super != "Y")
            {

                //sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) ) T    on A.EMP_ID=T.EMP_ID ");
                sb.Append(@" and  EMP_ID in (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) ) ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", departments);
            }
            */
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

    public string EMP_ID { get; set; }

    public string CALENDAR_DT { get; set; }

    public string CALENDAR_DT_S { get; set; }

    public string CALENDAR_DT_E { get; set; }

    public string DEPT_NO { get; set; }

    public string DUTY_CHECK_RESULT { get; set; }

    public string REMARK { get; set; }

    internal DataTable searchResult()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dept.DEPT_NO+' '+dept.DIV_DEPT_FULL_NAME  DIV_DEPT_NAME ");
            sb.Append(" ,a.EMP_ID,EMP_NAME,REPLACE(CONVERT(char(10), a.CALENDAR_DT, 120),'-','/') CALENDAR_DT,WORK_DAY_CD,WORK_DAY_CD + '-' + c.SUB_DESC WORK_DAY_DESC,");
            sb.Append(" REPLACE(CONVERT(char(16), a.DUTY_STIME, 120),'-','/') DUTY_STIME,REPLACE(CONVERT(char(16), a.DUTY_ETIME, 120),'-','/') DUTY_ETIME,");
            sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_IN_DT, 120),'-','/') CLOCK_IN_DT,IN_DATA_SOURCE_CD,IN_DATA_SOURCE_CD + '-' + d.SUB_DESC IN_DATA_SOURCE_DESC,");
            sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_OUT_DT, 120),'-','/') CLOCK_OUT_DT,OUT_DATA_SOURCE_CD,OUT_DATA_SOURCE_CD + '-' + e.SUB_DESC OUT_DATA_SOURCE_DESC,");
            sb.Append(" DUTY_CHECK_RESULT,DUTY_CHECK_RESULT + '-' + f.SUB_DESC DUTY_CHECK_RESULT_DESC,");
            sb.Append(" LATE_HOUR,LEAVE_EARLY_HOUR,LACK_HOUR,DUTY_HOUR,LEAVE_HOUR,LEAVE_INFO,OVERTIME_HOUR_APPLY,OVERTIME_HOUR_APPROVE,VIOLATE_BEFORE_HOUR,OVERTIME_PAY_HOUR,VIOLATE_AFTER_HOUR,");
            sb.Append(" OVERTIME_INFO,a.REMARK,WORK_SHIFT_ALLOWANCE_TYPE,WORK_SHIFT_ALLOWANCE_TYPE + '-' + g.SUB_DESC WORK_SHIFT_ALLOWANCE_TYPE_DESC");
            sb.Append(" ,a.DT_TYPE,a.DT_TYPE +'-' +h.SUB_DESC as DT_TYPE_DESC ");
            sb.Append(" from (");
            sb.Append(" select * from TB_D_M_EMP_DUTY_CHECK_STATUS a with (nolock) ");
            sb.Append(" where 1=1 ");

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (CALENDAR_DT_S != "")
            {
                sb.Append(" and a.CALENDAR_DT >= @CALENDAR_DT_S ");
                ht.Add("@CALENDAR_DT_S", CALENDAR_DT_S);
            }
            if (CALENDAR_DT_E != "")
            {
                sb.Append(" and a.CALENDAR_DT <= @CALENDAR_DT_E ");
                ht.Add("@CALENDAR_DT_E", CALENDAR_DT_E);
            }
            if (DEPT_NO != "")
            {
                //sb.Append(" and exists(select EMP_ID from VW_H_EMP_DATA where VW_H_EMP_DATA.DEPT_NO = @DEPT_NO and VW_H_EMP_DATA.EMP_ID = a.EMP_ID) ");
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO + "%");
            }
            if (DUTY_CHECK_RESULT != "-1")
            {
                if (DUTY_CHECK_RESULT.Equals("E0"))
                {
                    sb.Append(" and a.DUTY_CHECK_RESULT like 'E%' ");
                }
                else if (DUTY_CHECK_RESULT.Equals("Y1"))
                {
                    sb.Append(" and  a.LATE_HOUR+a.LEAVE_EARLY_HOUR>0   ");
                }
                else
                {
                    sb.Append(" and a.DUTY_CHECK_RESULT = @DUTY_CHECK_RESULT ");
                    ht.Add("@DUTY_CHECK_RESULT", DUTY_CHECK_RESULT);
                }
            }
            if (REMARK != "")
            {

                sb.Append(" and a.REMARK like @REMARK");
                ht.Add("@REMARK", REMARK + "%");

            }

            sb.Append(" ) a");
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on A.EMP_ID=T.EMP_ID ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            sb.Append(" inner join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c on a.WORK_DAY_CD = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'WORK_DAY_CD' ");
            sb.Append(" left join TB_9_M_COMM_D d on a.IN_DATA_SOURCE_CD = d.SUB_CD and d.SYS_CD = 'DC' and d.MAIN_CD = 'DATA_SOURCE_CD' ");
            sb.Append(" left join TB_9_M_COMM_D e on a.OUT_DATA_SOURCE_CD = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'DATA_SOURCE_CD' ");
            sb.Append(" left join TB_9_M_COMM_D f on a.DUTY_CHECK_RESULT = f.SUB_CD and f.SYS_CD = 'DC' and f.MAIN_CD = 'DUTY_CHECK_RESULT' ");
            sb.Append(" left join TB_9_M_COMM_D g on a.WORK_SHIFT_ALLOWANCE_TYPE = g.SUB_CD and g.SYS_CD = 'SC' and g.MAIN_CD = 'WORK_SHIFT_ALLOWANCE_TYPE' ");
            sb.Append(" left join TB_H_R_DEPT_DATA dept on a.DEPT_NO=dept.DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D h  with (nolock) on h.main_cd = 'DT_TYPE' and h.sys_cd = 'DA' and h.IS_VALID='Y' and a.DT_TYPE=h.SUB_CD ");
           
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public string DEPT_NAME { get; set; }

    public string EMP_NAME { get; set; }

    public string DUTY_CHECK_RESULT_DESC { get; set; }

    //舊刷卡比對SP
    internal void callSP_D_CARD_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_CARD_COMPARE");
            ht.Add("@paraDate", DBNull.Value);
            ht.Add("@compareType", "2");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC080");
            
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    //新刷卡比對SP
    internal void call_SP_DC_CARD_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DC_CARD_COMPARE");
            ht.Add("@p_CALENDAR_DT", DBNull.Value);
            ht.Add("@p_COMPARE_TYPE", "2");
            ht.Add("@p_EMP_ID", "");
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DC080");

            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
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
    public DataTable getDEPT_NAME(string dept_no)
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

    public DataTable check_CALENDAR_DT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@" if @CALENDAR_DT>= dbo.FN_S_DUTY_EDT('LM')
	                            select 1 as resultCount 
                            ELSE
	                            select 0 as resultCount  ");
            ht.Add("@CALENDAR_DT", CALENDAR_DT);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }


    //將 日勤務狀態檔 比對結果 改為 N
    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", EMP_ID);
            ht.Add("@pCalendarDt", CALENDAR_DT);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC080");

            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新Reopen
    internal void SP_D_EMP_DUTY_CHECK_STATUS_REOPEN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_REOPEN");
            ht.Add("@p_EMP_ID", EMP_ID);
            ht.Add("@p_CALENDAR_SDT", CALENDAR_DT);
            ht.Add("@p_CALENDAR_EDT", CALENDAR_DT);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DC080");

            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


}