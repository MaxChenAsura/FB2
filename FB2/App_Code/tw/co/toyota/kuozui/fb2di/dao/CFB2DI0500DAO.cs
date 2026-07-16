using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data.OleDb;
using System.Data.SqlClient;

/// <summary>
/// CFB2DI0500DAO 的摘要描述
/// </summary>
public class CFB2DI0500DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string SHIFT_CD { get; set; }
    public string OVERTIME_REASON { get; set; }
    public string BEFORE_STIME { get; set; }
    public string BEFORE_ETIME { get; set; }
    public string BEFORE_HOUR { get; set; }
    public string AFTER_STIME { get; set; }
    public string AFTER_ETIME { get; set; }
    public string AFTER_HOUR { get; set; }
    public string APPLY_OVERTIME_HOUR { get; set; }
    public string APPROVE_OVERTIME_HOUR { get; set; }
    public string IS_APPLY { get; set; }
    public string EXCHANGE_HOUR { get; set; }
    public string CLOCK_IN_TIME { get; set; }
    public string CLOCK_OUT_TIME { get; set; }
    public string IS_CONFIRM_CHECK { get; set; }
    public string OVERTIME_DT_TYPE { get; set; }
    public string OVERTIME_TIME_CD { get; set; }
    public string APPLY_OVERTIME_DT { get; set; }
    public string REPLACE_DT { get; set; }
    public string IFLOW_NO { get; set; }
    public string FORM_STATUS { get; set; }
    public string REMARK { get; set; }
    public string IS_CONFIRM_CLOSE { get; set; }
    public string SALARY_SETTLE_STATUS { get; set; }
    public string PAY_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string OVERTIME_CTL_CD { get; set; }
    public string OVERTIME_CD { get; set; }
    public string IFLOW_APPROVE_DT { get; set; }
    public string CHECK_STATUS { get; set; }

    public string TRIP_STIME { get; set; }
    public string TRIP_ETIME { get; set; }
    public string CALENDAR_DT { get; set; }
    public string DT_TYPE { get; set; }
    public string TRIP_HOUR { get; set; }
    public string APPROVE_BEFORE_HOUR { get; set; }
    public string APPROVE_AFTER_HOUR { get; set; }
    public string OVERTIME_PAY_HOUR { get; set; }
    public string HYPER_HOUR { get; set; }
    public string NORMAL_HOUR { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string O_SPECIAL_CD { get; set; }
    public string COURSE_LOG { get; set; }
    public string CLOSED_BY { get; set; }
    public string CLOSED_DT { get; set; }
    public string WORK_CD { get; set; }
    public string TARGET_TYPE { get; set; }
    public string WS_CD { get; set; }
    public string PJOB_CD { get; set; }

    public string IS_ADD { get; set; }

    public string RTN_Message { get; set; }
    public string RTN_Flag { get; set; }

    public CFB2DI0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string emp_name, string dept_no,
                             string overtime_cd, string overtime_time_cd, string iflow_no, string iflow_approve_dt,
                             string check_status, string apply_overtime_dt_s, string apply_overtime_dt_e, string form_status, string o_special_cd)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");
            }
            if (sortExpression.Contains("OVERTIME_CD"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_CD", "a.OVERTIME_CD");
            }
            if (sortExpression.Contains("OVERTIME_DT_TYPE"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_DT_TYPE", "a.OVERTIME_DT_TYPE");
            }
            if (sortExpression.Contains("IS_DUTY_CHECK"))
            {
                sortExpression = sortExpression.Replace("IS_DUTY_CHECK", "a.IS_DUTY_CHECK");
            }
            StringBuilder sb_OVERTIME_APPLY = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb_OVERTIME_APPLY.Append(@" select * from  TB_D_M_OVERTIME_APPLY a  with (nolock) 
                                        where a.FORM_STATUS <>'D' ");


            if (emp_id != "")
            {
                sb_OVERTIME_APPLY.Append("and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb_OVERTIME_APPLY.Append("and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (overtime_cd != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.OVERTIME_CD = LEFT(@OVERTIME_CD,1) ");
                ht.Add("@OVERTIME_CD", overtime_cd);
            }
            if (overtime_time_cd != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.OVERTIME_TIME_CD = LEFT(@OVERTIME_TIME_CD,1) ");
                ht.Add("@OVERTIME_TIME_CD", overtime_time_cd);
            }
            if (iflow_no != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb_OVERTIME_APPLY.Append(" and DATEPART(YY,IFLOW_APPROVE_DT) = LEFT(@IFLOW_APPROVE_DT,4) ");
                sb_OVERTIME_APPLY.Append(" and DATEPART(MM,IFLOW_APPROVE_DT) = RIGHT(@IFLOW_APPROVE_DT,2) ");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }
            if (apply_overtime_dt_s != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT >= @APPLY_OVERTIME_DT_S ");
                ht.Add("@APPLY_OVERTIME_DT_S", apply_overtime_dt_s);
            }
            if (apply_overtime_dt_e != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT <= @APPLY_OVERTIME_DT_E ");
                ht.Add("@APPLY_OVERTIME_DT_E", apply_overtime_dt_e);
            }
            if (check_status != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.CHECK_STATUS = @CHECK_STATUS ");
                ht.Add("@CHECK_STATUS", check_status);
            }
            if (form_status != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", form_status);
            }
            if (o_special_cd != "-1" && o_special_cd != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.O_SPECIAL_CD = @O_SPECIAL_CD ");
                ht.Add("@O_SPECIAL_CD", o_special_cd);
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" a.EMP_ID, b.EMP_NAME, CONVERT(char(10), APPLY_OVERTIME_DT, 111) APPLY_OVERTIME_DT, ");
            sb.AppendLine(" a.OVERTIME_CD, a.OVERTIME_CD +'-'+d.OVERTIME_DESC as OVERTIME_DESC, ");
            sb.AppendLine(" a.OVERTIME_DT_TYPE, a.OVERTIME_DT_TYPE +'-' +j.SUB_DESC as OVERTIME_DT_TYPE_DESC, ");
            sb.AppendLine(" a.DT_TYPE, a.DT_TYPE +'-' +h.SUB_DESC as DT_TYPE_DESC , ");
            sb.AppendLine(" convert(nvarchar(25), floor(APPLY_OVERTIME_HOUR/60))+':'+right('0'+CONVERT(nvarchar(25),floor(APPLY_OVERTIME_HOUR%60)),2) APPLY_OVERTIME_HOUR, ");
            sb.AppendLine(" convert(nvarchar(25), floor(APPROVE_OVERTIME_HOUR/60))+':'+right('0'+CONVERT(nvarchar(25),floor(APPROVE_OVERTIME_HOUR%60)),2) APPROVE_OVERTIME_HOUR, ");
            sb.AppendLine(" convert(nvarchar(25), floor(OVERTIME_PAY_HOUR/60))+':'+right('0'+CONVERT(nvarchar(25),floor(OVERTIME_PAY_HOUR%60)),2) OVERTIME_PAY_HOUR, ");
            sb.AppendLine(" convert(nvarchar(25), floor(BEFORE_HOUR/60))+':'+right('0'+convert(nvarchar(25),floor(BEFORE_HOUR%60)),2) BEFORE_HOUR, ");
            sb.AppendLine(" concat(CONVERT(char(5),BEFORE_STIME,108),'~'+CONVERT(char(5),BEFORE_ETIME,108))AS BSETIME, ");
            sb.AppendLine(" convert(nvarchar(25), floor(AFTER_HOUR/60))+':'+right('0'+convert(nvarchar(25),floor(AFTER_HOUR%60)),2) AFTER_HOUR, ");
            sb.AppendLine(" concat(CONVERT(char(5),AFTER_STIME,108),'~'+CONVERT(char(5),AFTER_ETIME,108))AS ASETIME, ");
            sb.AppendLine(" CONVERT(CHAR(7),A.IFLOW_APPROVE_DT,111) as IFLOW_APPROVE_DT, ");
            sb.AppendLine(" a.IS_DUTY_CHECK, g.SUB_CD+'-'+g.SUB_DESC CHECK_STATUS, ");
            sb.AppendLine(" f.SUB_CD+'-'+f.SUB_DESC FORM_STATUS, ");
            sb.AppendLine(" a.IFLOW_NO, c.DEPT_NO, c.DEPT_NO+'-'+c.DEPT_NAME DEPT_NAME,a.IS_APPLY ");
            //sb.AppendLine(" from TB_D_M_OVERTIME_APPLY a with (nolock) ");
            sb.Append(" from  ( " + sb_OVERTIME_APPLY + " ) a");
            sb.AppendLine(" left join VW_H_EMP_DATA b  with (nolock) on a.EMP_ID = b.EMP_ID ");
            sb.AppendLine(" left join VW_H_DEPT_DATA c  with (nolock) on a.DEPT_NO = c.DEPT_NO ");
            sb.AppendLine(" left join TB_D_M_OVERTIME_TYPE d  with (nolock) on d.OVERTIME_CD=a.OVERTIME_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D e  with (nolock) on e.main_cd = 'OVERTIME_TIME_CD' and e.sys_cd = 'DI' and e.IS_VALID='Y' and a.OVERTIME_TIME_CD=e.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D f  with (nolock) on f.main_cd = 'FORM_STATUS' and f.sys_cd = 'DH' and f.IS_VALID='Y' and a.FORM_STATUS=f.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D g  with (nolock) on g.main_cd = 'CHECK_STATUS' and g.sys_cd = 'DI' and g.IS_VALID='Y' and a.CHECK_STATUS=g.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D h  with (nolock) on h.main_cd = 'DT_TYPE' and h.sys_cd = 'DA' and h.IS_VALID='Y' and a.DT_TYPE=h.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D j  with (nolock) on j.main_cd = 'OVERTIME_DT_TYPE' and j.sys_cd = 'DI' and j.IS_VALID='Y' and a.OVERTIME_DT_TYPE=j.SUB_CD ");
            sb.AppendLine(" where 1=1 ");

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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string emp_name, string dept_no,
                             string overtime_cd, string overtime_time_cd, string iflow_no, string iflow_approve_dt,
                             string check_status, string apply_overtime_dt_s, string apply_overtime_dt_e, string form_status, string o_special_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a  with (nolock) ");
            sb.Append(" where a.FORM_STATUS <>'D'  ");

            if (emp_id != "")
            {
                sb.Append("and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append("and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (overtime_cd != "-1")
            {
                sb.Append(" and a.OVERTIME_CD = LEFT(@OVERTIME_CD,1) ");
                ht.Add("@OVERTIME_CD", overtime_cd);
            }
            if (overtime_time_cd != "-1")
            {
                sb.Append(" and a.OVERTIME_TIME_CD = LEFT(@OVERTIME_TIME_CD,1) ");
                ht.Add("@OVERTIME_TIME_CD", overtime_time_cd);
            }
            if (iflow_no != "")
            {
                sb.Append(" and a.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb.Append(" and DATEPART(YY,IFLOW_APPROVE_DT) = LEFT(@IFLOW_APPROVE_DT,4) ");
                sb.Append(" and DATEPART(MM,IFLOW_APPROVE_DT) = RIGHT(@IFLOW_APPROVE_DT,2) ");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }
            if (apply_overtime_dt_s != "")
            {
                sb.Append(" and a.APPLY_OVERTIME_DT >= @APPLY_OVERTIME_DT_S ");
                ht.Add("@APPLY_OVERTIME_DT_S", apply_overtime_dt_s);
            }
            if (apply_overtime_dt_e != "")
            {
                sb.Append(" and a.APPLY_OVERTIME_DT <= @APPLY_OVERTIME_DT_E ");
                ht.Add("@APPLY_OVERTIME_DT_E", apply_overtime_dt_e);
            }
            if (check_status != "-1")
            {
                sb.Append(" and a.CHECK_STATUS = @CHECK_STATUS ");
                ht.Add("@CHECK_STATUS", check_status);
            }
            if (form_status != "-1")
            {
                sb.Append(" and a.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", form_status);
            }
            if (o_special_cd != "-1" && o_special_cd != null)
            {
                sb.Append(" and a.O_SPECIAL_CD = @O_SPECIAL_CD ");
                ht.Add("@O_SPECIAL_CD", o_special_cd);
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
    public DataTable getBatchData(int startRowIndex, int maximumRows, string sortExpression, string plant_cd, string dept_no,
                         string ws_cd, string work_cd, string work_shift_cd, string AddEmp, string DeleteEmp)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" EMP_ID,EMP_NAME, DEPT_NO,DEPT_FULL_NAME,CONCAT(PLANT_CD,'-'+PLANT_NAME)AS PLANT_CD,");
            sb.Append(" CONCAT(WORK_CD,'-'+WORK_DESC)AS WORK_CD,CONCAT(WORK_SHIFT_CD,'-'+WORK_SHIFT_DESC)AS WORK_SHIFT_CD,CONCAT(WS_CD,'-'+WS_DESC)AS WS_CD ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS ='01' ");


            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }
            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@addEMP_IDS) )");
                ht.Add("@addEMP_IDS", arrAddEmp);
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
    public int getBatchCount(int startRowIndex, int maximumRows, string plant_cd, string dept_no,
                         string ws_cd, string work_cd, string work_shift_cd, string AddEmp, string DeleteEmp)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS ='01'");


            if (dept_no != "")
            {
                sb.Append("and DEPT_NO like @DEPT_NO");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }
            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@EMP_IDS) )");
                ht.Add("@EMP_IDS", arrAddEmp);
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

    public DataTable getConfirmData(int startRowIndex, int maximumRows, string sortExpression,
                         string is_confirm_check, string apply_overtime_dt_s, string apply_overtime_dt_e)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.EMP_ID,APPLY_OVERTIME_DT ASC ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,c.DEPT_NAME,REPLACE(CONVERT(char(10), APPLY_OVERTIME_DT, 120),'-','/') APPLY_OVERTIME_DT,OVERTIME_DESC,OVERTIME_TIME_CD, ");
            sb.Append(" a.OVERTIME_DT_TYPE,APPLY_OVERTIME_HOUR,");
            sb.Append(" IFLOW_NO,IS_CONFIRM_CHECK, concat(DATEPART(YY,IFLOW_APPROVE_DT),'/',DATEPART(MM,IFLOW_APPROVE_DT))as IFLOW_APPROVE_DT,CHECK_STATUS");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a,VW_H_EMP_DATA b, VW_H_DEPT_DATA c ,TB_D_M_OVERTIME_TYPE d");
            sb.Append(" where a.EMP_ID = b.EMP_ID ");
            sb.Append(" and a.DEPT_NO = c.DEPT_NO ");
            sb.Append(" and a.OVERTIME_CD=d.OVERTIME_CD ");
            sb.Append(" and a.FORM_STATUS <>'N'");
            sb.Append(" and a.FORM_STATUS <>'D'");

            if (apply_overtime_dt_s != "")
            {
                sb.Append(" and APPLY_OVERTIME_DT >= @APPLY_OVERTIME_DT_S ");
                ht.Add("@APPLY_OVERTIME_DT_S", apply_overtime_dt_s);
            }
            if (apply_overtime_dt_e != "")
            {
                sb.Append(" and APPLY_OVERTIME_DT <= @APPLY_OVERTIME_DT_E ");
                ht.Add("@APPLY_OVERTIME_DT_E", apply_overtime_dt_e);
            }
            if (is_confirm_check != "")
            {
                sb.Append("and IS_CONFIRM_CHECK = @IS_CONFIRM_CHECK");
                ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);
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
    public int getConfirmCount(int startRowIndex, int maximumRows,
                             string is_confirm_check, string apply_overtime_dt_s, string apply_overtime_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a,VW_H_EMP_DATA b, VW_H_DEPT_DATA c,TB_D_M_OVERTIME_TYPE d ");
            sb.Append(" where a.EMP_ID = b.EMP_ID ");
            sb.Append(" and a.DEPT_NO = c.DEPT_NO ");
            sb.Append(" and a.OVERTIME_CD=d.OVERTIME_CD ");
            sb.Append(" and a.FORM_STATUS <>'N'");
            sb.Append(" and a.FORM_STATUS <>'D'");

            if (apply_overtime_dt_s != "")
            {
                sb.Append(" and APPLY_OVERTIME_DT >= @APPLY_OVERTIME_DT_S ");
                ht.Add("@APPLY_OVERTIME_DT_S", apply_overtime_dt_s);
            }
            if (apply_overtime_dt_e != "")
            {
                sb.Append(" and APPLY_OVERTIME_DT <= @APPLY_OVERTIME_DT_E ");
                ht.Add("@APPLY_OVERTIME_DT_E", apply_overtime_dt_e);
            }
            if (is_confirm_check != "")
            {
                sb.Append("and IS_CONFIRM_CHECK = @IS_CONFIRM_CHECK");
                ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);
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

    internal DataTable getOvertimeCD(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select OVERTIME_CD,CONCAT(OVERTIME_CD, '-'+OVERTIME_DESC)AS OVERTIME_DESC from TB_D_M_OVERTIME_TYPE ");
            sb.Append("where IS_USED='Y'");

            ht.Add("@OVERTIME_CD", p);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeTimeCD(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select OVERTIME_TIME_CD from TB_D_M_OVERTIME_APPLY");
            ht.Add("@OVERTIME_TIME_CD", p);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCheckStatus(string p)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select CHECK_STATUS from TB_D_M_OVERTIME_APPLY");
        ht.Add("@CHECK_STATUS", p);
        return dbConn.Query(sb, ht);
    }




    internal void deleteEmpID(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_APPLY set FORM_STATUS='D',IS_CONFIRM_CHECK='N',CHECK_STATUS='N',");
            sb.Append("UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and IFLOW_NO = @IFLOW_NO ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_APPLY (EMP_ID,DEPT_NO,OVERTIME_CD,OVERTIME_DT_TYPE,OVERTIME_TIME_CD,CALENDAR_DT,SHIFT_CD,");
            sb.Append("APPLY_OVERTIME_DT,REPLACE_DT,OVERTIME_REASON,");
            sb.Append("BEFORE_STIME,BEFORE_ETIME,BEFORE_HOUR,AFTER_STIME,AFTER_ETIME,AFTER_HOUR,APPLY_OVERTIME_HOUR,");
            sb.Append("APPROVE_OVERTIME_HOUR,IS_APPLY,EXCHANGE_HOUR,CLOCK_IN_TIME,CLOCK_OUT_TIME,IS_CONFIRM_CHECK,CHECK_STATUS,");
            sb.Append("IFLOW_APPROVE_DT,IFLOW_NO,FORM_STATUS,REMARK,");
            sb.Append("IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS,PAY_DT,WORK_CD,OVERTIME_CTL_CD,WS_CD,PJOB_CD,");
            sb.Append("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" values (@EMP_ID,@DEPT_NO,@OVERTIME_CD,LEFT(@OVERTIME_DT_TYPE,1),LEFT(@OVERTIME_TIME_CD,1),@CALENDAR_DT,@SHIFT_CD,");
            sb.Append("@APPLY_OVERTIME_DT,@REPLACE_DT,@OVERTIME_REASON,");
            sb.Append("@BEFORE_STIME,@BEFORE_ETIME,@BEFORE_HOUR,@AFTER_STIME,@AFTER_ETIME,@AFTER_HOUR,@APPLY_OVERTIME_HOUR,");
            sb.Append("@APPROVE_OVERTIME_HOUR,LEFT(@IS_APPLY,1),@EXCHANGE_HOUR,@CLOCK_IN_TIME,@CLOCK_OUT_TIME,@IS_CONFIRM_CHECK,LEFT(@CHECK_STATUS,1),");
            sb.Append("@IFLOW_APPROVE_DT,");
            sb.Append(" 'HRO' + replace(CONVERT(CHAR(10), @APPLY_OVERTIME_DT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_OVERTIME_APPLY where replace(convert(varchar(10),APPLY_OVERTIME_DT,120),'-','/') = convert(varchar(10),@APPLY_OVERTIME_DT,120)and IFLOW_NO like 'HRO%'),'00001')  , ");
            sb.Append("@FORM_STATUS,@REMARK,");
            sb.Append("@IS_CONFIRM_CLOSE,@SALARY_SETTLE_STATUS,@PAY_DT,@WORK_CD,@OVERTIME_CTL_CD,@WS_CD,@PJOB_CD,");
            sb.Append("@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);

            DataTable dt = getEmp(EMP_ID);
            ht.Add("@WORK_CD", dt.Rows[0]["WORK_CD"].ToString());
            ht.Add("@OVERTIME_CTL_CD", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
            ht.Add("@WS_CD", dt.Rows[0]["WS_CD"].ToString());
            ht.Add("@PJOB_CD", dt.Rows[0]["PJOB_CD"].ToString());

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            if (REPLACE_DT == "")
                ht.Add("@REPLACE_DT", DBNull.Value);
            else
                ht.Add("@REPLACE_DT", REPLACE_DT);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME == "")
                ht.Add("@BEFORE_STIME", DBNull.Value);
            else
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            if (BEFORE_ETIME == "")
                ht.Add("@BEFORE_ETIME", DBNull.Value);
            else
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);
            if (AFTER_STIME == "")
                ht.Add("@AFTER_STIME", DBNull.Value);
            else
                ht.Add("@AFTER_STIME", AFTER_STIME);
            if (AFTER_ETIME == "")
                ht.Add("@AFTER_ETIME", DBNull.Value);
            else
                ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);
            if (CLOCK_IN_TIME == "")
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            if (CLOCK_OUT_TIME == "")
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            //ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@REMARK", REMARK);

            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            if (PAY_DT == "")
                ht.Add("@PAY_DT", DBNull.Value);
            else
                ht.Add("@PAY_DT", PAY_DT);
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

    private DataTable getEmp(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getOvertimeDtType(string overtime_cd)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(@"Select OVERTIME_DT_TYPE,OVERTIME_EXCHANGE_CD,OVERTIME_ALLOW_CD,CHG_WORK_CD,WORK_DAY_CD from TB_D_M_OVERTIME_TYPE ");
        sb.Append("where OVERTIME_CD = @OVERTIME_CD ");

        ht.Add("@OVERTIME_CD", overtime_cd);
        return dbConn.Query(sb, ht);
    }

    

    internal DataTable getIFlowNO(string emp_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("select replace(CONVERT(char(10),getdate(),120),'-','') + REPLACE(STR(MAX(substring(iflow_no,14,5)) + 1, 5), SPACE(1), '0') as IFLOW_NO  from TB_D_M_OVERTIME_APPLY");
        return dbConn.Query(sb, ht);
    }




    internal void updateEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_APPLY set OVERTIME_CD=LEFT(@OVERTIME_CD,1),OVERTIME_DT_TYPE=LEFT(@OVERTIME_DT_TYPE,1),OVERTIME_TIME_CD=@OVERTIME_TIME_CD,SHIFT_CD=@SHIFT_CD,");
            sb.Append("APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT,REPLACE_DT=@REPLACE_DT,OVERTIME_REASON=@OVERTIME_REASON,");
            sb.Append("BEFORE_STIME=@BEFORE_STIME,BEFORE_ETIME=@BEFORE_ETIME,BEFORE_HOUR=@BEFORE_HOUR,AFTER_STIME=@AFTER_STIME,AFTER_ETIME=@AFTER_ETIME,AFTER_HOUR=@AFTER_HOUR,APPLY_OVERTIME_HOUR=@APPLY_OVERTIME_HOUR,");
            sb.Append("APPROVE_OVERTIME_HOUR=@APPROVE_OVERTIME_HOUR,IS_APPLY=LEFT(@IS_APPLY,1),EXCHANGE_HOUR=@EXCHANGE_HOUR,CLOCK_IN_TIME=@CLOCK_IN_TIME,CLOCK_OUT_TIME=@CLOCK_OUT_TIME,IS_CONFIRM_CHECK=@IS_CONFIRM_CHECK,CHECK_STATUS=LEFT(@CHECK_STATUS,1),");
            sb.Append("IFLOW_APPROVE_DT=@IFLOW_APPROVE_DT,FORM_STATUS=@FORM_STATUS,REMARK=@REMARK,");
            sb.Append("WORK_CD=@WORK_CD,OVERTIME_CTL_CD=@OVERTIME_CTL_CD,WS_CD=@WS_CD,PJOB_CD=@PJOB_CD,");
            sb.Append("UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO = @IFLOW_NO");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IFLOW_NO", IFLOW_NO);

            DataTable dt = getEmp(EMP_ID);
            ht.Add("@WORK_CD", dt.Rows[0]["WORK_CD"].ToString());
            ht.Add("@OVERTIME_CTL_CD", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
            ht.Add("@WS_CD", dt.Rows[0]["WS_CD"].ToString());
            ht.Add("@PJOB_CD", dt.Rows[0]["PJOB_CD"].ToString());

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            if (REPLACE_DT == "")
                ht.Add("@REPLACE_DT", DBNull.Value);
            else
                ht.Add("@REPLACE_DT", REPLACE_DT);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME == "")
                ht.Add("@BEFORE_STIME", DBNull.Value);
            else
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            if (BEFORE_ETIME == "")
                ht.Add("@BEFORE_ETIME", DBNull.Value);
            else
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);

            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);
            if (AFTER_STIME == "")
                ht.Add("@AFTER_STIME", DBNull.Value);
            else
                ht.Add("@AFTER_STIME", AFTER_STIME);
            if (AFTER_ETIME == "")
                ht.Add("@AFTER_ETIME", DBNull.Value);
            else
                ht.Add("@AFTER_ETIME", AFTER_ETIME);

            ht.Add("@AFTER_HOUR", AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);
            if (CLOCK_IN_TIME == "")
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            if (CLOCK_OUT_TIME == "")
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            ht.Add("@FORM_STATUS", FORM_STATUS);
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

    internal DataTable getDtlData(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select a.EMP_ID,b.EMP_NAME,b.DEPT_NO +'-'+ b.DEPT_NAME DEPT_NO,OVERTIME_CD,OVERTIME_DT_TYPE,LEFT(b.OVERTIME_CTL_CD,1)OVERTIME_CTL_CD,OVERTIME_TIME_CD,a.SHIFT_CD,c.SHIFT_CD + '-' + c.SHIFT_DESC SHIFT_DESC,CONVERT(char(10),APPLY_OVERTIME_DT,111)APPLY_OVERTIME_DT,CONVERT(char(10),REPLACE_DT,111)REPLACE_DT,OVERTIME_REASON, ");
            sb.Append("LEFT(CONVERT(VARchar(12), BEFORE_STIME, 108),2) AS BSH,RIGHT(LEFT(CONVERT(VARchar(12), BEFORE_STIME, 108),5),2) AS BSM, ");
            sb.Append("LEFT(CONVERT(VARchar(12), BEFORE_ETIME, 108),2) AS BEH,RIGHT(LEFT(CONVERT(VARchar(12), BEFORE_ETIME, 108),5),2) AS BEM, ");
            sb.Append("cast(ROUND(BEFORE_HOUR/60,0)AS DECIMAL(5,0) ) AS BH,cast(ROUND(BEFORE_HOUR%60,0)AS DECIMAL(5,0) ) AS BM, ");
            sb.Append("LEFT(CONVERT(VARchar(12), AFTER_STIME, 108),2) AS ASH,RIGHT(LEFT(CONVERT(VARchar(12), AFTER_STIME, 108),5),2) AS ASM, ");
            sb.Append("LEFT(CONVERT(VARchar(12), AFTER_ETIME, 108),2) AS AEH,RIGHT(LEFT(CONVERT(VARchar(12), AFTER_ETIME, 108),5),2) AS AEM, ");
            sb.Append("cast(ROUND(AFTER_HOUR/60,0,1)AS DECIMAL(5,0) ) AS AH,cast(ROUND(AFTER_HOUR%60,0)AS DECIMAL(5,0) ) AS AM, ");
            sb.Append("cast(ROUND(APPLY_OVERTIME_HOUR/60,0,1)AS DECIMAL(5,0) ) AS AOHH,cast(ROUND(APPLY_OVERTIME_HOUR%60,0)AS DECIMAL(5,0) ) AS AOHM , ");
            sb.Append("cast(ROUND(APPROVE_OVERTIME_HOUR/60,0,1)AS DECIMAL(5,0) ) AS APOHH,cast(ROUND(APPROVE_OVERTIME_HOUR%60,0)AS DECIMAL(5,0) ) AS APOHM, ");
            sb.Append("IS_APPLY,cast(ROUND(EXCHANGE_HOUR/60,0,1)AS DECIMAL(5,0) ) AS EXHH,cast(ROUND(EXCHANGE_HOUR%60,0)AS DECIMAL(5,0) ) AS EXHM, ");
            sb.Append(" CONVERT(char(5),CLOCK_IN_TIME,108)CLOCK_IN_TIME,CONVERT(char(5),CLOCK_OUT_TIME,108)CLOCK_OUT_TIME, ");
            sb.Append("IS_CONFIRM_CHECK,CHECK_STATUS,CONVERT(char(10),IFLOW_APPROVE_DT,111)IFLOW_APPROVE_DT,IFLOW_NO,FORM_STATUS,a.REMARK ");
            sb.Append(",EXCHANGE_HOUR ");
            sb.Append("from (select * from   TB_D_M_OVERTIME_APPLY a with (nolock) where a.EMP_ID=@EMP_ID  and IFLOW_NO=@IFLOW_NO ) a ");
            sb.Append("left join VW_H_EMP_DATA b with (nolock) on a.EMP_ID=b.EMP_ID ");
            sb.Append("left join TB_D_M_SHIFT_H c with (nolock)  on a.SHIFT_CD=c.SHIFT_CD  and c.START_DT <= A.CALENDAR_DT and c.END_DT >= A.CALENDAR_DT ");


            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSalaryStatus(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select SALARY_SETTLE_STATUS, PAY_DT from TB_D_M_OVERTIME_APPLY where EMP_ID=@EMP_ID and IFLOW_NO=@IFLOW_NO and SALARY_SETTLE_STATUS='Y' and PAY_DT<>'' ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getShiftCD(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.DT_TYPE, a.DT_TYPE+'-'+b.SUB_DESC DT_TYPE_DESC, a.SHIFT_CD, a.SHIFT_CD+'-'+c.SHIFT_DESC SHIFT_DESC from TB_D_M_EMP_DAY_DUTY a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DA' and b.MAIN_CD='DT_TYPE' and a.DT_TYPE=b.SUB_CD ");
            sb.Append(" left join VW_D_M_SHIFT_H c on a.SHIFT_CD=c.SHIFT_CD ");
            sb.Append(" where a.EMP_ID=@EMP_ID ");
            sb.Append(" and a.CALENDAR_DT=@APPLY_OVERTIME_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal int chk_IS_APPLY(string empid, string overtime)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @WORK_CD varchar(1)
                         select @WORK_CD = WORK_CD from TB_H_M_EMP where EMP_ID = @EMP_ID

                        if exists (
                           select 1 from TB_D_M_OVERTIME_TYPE 
                            where OVERTIME_CD = @OVERTIME_CD
                            and charindex(@WORK_CD ,CHG_WORK_CD ) > 0
                        ) 
                            select 1 resutCount
                        else 
                            select 0 resutCount
                        ");
            ht.Add("@EMP_ID", empid);
            ht.Add("@OVERTIME_CD", overtime);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resutCount"];
            }

            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeCtlCD(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEFT(OVERTIME_CTL_CD,1)OVERTIME_CTL_CD");
            sb.Append(" from VW_H_EMP_DATA  ");
            sb.Append(" where EMP_ID=@EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getClockTime(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.DEPT_NO,a.OVERTIME_CTL_CD+'-'+b.SUB_DESC OVERTIME_CTL_DESC ");
            sb.Append(" ,CONVERT(char(10),a.CLOCK_IN_DT,111)+' '+CONVERT(char(5),a.CLOCK_IN_DT,108) CLOCK_IN_DT ");
            sb.Append(" ,CONVERT(char(10),a.CLOCK_OUT_DT,111)+' '+CONVERT(char(5),a.CLOCK_OUT_DT,108) CLOCK_OUT_DT ");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS a  ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='OVERTIME_CTL_CD' and a.OVERTIME_CTL_CD=b.SUB_CD ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and CALENDAR_DT=@APPLY_OVERTIME_DT ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select COUNT(0) empcount from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=BEFORE_ETIME and @BEFORE_ETIME<=BEFORE_STIME) ");
            sb.Append(" or (@BEFORE_STIME<=AFTER_ETIME and @BEFORE_ETIME<=AFTER_STIME) ");
            sb.Append(" or (@AFTER_STIME<=BEFORE_ETIME and @AFTER_ETIME<=BEFORE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=AFTER_ETIME and @AFTER_ETIME<=AFTER_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N'");
            sb.Append(" and FORM_STATUS <>'D'");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOverTimeData(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select *,(isnull(c.ACTUAL_WEEKDAYS_OVERTIME,0)-isnull(e.ACTUAL_EXCHANGED,0))+(isnull(d.ACTUAL_HOLIDAY_OVERTIME,0)-isnull(f.ACTUAL_APPLIED,0))as ACTUAL_OVERTIME_MANAGE ");
            sb.Append("from (select distinct a.emp_id,a.EMP_NAME,b.DEPT_NO,b.DEPT_NAME,concat(DATEPART(YY,APPLY_OVERTIME_DT),'/',DATEPART(MM,APPLY_OVERTIME_DT))as APPLY_OVERTIME_DT from VW_H_EMP_DATA a,VW_H_DEPT_DATA b,TB_D_M_OVERTIME_APPLY c where a.EMP_ID=@EMP_ID and  b.DEPT_NO=a.DEPT_NO and DATEPART(YY,APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) ) A left join ");
            sb.Append("(select EMP_ID,sum(APPROVE_OVERTIME_HOUR)as OVERTIME_GRAND_TOTAL from TB_D_M_OVERTIME_APPLY where CHECK_STATUS='Y'and EMP_ID=@EMP_ID and DATEPART(YY,APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) and FORM_STATUS<>'N' and FORM_STATUS<>'D'  group by EMP_ID) B on A.EMP_ID = B.EMP_ID left join ");
            sb.Append("(select EMP_ID,sum(APPROVE_OVERTIME_HOUR) as ACTUAL_WEEKDAYS_OVERTIME from TB_D_M_OVERTIME_APPLY where CHECK_STATUS='Y'and EMP_ID=@EMP_ID and DATEPART(YY,APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) and OVERTIME_DT_TYPE='1' and FORM_STATUS<>'N' and FORM_STATUS<>'D'  group by EMP_ID)C on A.EMP_ID = C.EMP_ID left join  ");
            sb.Append("(select EMP_ID,sum(APPROVE_OVERTIME_HOUR) as ACTUAL_HOLIDAY_OVERTIME from TB_D_M_OVERTIME_APPLY where CHECK_STATUS='Y'and EMP_ID=@EMP_ID and DATEPART(YY,APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) and OVERTIME_DT_TYPE='2' and FORM_STATUS<>'N' and FORM_STATUS<>'D'  group by EMP_ID)D on A.EMP_ID = D.EMP_ID left join ");
            sb.Append("(select ov.EMP_ID,sum(EXCHANGE_HOUR) as ACTUAL_EXCHANGED from TB_D_M_OVERTIME_APPLY ov,TB_D_M_LEAVE_APPLY_DAY le where le.CHECK_STATUS='Y'and ov.EMP_ID=@EMP_ID and ov.EMP_ID=le.EMP_ID and DATEPART(YY,le.APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,le.APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) and MAIN_LEAVE_CD='Z' and le.FORM_STATUS<>'N' and le.FORM_STATUS<>'D'  group by ov.EMP_ID)E on A.EMP_ID = E.EMP_ID left join ");
            sb.Append("(select ov.EMP_ID,sum(EXCHANGE_HOUR) as ACTUAL_APPLIED from TB_D_M_OVERTIME_APPLY ov,TB_D_M_LEAVE_APPLY_DAY le where le.CHECK_STATUS='Y'and ov.EMP_ID=@EMP_ID and ov.EMP_ID=le.EMP_ID and DATEPART(YY,le.APPLY_OVERTIME_DT)=DATEPART(YY,@APPLY_OVERTIME_DT) and DATEPART(MM,le.APPLY_OVERTIME_DT)=DATEPART(MM,@APPLY_OVERTIME_DT) and MAIN_LEAVE_CD='X' and le.FORM_STATUS<>'N' and le.FORM_STATUS<>'D'  group by ov.EMP_ID)F on A.EMP_ID = F.EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getControlCD(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 ");
            sb.Append("from TB_D_M_OVERTIME_APPLY a,TB_9_M_COMM_D b ");
            sb.Append("where a.EMP_ID=@EMP_ID ");
            sb.Append("and SYS_CD='HB' ");
            sb.Append("and MAIN_CD='OVERTIME_CTL_CD' ");
            sb.Append("and a.OVERTIME_CTL_CD=b.SUB_CD ");

            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }


    internal void Confirm_empid(string emp_id, string iflow_no, string is_confirm_check)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_APPLY set IS_CONFIRM_CHECK=Case when @IS_CONFIRM_CHECK='Y' then 'N' when @IS_CONFIRM_CHECK='N' then 'Y' END,");
            sb.Append("CHECK_STATUS=Case when @IS_CONFIRM_CHECK='Y' then 'N' when @IS_CONFIRM_CHECK='N' then 'Y' END,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID ");
            sb.Append(" and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getDupData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select a.EMP_ID,EMP_NAME from ");
            sb.Append("(select EMP_ID from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=BEFORE_ETIME and @BEFORE_ETIME<=BEFORE_STIME) ");
            sb.Append(" or (@BEFORE_STIME<=AFTER_ETIME and @BEFORE_ETIME<=AFTER_STIME) ");
            sb.Append(" or (@AFTER_STIME<=BEFORE_ETIME and @AFTER_ETIME<=BEFORE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=AFTER_ETIME and @AFTER_ETIME<=AFTER_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N'");
            sb.Append(" and FORM_STATUS <>'D')A left join");
            sb.Append("(select EMP_NAME,EMP_ID from VW_H_EMP_DATA) B on A.EMP_ID=B.EMP_ID");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addBatch()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_APPLY (EMP_ID,DEPT_NO,OVERTIME_CD,OVERTIME_DT_TYPE,OVERTIME_TIME_CD,CALENDAR_DT,SHIFT_CD,");
            sb.Append("APPLY_OVERTIME_DT,REPLACE_DT,OVERTIME_REASON,");
            sb.Append("BEFORE_STIME,BEFORE_ETIME,BEFORE_HOUR,AFTER_STIME,AFTER_ETIME,AFTER_HOUR,APPLY_OVERTIME_HOUR,");
            sb.Append("APPROVE_OVERTIME_HOUR,IS_APPLY,EXCHANGE_HOUR,CLOCK_IN_TIME,CLOCK_OUT_TIME,IS_CONFIRM_CHECK,CHECK_STATUS,");
            sb.Append("IFLOW_APPROVE_DT,IFLOW_NO,FORM_STATUS,REMARK,");
            sb.Append("IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS,PAY_DT,WORK_CD,OVERTIME_CTL_CD,WS_CD,PJOB_CD,");
            sb.Append("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" values (@EMP_ID,@DEPT_NO,LEFT(@OVERTIME_CD,1),LEFT(@OVERTIME_DT_TYPE,1),@OVERTIME_TIME_CD,@CALENDAR_DT,@SHIFT_CD,");
            sb.Append("@APPLY_OVERTIME_DT,@REPLACE_DT,@OVERTIME_REASON,");
            sb.Append("@BEFORE_STIME,@BEFORE_ETIME,@BEFORE_HOUR,@AFTER_STIME,@AFTER_ETIME,@AFTER_HOUR,@APPLY_OVERTIME_HOUR,");
            sb.Append("@APPROVE_OVERTIME_HOUR,LEFT(@IS_APPLY,1),@EXCHANGE_HOUR,@CLOCK_IN_TIME,@CLOCK_OUT_TIME,@IS_CONFIRM_CHECK,LEFT(@CHECK_STATUS,1),@IFLOW_APPROVE_DT,");
            sb.Append(" 'HRO' + replace(CONVERT(CHAR(10), @APPLY_OVERTIME_DT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_OVERTIME_APPLY where replace(convert(varchar(10),APPLY_OVERTIME_DT,120),'-','/') = convert(varchar(10),@APPLY_OVERTIME_DT,120)and IFLOW_NO like 'HRO%'),'00001')  , ");
            sb.Append("@FORM_STATUS,@REMARK,");
            sb.Append("@IS_CONFIRM_CLOSE,@SALARY_SETTLE_STATUS,@PAY_DT,@WORK_CD,@OVERTIME_CTL_CD,@WS_CD,@PJOB_CD,");
            sb.Append("@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);

            DataTable dt = getEmp(EMP_ID);
            ht.Add("@WORK_CD", dt.Rows[0]["WORK_CD"].ToString());
            ht.Add("@OVERTIME_CTL_CD", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
            ht.Add("@WS_CD", dt.Rows[0]["WS_CD"].ToString());
            ht.Add("@PJOB_CD", dt.Rows[0]["PJOB_CD"].ToString());

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            if (REPLACE_DT == "")
                ht.Add("@REPLACE_DT", DBNull.Value);
            else
                ht.Add("@REPLACE_DT", REPLACE_DT);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME == "")
                ht.Add("@BEFORE_STIME", DBNull.Value);
            else
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            if (BEFORE_ETIME == "")
                ht.Add("@BEFORE_ETIME", DBNull.Value);
            else
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);

            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);

            if (AFTER_STIME == "")
                ht.Add("@AFTER_STIME", DBNull.Value);
            else
                ht.Add("@AFTER_STIME", AFTER_STIME);
            if (AFTER_ETIME == "")
                ht.Add("@AFTER_ETIME", DBNull.Value);
            else
                ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);
            if (CLOCK_IN_TIME == "")
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            if (CLOCK_OUT_TIME == "")
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            //ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@REMARK", REMARK);

            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            if (PAY_DT == "")
                ht.Add("@PAY_DT", DBNull.Value);
            else
                ht.Add("@PAY_DT", PAY_DT);
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


    internal DataTable getLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select a.EMP_ID,EMP_NAME from  ");
            sb.Append("(select EMP_ID from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_LEAVE_SDT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=APPLY_LEAVE_ETIME and @BEFORE_ETIME>=APPLY_LEAVE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=APPLY_LEAVE_ETIME and @AFTER_ETIME>=APPLY_LEAVE_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N'");
            sb.Append(" and FORM_STATUS <>'D')A left join");
            sb.Append("(select EMP_NAME,EMP_ID from VW_H_EMP_DATA) B on A.EMP_ID=B.EMP_ID");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal bool getLeaveGI(string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(1) total  ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_LEAVE_SDT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=APPLY_LEAVE_ETIME and @BEFORE_ETIME>=APPLY_LEAVE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=APPLY_LEAVE_ETIME and @AFTER_ETIME>=APPLY_LEAVE_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N' and FORM_STATUS <>'D' ");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getBatchLeaveData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select COUNT(0) empcount2 from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_LEAVE_SDT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=APPLY_LEAVE_ETIME and @BEFORE_ETIME>=APPLY_LEAVE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=APPLY_LEAVE_ETIME and @AFTER_ETIME>=APPLY_LEAVE_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N'");
            sb.Append(" and FORM_STATUS <>'D'");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_REOPEN");
            ht.Add("@p_EMP_ID", emp_id);
            ht.Add("@p_CALENDAR_SDT", apply_overtime_dt);
            ht.Add("@p_CALENDAR_EDT", apply_overtime_dt);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DI050");


            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getWorkDayCd(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select WORK_DAY_CD from TB_D_M_EMP_DAY_DUTY where replace(convert(char(10),CALENDAR_DT,120),'-','/') = @APPLY_OVERTIME_DT and EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string OVERTIME_ALLOW_CD { get; set; }

    internal DataTable getOVERTIME_ALLOW_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD from TB_D_M_OVERTIME_ALLOW a inner join VW_H_EMP_DATA b on a.WS_CD = b.WS_CD and a.PJOB_CD = b.PJOB_CD and a.WORK_CD = b.WORK_CD");
            sb.Append(" where a.OVERTIME_CD = @OVERTIME_CD and a.OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE and b.EMP_ID = @EMP_ID");
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE.Substring(0, 1));
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //檢查 HR 加班申請是否重複
    internal DataTable getdupApplyData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(0) datacount FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE EMP_ID=@EMP_ID and replace(convert(char(10),APPLY_OVERTIME_DT,120),'-','/') = @APPLY_OVERTIME_DT ");
            sb.Append(" AND(  ( @BEFORE_STIME <= BEFORE_ETIME and @BEFORE_ETIME >= BEFORE_STIME ) ");
            sb.Append("  OR ( @BEFORE_STIME <= AFTER_ETIME and @BEFORE_ETIME >= AFTER_STIME ) ");
            sb.Append("  OR ( @AFTER_STIME <= BEFORE_ETIME and @AFTER_ETIME >= BEFORE_STIME ) ");
            sb.Append("  OR ( @AFTER_STIME <= AFTER_ETIME and @AFTER_ETIME >= AFTER_STIME )  ) ");
            sb.Append("  AND FORM_STATUS not in ('N', 'D') ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //檢查 HR 請假申請是否重複
    internal DataTable getdupLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(*) datacount FROM TB_D_M_LEAVE_APPLY_DAY");
            sb.Append(" WHERE EMP_ID=@EMP_ID AND ( replace(convert(char(10),APPLY_LEAVE_SDT,120),'-','/') = @APPLY_OVERTIME_DT)");
            sb.Append(" AND (  ( @BEFORE_STIME <= APPLY_LEAVE_ETIME  and  @BEFORE_ETIME >= APPLY_LEAVE_STIME )");
            sb.Append(" OR ( @AFTER_STIME <= APPLY_LEAVE_ETIME  and  @AFTER_ETIME >= APPLY_LEAVE_STIME )  )");
            sb.Append(" AND FORM_STATUS not in ('N', 'D')");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢查 IFLOW 加班申請是否重複
    internal DataTable check_APPLY_IFLOW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT COUNT(*) datacount from [" + utilities.IFLOWName + "].[IFLOW2].[dbo].[VW_D_M_OVERTIME_FLOW] ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            sb.Append("  AND convert(char(10),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append(" AND(  ( @BEFORE_STIME <= BEFORE_EDT and @BEFORE_ETIME >= BEFORE_SDT ) ");
            sb.Append("  OR ( @BEFORE_STIME <= AFTER_EDT and @BEFORE_ETIME >= AFTER_SDT ) ");
            sb.Append("  OR ( @AFTER_STIME <= BEFORE_EDT and @AFTER_ETIME >= BEFORE_SDT ) ");
            sb.Append("  OR ( @AFTER_STIME <= AFTER_EDT and @AFTER_ETIME >= AFTER_SDT )  ) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
  
    internal DataTable getCTLHour()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select isnull(sum(APPROVE_OVERTIME_HOUR),0) ctlsum FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE EMP_ID=@EMP_ID and replace(convert(char(7),APPLY_OVERTIME_DT,120),'-','/') = @APPLY_OVERTIME_DT ");
            sb.Append("  AND FORM_STATUS not in ('N', 'D') ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT.Substring(0, 7));

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getCTLHourType1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select (s.ctlsum_1 + s.ctlsum_2) ctlsum ");
            sb.Append(" from ( ");
            sb.Append(" select isnull(sum(a.APPROVE_OVERTIME_HOUR),0) ctlsum_1 ,");
            sb.Append("	( ");
            sb.Append("	 select isnull(sum(APPROVE_OVERTIME_HOUR),0) ctlsum_2  ");
            sb.Append("	  FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append("	 WHERE EMP_ID= @EMP_ID and convert(char(7),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append("	  AND FORM_STATUS not in ('N', 'D') ");
            sb.Append("	  AND OVERTIME_DT_TYPE ='1' ");
            sb.Append("	) ctlsum_2 ");
            sb.Append(" from ( ");
            sb.Append("	  select case when APPROVE_OVERTIME_HOUR >=480 then APPROVE_OVERTIME_HOUR-480 ");
            sb.Append("				  else 0 end as APPROVE_OVERTIME_HOUR  ");
            sb.Append("	   FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append("	 WHERE EMP_ID= @EMP_ID and convert(char(7),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append("	  AND FORM_STATUS not in ('N', 'D') ");
            sb.Append("	  AND OVERTIME_DT_TYPE ='2' ");
            sb.Append(" )a ");
            sb.Append(" )s ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT.Substring(0, 7));

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public string ORI_OVERTIME_APPLY_DT { get; set; }

    internal DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_NAME,DEPT_NO from VW_H_EMP_DATA ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDEPT_DATA(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NAME from VW_H_DEPT_DATA ");
            sb.Append(" WHERE DEPT_NO=@DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeCD(string emp_id, string apply_overtime_dt, string apply_overtime_s, string apply_overtime_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select * from TB_H_R_EMP_COURSE ");
            sb.Append(" WHERE EMP_ID = @EMP_ID ");
            sb.Append(" and COURSE_DT = @APPLY_OVERTIME_DT ");
            sb.Append(" and COURSE_STIME >= @APPLY_OVERTIME_S ");
            sb.Append(" and COURSE_ETIME <= @APPLY_OVERTIME_E ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@APPLY_OVERTIME_S", apply_overtime_s);
            ht.Add("@APPLY_OVERTIME_E", apply_overtime_e);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDutyData(string emp_id, string apply_overtime_dt)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DUTY_STIME,DUTY_ETIME from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" WHERE EMP_ID = @EMP_ID and CALENDAR_DT = @CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);

            return dbConn.Query(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSHIFT_CD(string shift_cd)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_D_M_SHIFT_H where SHIFT_CD is not null and END_DT > GETDATE()");

            if (shift_cd != "")
            {
                sb.Append(" and shift_cd = @shift_cd");
                ht.Add("@shift_cd", shift_cd);
            }

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkOVERTIME(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,DUTY_STIME,DUTY_ETIME ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            sb.Append(" and CONVERT(char(10), DUTY_STIME, 111) < CONVERT(char(10), DUTY_ETIME, 111) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable checkDUTY_ETIME(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and DUTY_ETIME <= @DUTY_ETIME ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@DUTY_ETIME", after_stime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkDUTY_STIME(string emp_id, string apply_overtime_dt, string before_etime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and DUTY_STIME >= @DUTY_STIME ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@DUTY_STIME", before_etime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkOVERTIME2(string apply_overtime_dt, string shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,DUTY_STIME,DUTY_ETIME ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where CALENDAR_DT=@CALENDAR_DT and SHIFT_CD=@SHIFT_CD ");
            sb.Append(" and CONVERT(char(10), DUTY_STIME, 111) < CONVERT(char(10), DUTY_ETIME, 111) ");
            ht.Add("@SHIFT_CD", shift_cd);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_CD(string overtime_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD+'-'+OVERTIME_DESC OVERTIME_DESC,OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            sb.Append(" where IS_USED='Y'  ");
            if (overtime_cd != "")
            {
                sb.Append(" and OVERTIME_CD=@overtime_cd");
                ht.Add("@overtime_cd", overtime_cd);
            }
            sb.Append(" order by IS_IFLOW_SHOW desc, OVERTIME_CD asc");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_RECORDS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_IN_DT,CLOCK_OUT_DT ");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEMP_ID(string plant_cd, string dept_no, string ws_cd, string work_cd, string work_shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS ='01' ");

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCHECK_STATUS(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_EMP_DUTY_CHECK_STATUS set DUTY_CHECK_RESULT = 'N' ");
            sb.Append(@" ,LATE_HOUR = 0
                        ,LEAVE_EARLY_HOUR = 0
                        ,LACK_HOUR = 0
                        ,DUTY_HOUR = 0
                        ,LEAVE_HOUR = 0
                        ,LEAVE_INFO = ''
                        ,OVERTIME_HOUR_APPLY = 0
                        ,OVERTIME_HOUR_APPROVE = 0
			            ,OVERTIME_PAY_HOUR = 0
                        ,VIOLATE_BEFORE_HOUR = 0
                        ,VIOLATE_AFTER_HOUR = 0
                        ,OVERTIME_INFO = ''
                        ,SHIFT_CD = ''
                        ,WORK_SHIFT_ALLOWANCE_TYPE = ''
                      ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT = @CALENDAR_DT ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCHECK_STATUS2(string emp_id, string iflow_no, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_EMP_DUTY_CHECK_STATUS set DUTY_CHECK_RESULT = 'N' ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT = ( ");
            sb.Append("   select top 1 REPLACE_DT from TB_D_M_OVERTIME_APPLY ");
            sb.Append("   where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append("   and IFLOW_NO=@IFLOW_NO ");
            sb.Append(" ) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal string SP_DI_OVERTIME_CHK()
    {
        try
        {
            //C:\AP_Source_New_svn\App_Code\tw\co\toyota\kuozui\comm\dao\DBConnector.cs
            //新增getSP_String
            //StringBuilder sb = new StringBuilder();
            //Hashtable ht = new Hashtable();
            //sb.Append("SP_DI_OVERTIME_CHK");
            //ht.Add("@p_EMP_ID", EMP_ID);                 //工號
            //ht.Add("@p_CALENDAR_DT", APPLY_OVERTIME_DT); //勤務日期(加班日期) yyyy/MM/dd
            //ht.Add("@p_OVERTIME_CD", OVERTIME_CD);       //加班類型
            //ht.Add("@p_IS_APPLY", IS_APPLY);             //是否申告換休
            //ht.Add("@p_BEFORE_STIME", BEFORE_STIME);     //勤前開始時間
            //ht.Add("@p_BEFORE_ETIME", BEFORE_ETIME);     //勤前結束時間
            //ht.Add("@p_AFTER_STIME", AFTER_STIME);       //勤後開始時間
            //ht.Add("@p_AFTER_ETIME", AFTER_ETIME);       //勤後結束時間
            //ht.Add("@p_REPLACE_DT", REPLACE_DT);        //代休假日期
            //ht.Add("@p_IS_SUPER", "N");                  //若是管理者(Y)，則有些邏輯不用判斷(待討論)
            //ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            //ht.Add("@p_FuncID", "FB2DI050");
            //string rtn_flag = dbConn.getSP_String(sb, ht, true, "@p_RTN_FLAG"); //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
            //string rtn_msg = dbConn.getSP_String(sb, ht, true, "@p_RTN_MSG");
            //return rtn_flag;

            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DI_OVERTIME_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", EMP_ID);                   //工號
                comm.Parameters.AddWithValue("@p_CALENDAR_DT", APPLY_OVERTIME_DT);   //勤務日期(加班日期) yyyy/MM/dd
                comm.Parameters.AddWithValue("@p_OVERTIME_CD", OVERTIME_CD);         //加班類型
                comm.Parameters.AddWithValue("@p_IS_APPLY", IS_APPLY);               //是否申告換休
                comm.Parameters.AddWithValue("@p_BEFORE_STIME", BEFORE_STIME);       //勤前開始時間
                comm.Parameters.AddWithValue("@p_BEFORE_ETIME", BEFORE_ETIME);       //勤前結束時間
                comm.Parameters.AddWithValue("@p_AFTER_STIME", AFTER_STIME);         //勤後開始時間
                comm.Parameters.AddWithValue("@p_AFTER_ETIME", AFTER_ETIME);         //勤後結束時間
                comm.Parameters.AddWithValue("@p_REPLACE_DT", REPLACE_DT);           //代休假日期
                //comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                if (IS_ADD == "Y")
                {
                    comm.Parameters.AddWithValue("@p_IFLOW_NO", "");
                }
                else
                {
                    comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                }
                comm.Parameters.AddWithValue("@p_IS_SUPER", "N");                    //若是管理者(Y)，則有些邏輯不用判斷(待討論)
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DI050");
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;  
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;              //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }
            this.RTN_Message = rtnMessage;
            this.RTN_Flag = rtnFlag;
            return rtnFlag + ";" + rtnMessage;

            //if (rtnFlag != "E")
            //    return rtnFlag;
            //else
            //    return rtnMessage;
            
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getHYPER_SHOUR()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CAST(HYPER_SHOUR as int) * 60 HYPER_SHOUR, CAST(NORMAL_SHOUR as int) * 60 NORMAL_SHOUR ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            //sb.Append(" where OVERTIME_CD=@OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE ");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD  ");
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void insertTB_D_M_OVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_OVERTIME_APPLY (  ");
            sb.Append(" EMP_ID, APPLY_OVERTIME_DT, IFLOW_NO, OVERTIME_CD, OVERTIME_DT_TYPE ");
            sb.Append(" , OVERTIME_TIME_CD, CALENDAR_DT, SHIFT_CD, DT_TYPE, IS_APPLY ");
            sb.Append(" , OVERTIME_REASON, BEFORE_STIME, BEFORE_ETIME, BEFORE_HOUR, AFTER_STIME ");
            sb.Append(" , AFTER_ETIME, AFTER_HOUR, TRIP_STIME, TRIP_ETIME, TRIP_HOUR ");
            sb.Append(" , APPROVE_BEFORE_HOUR, APPROVE_AFTER_HOUR, APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR, REPLACE_DT, HYPER_HOUR, NORMAL_HOUR, IFLOW_APPROVE_DT ");
            sb.Append(" , FORM_STATUS, IS_DUTY_CHECK, O_SPECIAL_CD, IS_CONFIRM_CHECK, CHECK_STATUS ");
            sb.Append(" , CLOCK_IN_TIME, CLOCK_OUT_TIME, COURSE_LOG, REMARK, IS_CONFIRM_CLOSE ");
            sb.Append(" , SALARY_SETTLE_STATUS, PAY_DT, CLOSED_BY, CLOSED_DT, DEPT_NO ");
            sb.Append(" , WORK_CD, OVERTIME_CTL_CD, TARGET_TYPE, WS_CD, PJOB_CD ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) values ( ");
            sb.Append(" @EMP_ID, @APPLY_OVERTIME_DT, @IFLOW_NO, @OVERTIME_CD, @OVERTIME_DT_TYPE ");
            sb.Append(" , @OVERTIME_TIME_CD, @CALENDAR_DT, @SHIFT_CD, @DT_TYPE, @IS_APPLY ");
            sb.Append(" , @OVERTIME_REASON, @BEFORE_STIME, @BEFORE_ETIME, @BEFORE_HOUR, @AFTER_STIME ");
            sb.Append(" , @AFTER_ETIME, @AFTER_HOUR, @TRIP_STIME, @TRIP_ETIME, @TRIP_HOUR ");
            sb.Append(" , @APPROVE_BEFORE_HOUR, @APPROVE_AFTER_HOUR, @APPLY_OVERTIME_HOUR, @APPROVE_OVERTIME_HOUR, @OVERTIME_PAY_HOUR ");
            sb.Append(" , @EXCHANGE_HOUR, @REPLACE_DT, @HYPER_HOUR, @NORMAL_HOUR, @IFLOW_APPROVE_DT ");
            sb.Append(" , @FORM_STATUS, @IS_DUTY_CHECK, @O_SPECIAL_CD, @IS_CONFIRM_CHECK, @CHECK_STATUS ");
            sb.Append(" , @CLOCK_IN_TIME, @CLOCK_OUT_TIME, @COURSE_LOG, @REMARK, @IS_CONFIRM_CLOSE ");
            sb.Append(" , @SALARY_SETTLE_STATUS, @PAY_DT, @CLOSED_BY, @CLOSED_DT, @DEPT_NO ");
            sb.Append(" , @WORK_CD, @OVERTIME_CTL_CD, @TARGET_TYPE, @WS_CD, @PJOB_CD ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ) ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@DT_TYPE", DT_TYPE);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);

            if (TRIP_STIME == "")
                ht.Add("@TRIP_STIME", DBNull.Value);
            else
                ht.Add("@TRIP_STIME", TRIP_STIME);
            if (TRIP_ETIME == "")
                ht.Add("@TRIP_ETIME", DBNull.Value);
            else
                ht.Add("@TRIP_ETIME", TRIP_ETIME);
            if (TRIP_HOUR == "")
                ht.Add("@TRIP_HOUR", DBNull.Value);
            else
                ht.Add("@TRIP_HOUR", TRIP_HOUR);
         
            ht.Add("@APPROVE_BEFORE_HOUR", APPROVE_BEFORE_HOUR);
            ht.Add("@APPROVE_AFTER_HOUR", APPROVE_AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@OVERTIME_PAY_HOUR", OVERTIME_PAY_HOUR);
            ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);

            if (REPLACE_DT == "")
                ht.Add("@REPLACE_DT", DBNull.Value);
            else
                ht.Add("@REPLACE_DT", REPLACE_DT);
            
            ht.Add("@HYPER_HOUR", HYPER_HOUR);
            ht.Add("@NORMAL_HOUR", NORMAL_HOUR);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
            ht.Add("@O_SPECIAL_CD", O_SPECIAL_CD);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);

            if (CLOCK_IN_TIME == "") 
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            if (CLOCK_OUT_TIME == "") 
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);

            ht.Add("@COURSE_LOG", COURSE_LOG);
            ht.Add("@REMARK", REMARK);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            ht.Add("@PAY_DT", DBNull.Value);
            ht.Add("@CLOSED_BY", CLOSED_BY);
            ht.Add("@CLOSED_DT", DBNull.Value);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
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

    internal DataTable getTB_H_M_EMP(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,WORK_CD,OVERTIME_CTL_CD,WS_CD,PJOB_CD ");
            sb.Append(" from TB_H_M_EMP ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getFN_D_GET_OVERTIME_APPLY_HOUR(string o_start_time, string o_end_time, string soruce_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT [dbo].[FN_D_GET_OVERTIME_APPLY_HOUR]( ");
            sb.Append(" @EMP_ID, @OVERTIME_DT, @SHIFT_CD, @O_START_TIME, @O_END_TIME, @SORUCE_CD ");
            sb.Append(" ) as OVERTIME_APPLY_HOUR ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@O_START_TIME", o_start_time);
            ht.Add("@O_END_TIME", o_end_time);
            ht.Add("@SORUCE_CD", soruce_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDefaultData(string emp_id, string apply_overtime_dt, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID, APPLY_OVERTIME_DT, IFLOW_NO, OVERTIME_CD, OVERTIME_DT_TYPE ");
            sb.Append(" , OVERTIME_TIME_CD, CALENDAR_DT, SHIFT_CD, DT_TYPE, IS_APPLY ");
            sb.Append(" , OVERTIME_REASON, BEFORE_STIME, BEFORE_ETIME, BEFORE_HOUR, AFTER_STIME ");
            sb.Append(" , AFTER_ETIME, AFTER_HOUR, TRIP_STIME, TRIP_ETIME, TRIP_HOUR ");
            sb.Append(" , APPROVE_BEFORE_HOUR, APPROVE_AFTER_HOUR, APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR, REPLACE_DT, HYPER_HOUR, NORMAL_HOUR, IFLOW_APPROVE_DT ");
            sb.Append(" , FORM_STATUS, IS_DUTY_CHECK, O_SPECIAL_CD, IS_CONFIRM_CHECK, CHECK_STATUS ");
            sb.Append(" , CLOCK_IN_TIME, CLOCK_OUT_TIME, COURSE_LOG, REMARK, IS_CONFIRM_CLOSE ");
            sb.Append(" , SALARY_SETTLE_STATUS, PAY_DT, CLOSED_BY, CLOSED_DT, DEPT_NO ");
            sb.Append(" , WORK_CD, OVERTIME_CTL_CD, TARGET_TYPE, WS_CD, PJOB_CD ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@IFLOW_NO", iflow_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateEMP_DUTY_CHECK_STATUS(string flagDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" set REMARK=@REMARK,DUTY_CHECK_RESULT = 'N', ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@APPLY_OVERTIME_DT ");

            ht.Add("@EMP_ID", EMP_ID);
            if (flagDT == "0")
            {
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            }
            else
            {
                ht.Add("@APPLY_OVERTIME_DT", REPLACE_DT);
            }
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

    public void updateOVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_OVERTIME_APPLY set ");
            sb.Append(" OVERTIME_CD=@OVERTIME_CD, OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE, OVERTIME_TIME_CD=@OVERTIME_TIME_CD, CALENDAR_DT=@CALENDAR_DT, SHIFT_CD=@SHIFT_CD ");
            sb.Append(" , DT_TYPE=@DT_TYPE, IS_APPLY=@IS_APPLY, OVERTIME_REASON=@OVERTIME_REASON, BEFORE_STIME=@BEFORE_STIME, BEFORE_ETIME=@BEFORE_ETIME ");
            sb.Append(" , BEFORE_HOUR=@BEFORE_HOUR, AFTER_STIME=@AFTER_STIME, AFTER_ETIME=@AFTER_ETIME, AFTER_HOUR=@AFTER_HOUR, TRIP_STIME=@TRIP_STIME ");
            sb.Append(" , TRIP_ETIME=@TRIP_ETIME, TRIP_HOUR=@TRIP_HOUR, APPLY_OVERTIME_HOUR=@APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR=@APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR=@OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR=@EXCHANGE_HOUR, REPLACE_DT=@REPLACE_DT, HYPER_HOUR=@HYPER_HOUR, NORMAL_HOUR=@NORMAL_HOUR, IFLOW_APPROVE_DT=@IFLOW_APPROVE_DT ");
            sb.Append(" , CHECK_STATUS=@CHECK_STATUS, O_SPECIAL_CD=@O_SPECIAL_CD, REMARK=@REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);

            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@DT_TYPE", DT_TYPE);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME != "")
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            else
                ht.Add("@BEFORE_STIME", DBNull.Value);

            if (BEFORE_ETIME != "")
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            else
                ht.Add("@BEFORE_ETIME", DBNull.Value);
            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);

            if (AFTER_STIME != "")
                ht.Add("@AFTER_STIME", AFTER_STIME);
            else
                ht.Add("@AFTER_STIME", DBNull.Value);

            if (AFTER_ETIME != "")
                ht.Add("@AFTER_ETIME", AFTER_ETIME);
            else
                ht.Add("@AFTER_ETIME", DBNull.Value);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);

            if (TRIP_STIME != "")
                ht.Add("@TRIP_STIME", TRIP_STIME);
            else
                ht.Add("@TRIP_STIME", DBNull.Value);

            if (TRIP_ETIME != "")
                ht.Add("@TRIP_ETIME", TRIP_ETIME);
            else
                ht.Add("@TRIP_ETIME", DBNull.Value);
            ht.Add("@TRIP_HOUR", TRIP_HOUR);

            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", "0");
            ht.Add("@OVERTIME_PAY_HOUR", "0");
            ht.Add("@EXCHANGE_HOUR", 0);
            if (REPLACE_DT != "")
                ht.Add("@REPLACE_DT", REPLACE_DT);
            else
                ht.Add("@REPLACE_DT", DBNull.Value);

            ht.Add("@HYPER_HOUR", HYPER_HOUR);
            ht.Add("@NORMAL_HOUR", NORMAL_HOUR);
            if (IFLOW_APPROVE_DT != "")
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            else
                ht.Add("@IFLOW_APPROVE_DT", DBNull.Value);

            ht.Add("@CHECK_STATUS", "N");
            ht.Add("@O_SPECIAL_CD", O_SPECIAL_CD);

            if (CLOCK_IN_TIME != "")
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            else
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            if (CLOCK_OUT_TIME != "")
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            else
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            if (PAY_DT != "")
                ht.Add("@PAY_DT", PAY_DT);
            else
                ht.Add("@PAY_DT", DBNull.Value);
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

    public DataTable getCalendarTime()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"
                select CALENDAR_DT, DUTY_STIME, DUTY_ETIME from TB_D_M_EMP_DAY_DUTY 
                where EMP_ID = @EMP_ID and Convert(VARCHAR(10), CALENDAR_DT, 111) = @CALENDAR_DT
            ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT); //'2017/01/01'

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //申請單號:'HR'+  SP_D_GET_FLOWNO('FB2DI050','回傳值 output ') 的回傳值
    public string getSP_D_GET_FLOWNO(string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //exec dbo.SP_MB010_01 '@FORM_ID', '@YM', '@USER_ID', '@RTN'
            sb.Append("SP_D_GET_FLOWNO");
            ht.Add("@p_FUNC_DT", apply_overtime_dt);
            ht.Add("@p_FuncID", "FB2DI050");
            //ht.Add("@r_FLOWNO", "@rtn");
          
            //有回傳值再使用
            return dbConn.getSP_String(sb, ht, true, "@r_FLOWNO"); 
        }
        catch (Exception)
        {
            throw;
        }
    }


    //假日換休駐銷檢查
    internal string SP_DI_OVERTIME_X0_CHK(Tuple<string, string, string, string> item)
    {
        try
        {

            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DI_OVERTIME_X0_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", item.Item1);                   //工號
                comm.Parameters.AddWithValue("@p_CALENDAR_DT", item.Item2);   //勤務日期(加班日期) yyyy/MM/dd
                comm.Parameters.AddWithValue("@p_IS_APPLY", item.Item3);   //是否申告,絕對是Y
                comm.Parameters.AddWithValue("@p_IS_SUPER", "N");   
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DI060");
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;              //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }

            this.RTN_Message = rtnMessage;
            this.RTN_Flag = rtnFlag;
            if (rtnFlag == "Y")
                return "";
            else
                return item.Item1 + " " + item.Item2 + " " + rtnMessage;
        }
        catch (Exception)
        {
            throw;
        }
    }



   
}