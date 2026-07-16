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
/// CFB2SC2200DAO 的摘要描述
/// </summary>
public class CFB2SC2200DAO : BaseDAO
{
    public CFB2SC2200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    //public string QDATAKEY { get; set; }
    public string KIND_CD { get; set; }
    public string GROUP_ID { get; set; }
    public string GROUP_NAME { get; set; }
    public string CLASSIFY { get; set; }
    public string ORDER_SEQ { get; set; }
    public string GROUP_TYPE { get; set; }
    public string FUNC_ID { get; set; }

    //for查詢欄位
    public string ddl_SYS_CD { get; set; }

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
    public DataTable getSALARY_NAME(string salary_id)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_NAME ");
            sb.Append(" from TB_S_M_SALARY_ITEM ");
            sb.Append(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSALARY_YM_By_Fn()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as SALARY_YM ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    #region Qry

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                        string salary_ym, string salary_dt, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression == "")
            {
                //sortExpression = "alltb.KIND_CD ASC,alltb.ORDER_SEQ";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            SELECT t1.SALARY_DT,t1.SALARY_YM,t1.SALARY_TYPE,t1.SALARY_SDT ,t1.SALARY_EDT ,t1.DUTY_SDT,t1.DUTY_EDT
                                             ,t2.EMP_ID,t.EMP_NAME as EMP_NAME,c.SUB_DESC as SALARY_TYPE_DESC																																																																													
                                        from TB_S_M_EMP_RESULT t2 
                                        left join TB_S_M_SALARY_CAL_H t1 
                                            on t1.SALARY_YM = t2.SALARY_YM and t2.SALARY_DT = t1.SALARY_DT
                                            --and t1.SALARY_TYPE = t2.SALARY_TYPE                                                  																																																																													
                                        left join TB_H_M_EMP t  
                                            on t2.EMP_ID = t.EMP_ID
                                        left join TB_9_M_COMM_D c
                                            on c.SYS_CD = 'SC' 
                                            and c.MAIN_CD = 'SALARY_TYPE'
                                            and t1.SALARY_TYPE = c.SUB_CD 
                                        where 1=1 and t1.SALARY_TYPE ='A' ");
																																																																													
            if (salary_ym != "" && salary_ym != null)
            {
                sb.AppendLine(" and t1.SALARY_YM = @SALARY_YM  ");
                ht.Add("@SALARY_YM", salary_ym);
            }
            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t1.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "" && emp_name != null)
            {
                sb.AppendLine(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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
    public int getCount(int startRowIndex, int maximumRows, 
                        string salary_ym,  string salary_dt, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"            SELECT t1.SALARY_DT,t1.SALARY_YM,t1.SALARY_TYPE,t1.SALARY_SDT ,t1.SALARY_EDT ,t1.DUTY_SDT,t1.DUTY_EDT
                                             ,t2.EMP_ID,t.EMP_NAME as EMP_NAME,c.SUB_DESC as SALARY_TYPE_DESC
                                        from TB_S_M_EMP_RESULT t2 
                                        left join TB_S_M_SALARY_CAL_H t1   
                                            on t1.SALARY_YM = t2.SALARY_YM and t2.SALARY_DT = t1.SALARY_DT
                                            --and t1.SALARY_TYPE = t2.SALARY_TYPE  																																																																													
                                        left join TB_H_M_EMP t  
                                            on t2.EMP_ID = t.EMP_ID
                                        left join TB_9_M_COMM_D c
                                            on c.SYS_CD = 'SC' 
                                            and c.MAIN_CD = 'SALARY_TYPE'
                                            and t1.SALARY_TYPE = c.SUB_CD                                                                               																																																																													
                                        where 1=1 and t1.SALARY_TYPE ='A' ");
																																																																													
            if (salary_ym != "" && salary_ym != null)
            {
                sb.AppendLine(" and t1.SALARY_YM = @SALARY_YM  ");
                ht.Add("@SALARY_YM", salary_ym);
            }
            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t1.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "" && emp_name != null)
            {
                sb.AppendLine(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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

    public DataTable getDetail1Data(string salary_type, string salary_ym, string emp_id)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@" SELECT  t1.SALARY_YM,t1.SALARY_DT,t1.EMP_ID,t1.EMP_NAME,t1.RELATIVES,t1.SALARY_ACCOUNT_NO,t1.SALARY_EMAIL,t1.FAMILY_BIRTH_DT_1																																																																													
 	                                ,t1.FAMILY_BIRTH_DT_2,t1.IS_ALLOWANCE,t1.DEPT_NO,d1.DEPT_NAME DEPT_DESC,t1.EMP_CD,t1.EMP_CD +'-'+ d.SUB_DESC as DESC1																																																																												
 	                                ,t1.PLANT_CD,t1.PLANT_CD +'-'+ e.SUB_DESC as DESC3,t1.WS_CD,t1.WS_CD +'-'+ f.SUB_DESC as DESC4,t1.COMPANY_CD,l.COMPANY_SNAME,t1.JPN_CD																																																																												
	                                ,t1.JPN_CD +'-'+ g.SUB_DESC as DESC5,t1.NATION_CD,t1.NATION_CD +'-'+ h.SUB_DESC as DESC6,t1.LEVEL_CD,t1.GRADE_CD																																																																												
	                                ,t1.TRANSFER_COMPANY_CD,t1.TRANSFER_COMPANY_CD +'-'+ i.SUB_DESC as DESC7,t1.PJOB_CD,m.PJOB_DESC,t1.WORK_SHIFT_CD,t1.ACC_CD																																																																												
	                                ,t1.ACC_CD +'-'+ j.SUB_DESC as DESC8,t1.INCOME_CD,t1.INCOME_CD +'-'+ k.SUB_DESC as DESC9,t1.EMP_CHG_CD																																																																												
	                                ,t1.EMP_CHG_DATE,t1.JOIN_DT,t1.LEAVE_DT,t1.LEAVE_REASON,t1.PLAN_RETENTION_EDT,t1.RETENTION_EDT																																																																												
	                                --,t1.GCC_SDT,t1.PLAN_GCC_EDT,t1.GCC_EDT,t1.ICT_SDT,t1.PLAN_ICT_EDT
	                                --,t1.ICT_EDT,t1.SUPPORT_SDT,t1.PLAN_SUPPORT_EDT,t1.SUPPORT_EDT
	                                ,t1.BACK_SCHOOL_DT,t1.BACK_PLANT_DT,t1.BE_CONTRACT_DT,t1.BE_DESPATCH_DT,t1.BE_EMP_DT,t1.RECENT_LEVEL_DT																																																																												
	                                ,t1.RECENT_PJOB_DT,t1.RECENT_DEPT_DT,t1.RECENT_DIV_DT  																																																																												
	                                ,t1.RECENT_LEVEL_WORK_DAYS,dbo.FN_S_YEARS_DATA(t1.RECENT_LEVEL_WORK_DAYS) as RECENT_LEVEL_WORK_YEARS
	                                ,t1.RECENT_PJOB_WORK_DAYS,dbo.FN_S_YEARS_DATA(t1.RECENT_PJOB_WORK_DAYS) as RECENT_PJOB_WORK_YEARS
	                                ,t1.RECENT_DEPT_WORK_DAYS,dbo.FN_S_YEARS_DATA(t1.RECENT_DEPT_WORK_DAYS) as RECENT_DEPT_WORK_YEARS
	                                ,t1.RECENT_DIV_WORK_DAYS,dbo.FN_S_YEARS_DATA(t1.RECENT_DIV_WORK_DAYS) as RECENT_DIV_WORK_YEARS
	                                ,t1.WORK_DAYS,dbo.FN_S_YEARS_DATA(t1.WORK_DAYS) as WORK_YEARS,t1.SERVICE_DAYS,dbo.FN_S_YEARS_DATA(t1.SERVICE_DAYS) as SERVICE_YEARS
	                                ,t1.LEVEL_PAY,t1.ABILITY_PAY,t1.PJOB_PAY,t1.PROFESSION_PAY
	                                ,t1.ACC_DEPT_NO,t1.WORK_DAYS_MONTH,t1.CAL_WORK_DAYS,HOURLY_WAGE
                                from TB_S_M_EMP_RESULT t1 																																																																													
                                  left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t1.EMP_CD = d.SUB_CD   																																																																													
                                  left join TB_9_M_COMM_D e on  e.SYS_CD ='HB' and  e.MAIN_CD='PLANT_CD' and  t1.PLANT_CD = e.SUB_CD       																																																																													
                                  left join TB_9_M_COMM_D f on  f.SYS_CD ='HB' and  f.MAIN_CD='WS_CD' and  t1.WS_CD = f.SUB_CD 																																																																													
                                  left join TB_9_M_COMM_D g on  g.SYS_CD ='HB' and  g.MAIN_CD='JPN_CD' and  t1.JPN_CD = g.SUB_CD            																																																																													
                                  left join TB_9_M_COMM_D h on  h.SYS_CD ='HB' and  h.MAIN_CD='NATION_CD' and  t1.NATION_CD = h.SUB_CD     																																																																													
                                  left join TB_9_M_COMM_D i on  i.SYS_CD ='HC' and  i.MAIN_CD='ICT_COMPANY_CD' and  t1.TRANSFER_COMPANY_CD = i.SUB_CD     																																																																													
                                  left join TB_9_M_COMM_D j on  j.SYS_CD ='HA' and  j.MAIN_CD='ACC_CD' and  t1.ACC_CD = j.SUB_CD       																																																																													
                                  left join TB_9_M_COMM_D k on  k.SYS_CD ='SC' and  k.MAIN_CD='INCOME_CD' and  t1.INCOME_CD = k.SUB_CD 																																																																													
                                  left join TB_H_M_COMPANY l on t1.COMPANY_CD = l.COMPANY_CD 																																																																													
                                  left join TB_H_M_PJOB m on t1.PJOB_CD = m.PJOB_CD 
                                  left join TB_H_M_DEPT d1 on t1.DEPT_NO = d1.DEPT_NO																																																																													
                                where 1=1 ");

            if (salary_ym != "" && salary_ym != null)
            {
                sb.AppendLine(" and t1.SALARY_YM = @SALARY_YM  ");
                ht.Add("@SALARY_YM", salary_ym);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t1.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }            

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    //勤務月結資料畫面-表頭:以查詢畫面選取的資料列,以資料列.發薪日期 讀取 薪資用勤務月結資料主檔(TB_S_DUTY_RESULT_H)資料 .
    public DataTable getDetail2Data_duty(string salary_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@" SELECT t1.SALARY_YM,t1.SALARY_DT,t1.EMP_ID,t1.EMP_NAME,t3.DUTY_SDT,t3.DUTY_EDT
                                from TB_S_M_DUTY_RESULT_H t2
                                left join TB_S_M_EMP_RESULT t1 on t2.SALARY_DT = t1.SALARY_DT
                                left join TB_S_M_SALARY_CAL_H t3 on t3.SALARY_TYPE = 'A' and t2.SALARY_DT = t3.SALARY_DT
                                where 1=1 ");

            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t2.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t1.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    //勤務月結資料畫面-加班明細GIRD:以查詢畫面選取的資料列,以資料列.發薪日期+資料列.工號 讀取 薪資用加班月結明細檔(TB_S_OVERTIME_RESULT_D)資料 .
    public DataTable getDetail2Data_overtime(string salary_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@" SELECT t2.SALARY_DT,t2.EMP_ID,t2.OVERTIME_PAY_TYPE,t2.TOTAL_HOURS,t2.OVERTIME_PAY_TYPE + '-' + d.SUB_DESC as DESC1, d.SUB_DESC OVERTIME_PAY_TYPE_DESC
                                from TB_S_M_OVERTIME_RESULT_D t2
                                left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='OVERTIME_PAY_TYPE' and  t2.OVERTIME_PAY_TYPE = d.SUB_CD
                                where 1=1 ");

            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t2.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            //sb.AppendLine(" order by convert(int,sub_cd) ; ");//代號已經改成有英文字...無法再轉型

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDetail2Data_leave(string salary_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"  SELECT t2.SALARY_DT,t2.EMP_ID,t2.SUB_LEAVE_CD,t2.TOTAL_HOURS,t2.LEAVE_PAY_RATE,t2.LEAVE_CNT_UNIT,t1.SUB_LEAVE_DESC
                                 ,t2.LEAVE_CNT_UNIT + '-' + d.SUB_DESC as DESC1, d.SUB_DESC LEAVE_CNT_UNIT_DESC
                                 from TB_S_M_LEAVE_RESULT_D t2
                                 left join TB_D_M_LEAVE_TYPE_D t1 on t2.SUB_LEAVE_CD = t1.SUB_LEAVE_CD
                                 left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='LEAVE_CNT_UNIT' and  t2.LEAVE_CNT_UNIT = d.SUB_CD
                                 where 1=1 ");

            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t2.SALARY_DT = @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDetail2Data_work(string salary_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@" SELECT t2.SALARY_DT,t2.EMP_ID,t2.WORK_SHIFT_ALLOWANCE_TYPE,t2.TOTAL_DAYS,t2.WORK_SHIFT_ALLOWANCE_TYPE + '-' + d.SUB_DESC as DESC1, d.SUB_DESC WORK_SHIFT_ALLOWANCE_TYPE_DESC
                                from TB_S_M_WORK_SHIFT_ALLOWANCE_D t2
                                left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' and  t2.WORK_SHIFT_ALLOWANCE_TYPE = d.SUB_CD       																																																																													
                                where 1=1 ");
            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t2.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDetail2Data_available(string salary_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@" SELECT t2.SALARY_DT,t2.EMP_ID,t2.DATA_YEAR,t2.LEAVE_ALLOWANCE_TYPE,t2.TOTAL_HOURS,t2.IS_YN ,t2.LEAVE_ALLOWANCE_TYPE + '-' + d.SUB_DESC as DESC1, d.SUB_DESC LEAVE_ALLOWANCE_TYPE_DESC
                                from TB_S_M_AVAILABLE_LEAVE_D t2
                                left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='LEAVE_ALLOWANCE_TYPE' and  t2.LEAVE_ALLOWANCE_TYPE = d.SUB_CD       																																																																													
                                where 1=1 ");

            if (salary_dt != "" && salary_dt != null)
            {
                sb.AppendLine(" and t2.SALARY_DT = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            sb.AppendLine(" order by t2.DATA_YEAR,t2.LEAVE_ALLOWANCE_TYPE");
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    //internal DataTable getExistData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        //sb.AppendLine("Select * from TB_9_M_COMM_H where SYS_CD+MAIN_CD = @SYS_CD+@MAIN_CD");
    //        //ht.Add("@SYS_CD", SYS_CD);
    //        //ht.Add("@MAIN_CD", MAIN_CD);

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_S_M_SALARY_GROUP_H ");
            sb.AppendLine(" Set GROUP_NAME = @GROUP_NAME,CLASSIFY = @CLASSIFY,ORDER_SEQ = @ORDER_SEQ ");
            sb.AppendLine("     ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where KIND_CD+GROUP_ID = @KIND_CD+@GROUP_ID");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@GROUP_NAME", GROUP_NAME);
            ht.Add("@CLASSIFY", CLASSIFY);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" SELECT t1.KIND_CD, t1.GROUP_TYPE, t1.GROUP_ID, t1.GROUP_NAME, t1.CLASSIFY ");
        sb.AppendLine(" ,t1.KIND_CD +'-'+ d.SUB_DESC as KIND_CD_name ");
        sb.AppendLine(" ,t1.GROUP_TYPE +'-'+ e.SUB_DESC as GROUP_TYPE_name ");
        sb.AppendLine(" from TB_S_M_SALARY_GROUP_H t1");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='KIND_CD' and  t1.KIND_CD = d.SUB_CD ");
        sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='GROUP_TYPE' and  t1.GROUP_TYPE = e.SUB_CD ");
        sb.AppendLine(" where KIND_CD+GROUP_ID = @QDATAKEY");
        ht.Add("@QDATAKEY", qdatakey);
        DataTable dt = dbConn.Query(sb, ht);

        return dt;
    }
    public DataTable getSelectedData(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine("  select d1.SALARY_ID, d1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME  ");
        sb.AppendLine(" from TB_S_M_SALARY_GROUP_D d1 ");
        sb.AppendLine(" left join TB_S_M_SALARY_ITEM s on d1.SALARY_ID = s.SALARY_ID ");
        sb.AppendLine(" where KIND_CD+GROUP_ID = @QDATAKEY ");
        sb.AppendLine(" order by SALARY_ID ASC ");
        ht.Add("@QDATAKEY", qdatakey);
        DataTable dt = dbConn.Query(sb, ht);

        return dt;
    }

    public DataTable getNonSelectedData(string salary_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select s.SALARY_ID, s.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME  ");
        sb.AppendLine(" from TB_S_M_SALARY_ITEM s  ");
        sb.AppendLine(" where 1=1 ");
        if (salary_id != "" && salary_id != null)
        {
            sb.AppendLine(" and  s.SALARY_ID not in (" + salary_id + ") ");
        }
        sb.AppendLine(" order by s.SALARY_ID ASC ");

        DataTable dt = dbConn.Query(sb, ht);
        return dt;
    }
    public string deleteDtlData(string kind_cd, string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Delete from TB_S_M_SALARY_GROUP_D ");
            sb.AppendLine(" where KIND_CD = @KIND_CD and GROUP_ID = @GROUP_ID ");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_ID", group_id);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    internal DataTable getExistDtlData(string kind_cd, string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) as total from TB_S_M_SALARY_GROUP_H ");
            sb.AppendLine(" where KIND_CD = @KIND_CD and GROUP_ID = @GROUP_ID ");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_ID", group_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateDtlData(string kind_cd, string group_id, string selectedSalary_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_M_SALARY_GROUP_D ");
            sb.AppendLine(" (kind_cd,GROUP_ID,SALARY_ID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine(" ");
            sb.AppendLine(" Values (@KIND_CD,@GROUP_ID,@SALARY_ID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_ID", group_id);
            ht.Add("@SALARY_ID", selectedSalary_ID);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
}