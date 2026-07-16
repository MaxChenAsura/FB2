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
/// CFB2HA0300BO 的摘要描述
/// </summary>
public class CFB2SB2100DAO : BaseDAO
{
    public CFB2SB2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string YEAR_MONTH { get; set; }
    public string CHG_AMT_A { get; set; }
    public string END_DT_A { get; set; }
    public string DATA_YM { get; set; }
    public string START_DT_S { get; set; }
    public string APP_REMARK { get; set; }
    public string APPROVE_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }

    public string SALARY_NAME { get; set; }
    public string SALARY_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string AMOUNT { get; set; }
    public string START_DT { get; set; }
    public string START_DT_E { get; set; }
    public string REMARK { get; set; }
    public string LOGINID { get; set; }
    public int SEQ_NO { get; set; }
    public string EDT { get; set; }

    public DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='EMP_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSYS_ID(string SUB_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
            ht.Add("@SUB_CD", SUB_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSubsidyCount()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_S_M_SUBSIDY_MEM_D ");
            sb.Append(" where TYPE=@TYPE ");
            sb.Append(" and EMP_ID=@EMP_ID ");
            ht.Add("@TYPE", "1");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getLatestSalaryYM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as SALARY_YM ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getEndtSalaryYM(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select convert(varchar,SALARY_EDT,111) SALARY_EDT from TB_S_M_SALARY_CAL_H
                        Where PROCESS_STATUS >= '2'
                        And SALARY_TYPE = 'A'
                        and SALARY_YM = @SALARY_DT
                        Order By SALARY_DT Desc");

            ht.Add("@SALARY_DT", SALARY_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 取得 前一個月的薪資是否已鎖定
    /// </summary>
    /// <returns></returns>
    public DataTable getIsLoked()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_LOCKED from TB_S_M_SALARY_MONTH_CTRL ");
            sb.Append(" where SALARY_TYPE=@SALARY_TYPE ");
            sb.Append(" and OPERATION_ID =@OPERATION_ID ");
            sb.Append(" and SALARY_YM =@SALARY_YM ");
            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@OPERATION_ID", "G01");
            ht.Add("@SALARY_YM", DATA_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable getFUNC_ID(string ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT        *");
            sb.Append(" FROM            TB_9_M_SYS_M");
            sb.Append(" WHERE SYS_ID+MODE_ID = @ID");
            ht.Add("@ID", ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //public System.Data.DataTable getSYS_ID()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
    //        return dbConn.Query(sb);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public DataTable getDefaultData(string emp_id, string SALARY_ID, string START_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t1.EMP_ID as EMP_ID ,t.EMP_NAME as EMP_NAME, t1.SALARY_ID  as SALARY_ID ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t.EMP_CD+'-'+ d.SUB_DESC as DESC1,t1.EMP_ID+'/'+t.EMP_NAME as ID_NAME,t.DEPT_NO+'-'+	tt.DEPT_NAME as DEPT_A,t.EMP_CD+'-'+comm.SUB_DESC as EMP_CD_DESC  		");
            sb.Append("   ,t1.AMOUNT as CHG_AMT_B ,t1.START_DT as START_DT_A ,t1.END_DATE as END_DT_A 		");
            sb.Append("   ,'' as CHG_AMT_B ,'N' as PROCESS_STATUS ,'U' as CHG_STATUS , t1.APPROVE_DT as APPROVE_DT ,t1.APPROVE_BY as APPROVE_BY ,t1.REMARK as REMARK");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_2 t1 		");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID  		");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID 		");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD	   ");
            sb.Append(" left JOIN VW_H_DEPT_DATA tt ON t.DEPT_NO = tt.DEPT_NO ");
            sb.Append("  left JOIN TB_9_M_COMM_D COMM ON t.EMP_CD = COMM.SUB_CD and COMM.MAIN_CD = 'EMP_CD' AND COMM.SYS_CD = 'HB' ");
            sb.Append(" where 1=1 and t1.EMP_ID =@EMP_ID and t1.SALARY_ID = @SALARY_ID and t1.START_DT  = @START_DT ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getDefaultData2(string emp_id, string SALARY_ID, string START_DT,string seq_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("   select t2.EMP_ID as EMP_ID ,t.EMP_NAME as EMP_NAME, t2.SALARY_ID  as SALARY_ID ,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.EMP_ID+'/'+t.EMP_NAME as ID_NAME,t.DEPT_NO+'-'+	tt.DEPT_NAME as DEPT_A,t.EMP_CD+'-'+comm.SUB_DESC as EMP_CD_DESC			");
            sb.Append("   ,t2.CHG_AMT_A as CHG_AMT_A ,t2.START_DT_A as START_DT_A,t2.END_DATE_A as END_DT_A ,t2.START_DT_B as START_DT_B,t2.END_DATE_B as END_DT_B");
            sb.Append("   ,t2.CHG_AMT_B as CHG_AMT_B,t2.PROCESS_STATUS as PROCESS_STATUS ,t2.PROCESS_STATUS +'-'+ d.SUB_DESC as DESC1,t2.CHG_STATUS as CHG_STATUS ");
            sb.Append("   ,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC2,t2.APPROVE_DT as APPROVE_DT ,t2.APPROVE_BY as APPROVE_BY    				");
            sb.Append("   ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK				");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_2_TMP t2 				");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t2.SALARY_ID = s.SALARY_ID				");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID  				");
            sb.Append(" left JOIN VW_H_DEPT_DATA tt ON t.DEPT_NO = tt.DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD 				");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD                                     ");
            sb.Append("  left JOIN TB_9_M_COMM_D COMM ON t.EMP_CD = COMM.SUB_CD and COMM.MAIN_CD = 'EMP_CD' AND COMM.SYS_CD = 'HB' ");
            sb.Append(" where 1=1 and t2.EMP_ID =@EMP_ID and t2.SALARY_ID = @SALARY_ID and t2.START_DT_A  = @START_DT and t2.SEQ_NO = @SEQ_NO");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@SEQ_NO", seq_no);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD=CAR_TYPE  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_EMP_ID, string ddl_EMP_CD, string txt_EMP_NAME, string txt_SALARY_ID, string txt_START_DT_S, string txt_START_DT_E, string txt_SALARY_NAME)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "EMP_ID asc";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select * From (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, * from (");
            sb.Append(" select t1.EMP_ID as EMP_ID,t.EMP_NAME as EMP_NAME,t.EMP_CD as EMP_CD,t.EMP_CD+'-'+ d.SUB_DESC as DESC1");
            sb.Append(" , t1.SALARY_ID  as SALARY_ID,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t1.AMOUNT as CHG_AMT_A");
            sb.Append(" ,t1.START_DT as START_DT_B,t1.END_DATE as END_DATE_B, t1.START_DT as START_DT_A,t1.END_DATE as END_DATE_A");
            sb.Append(" , '0' as CHG_AMT_B,'Y' as PROCESS_STATUS ,'Y-已生效'as DESC2,''as CHG_STATUS, '' as DESC3,t1.APPROVE_DT as APPROVE_DT");
            sb.Append(" ,t1.REMARK as REMARK,'' as APP_REMARK,t1.CREATED_BY ");
            sb.Append(" ,t1.APPROVE_BY, t1.APPROVE_BY + CASE WHEN ISNULL(at.EMP_NAME,'') = '' THEN '' ELSE '-' + at.EMP_NAME END AS APPROVE_NAME");
            sb.Append(" ,'0' as SEQ_NO ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_2 t1 ");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_H_M_EMP at  on t1.APPROVE_BY = at.EMP_ID");
            sb.Append(" where (1=1) and  (t1.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' AND EMP_ID = @LOGIN_ID))");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t1.SALARY_ID = @SALARY_ID)  ");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)  ");
            }
            if (txt_START_DT_S != "")
            {
                sb.Append(" and (t1.START_DT >= @START_DT_S)  ");
            }
            if (txt_START_DT_E != "")
            {
                sb.Append(" and (t1.START_DT <= @START_DT_E)  ");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t1.EMP_ID LIKE @EMP_ID)  ");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)  ");
            }
            sb.Append(" UNION ");
            sb.Append(" select t2.EMP_ID as EMP_ID,t.EMP_NAME as EMP_NAME,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1");
            sb.Append(" ,t2.SALARY_ID as SALARY_ID,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A");
            sb.Append(" ,t2.START_DT_B as START_DT_B,t2.END_DATE_B as END_DATE_B,t2.START_DT_A as START_DT_A,t2.END_DATE_A as END_DATE_A ");
            sb.Append(" ,t2.CHG_AMT_B as CHG_AMT_B,t2.PROCESS_STATUS as PROCESS_STATUS,t2.PROCESS_STATUS+'-'+ h.SUB_DESC as DESC2");
            sb.Append(" ,t2.CHG_STATUS as CHG_STATUS,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC3,t2.APPROVE_DT as APPROVE_DT");
            sb.Append(" ,t2.REMARK as REMARK,t2.APP_REMARK as APP_REMARK,t2.CREATED_BY ");
            sb.Append(" ,t2.APPROVE_BY, t2.APPROVE_BY + CASE WHEN ISNULL(appt.EMP_NAME,'') = '' THEN '' ELSE '-' + appt.EMP_NAME END AS APPROVE_NAME");
            sb.Append(" ,t2.SEQ_NO ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_2_TMP t2");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_H_M_EMP appt  on t2.APPROVE_BY = appt.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='PROCESS_STATUS' and h.SYS_CD ='SA' and  t2.PROCESS_STATUS = h.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" where 1=1 and  t2.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  )  and t2.PROCESS_STATUS <>'Y'");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t2.SALARY_ID = @SALARY_ID)  ");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)  ");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t2.EMP_ID LIKE @EMP_ID)  ");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)  ");
            }
            sb.Append(" ) as tb");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);


            ht.Add("@SALARY_ID", txt_SALARY_ID);
            ht.Add("@EMP_CD", ddl_EMP_CD);
            ht.Add("@START_DT_S", txt_START_DT_S);
            ht.Add("@START_DT_E", txt_START_DT_E);
            ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string txt_EMP_ID, string ddl_EMP_CD, string txt_EMP_NAME, string txt_SALARY_ID, string txt_START_DT_S, string txt_START_DT_E, string txt_SALARY_NAME)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) as total_record from (");
            sb.Append(" select t1.EMP_ID as EMP_ID,t.EMP_NAME as EMP_NAME,t.EMP_CD as EMP_CD,t.EMP_CD+'-'+ d.SUB_DESC as DESC1");
            sb.Append(" , t1.SALARY_ID  as SALARY_ID,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t1.AMOUNT as CHG_AMT_A");
            sb.Append(" , t1.START_DT as START_DT_B,t1.END_DATE as END_DATE_B,'' as START_DT_A,'' as END_DATE_A");
            sb.Append(" , '0' as CHG_AMT_B,'' as PROCESS_STATUS, '' as DESC2,''as CHG_STATUS, '' as DESC3,t1.APPROVE_DT as APPROVE_DT");
            sb.Append(" , t1.APPROVE_BY as APPROVE_BY,t1.REMARK as REMARK,'' as APP_REMARK");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_2 t1 ");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" where (1=1) and  (t1.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' AND EMP_ID = @LOGIN_ID))");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t1.SALARY_ID = @SALARY_ID)  ");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)  ");
            }
            if (txt_START_DT_S != "")
            {
                sb.Append(" and (t1.START_DT >= @START_DT_S)  ");
            }
            if (txt_START_DT_E != "")
            {
                sb.Append(" and (t1.START_DT <= @START_DT_E)  ");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t1.EMP_ID LIKE @EMP_ID)  ");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)  ");
            }
            sb.Append(" UNION ");
            sb.Append(" select t2.EMP_ID as EMP_ID,t.EMP_NAME as EMP_NAME,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1");
            sb.Append(" ,t2.SALARY_ID as SALARY_ID,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A");
            sb.Append(" ,t2.START_DT_B as START_DT_B,t2.END_DATE_B as END_DATE_B,t2.START_DT_A as START_DT_A,t2.END_DATE_A as END_DATE_A ");
            sb.Append(" ,t2.CHG_AMT_B as CHG_AMT_B,t2.PROCESS_STATUS as PROCESS_STATUS,t2.PROCESS_STATUS+'-'+ h.SUB_DESC as DESC2");
            sb.Append(" ,t2.CHG_STATUS as CHG_STATUS,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC3,t2.APPROVE_DT as APPROVE_DT");
            sb.Append(" ,t2.APPROVE_BY as APPROVE_BY,t2.REMARK as REMARK,t2.APP_REMARK as APP_REMARK");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_2_TMP t2");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='PROCESS_STATUS' and h.SYS_CD ='SA' and  t2.PROCESS_STATUS = h.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" where 1=1 and  t2.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  )  and t2.PROCESS_STATUS <>'Y'");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t2.SALARY_ID = @SALARY_ID)  ");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)  ");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t2.EMP_ID LIKE @EMP_ID)  ");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)  ");
            }
            sb.Append(" ) as tb");

            ht.Add("@SALARY_ID", txt_SALARY_ID);
            ht.Add("@EMP_CD", ddl_EMP_CD);
            ht.Add("@START_DT_S", txt_START_DT_S);
            ht.Add("@START_DT_E", txt_START_DT_E);
            ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

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

    public DataTable getModeData(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");

            ht.Add("@ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(int startRowIndex, int maximumRows, string sortExpression, string id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "MODE_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@ID", id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getModeCount(int startRowIndex, int maximumRows, string id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record from (");
            sb.Append(" 	select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) as tb1");

            ht.Add("@ID", id);
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

    //public DataTable getData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(" Select * From TB_9_M_COMM_H";
    //         sb.Append(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.Append(" and SYS_CD = @SYS_CD ";
    //            ht.Add("@SYS_CD", SYS_CD);
    //        }

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        char[] ch1 = new Char[] { '|' };
        string[] split1 = deleteitem.Split(ch1);
        string a = split1[0].ToString();
        string b = split1[1].ToString();
        string c = split1[2].ToString();
        sb.Append("Delete from TB_S_M_SUBSIDY_DEDU_2_TMP where  ");
        sb.Append(" EMP_ID = @a and SALARY_ID = @b and START_DT_A = @c");
        ht.Add("@a", a);
        ht.Add("@b", b);
        ht.Add("@c", c);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_SUBSIDY_DEDUCTIONS_2 where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getExistData1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_SUBSIDY_DEDU_2_TMP where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_SUBSIDY_DEDU_2_TMP (EMP_ID,SALARY_ID,START_DT_A,START_DT_B,END_DATE_A,END_DATE_B,CHG_AMT_A,CHG_AMT_B,CHG_STATUS,PROCESS_STATUS,REMARK,SEQ_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,APPROVE_BY,APP_REMARK,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@SALARY_ID,@START_DT_A,@START_DT_B,@END_DATE_A,@END_DATE_B,@CHG_AMT_A,@CHG_AMT_B,@CHG_STATUS,@PROCESS_STATUS,@REMARK,@SEQ_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@APPROVE_BY,@APP_REMARK,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT);
            ht.Add("@START_DT_B", START_DT);
            if (string.IsNullOrEmpty(START_DT_E))
            {
                START_DT_E = "9999/12/31";
            }
            ht.Add("@END_DATE_A", START_DT_E);
            ht.Add("@END_DATE_B", START_DT_E);
            ht.Add("@CHG_AMT_A", AMOUNT);
            ht.Add("@CHG_AMT_B", "0");
            ht.Add("@CHG_STATUS", CHG_STATUS.Substring(0, 1));
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS.Substring(0, 1));
            ht.Add("@REMARK", REMARK);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@APPROVE_BY", "");

            ht.Add("@APP_REMARK", "");
            ht.Add("@FUNC_ID", "FB2SB2100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void updateData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("INSERT INTO TB_S_M_SUBSIDY_DEDU_2_TMP (EMP_ID,SALARY_ID,START_DT_A,START_DT_B,END_DATE_A,END_DATE_B,CHG_AMT_A,CHG_AMT_B,CHG_STATUS,PROCESS_STATUS,APPROVE_BY,REMARK,APP_REMARK,SEQ_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@SALARY_ID,@START_DT_A,@START_DT_B,@END_DATE_A,@END_DATE_B,@CHG_AMT_A,@CHG_AMT_B,@CHG_STATUS,@PROCESS_STATUS,@APPROVE_BY,@REMARK,@APP_REMARK,@SEQ_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", Convert.ToDateTime(START_DT_S));
            ht.Add("@START_DT_B", Convert.ToDateTime(START_DT_S));
            ht.Add("@END_DATE_A", Convert.ToDateTime(END_DT_A));
            ht.Add("@END_DATE_B", Convert.ToDateTime(START_DT_E));
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_AMT_B", AMOUNT);

            ht.Add("@CHG_STATUS", "U");
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS.Substring(0, 1));
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            ht.Add("@FUNC_ID", "FB2SB2100");

            dbConn.ExecuteT(sb, ht, true);


            //sb.Append("Update TB_S_M_SUBSIDY_DEDUCTIONS_2 ");
            //sb.Append(" Set END_DATE=@END_DATE,CHG_AMT_B=@CHG_AMT_B,CHG_AMT_A=@CHG_AMT_A,CHG_STATUS=@CHG_STATUS,PROCESS_STATUS=@PROCESS_STATUS,REMARK=@REMARK,CREATED_BY=@CREATED_BY,CREATED_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),APPROVE_BY=@APPROVE_BY,FUNC_ID=@FUNC_ID");
            //sb.Append(" where EMP_ID = @EMP_ID and SALARY_ID=@SALARY_ID and START_DT=@START_DT");
            //ht.Add("@EMP_ID", EMP_ID);
            //ht.Add("@SALARY_ID", SALARY_ID);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            //ht.Add("@END_DATE", Convert.ToDateTime(END_DT_A));

            //ht.Add("@CHG_AMT_B", CHG_AMT_A);
            //ht.Add("@CHG_AMT_A", AMOUNT);

            //ht.Add("@CHG_STATUS", "U");
            //ht.Add("@PROCESS_STATUS", PROCESS_STATUS.Substring(0, 1));

            //ht.Add("@APPROVE_BY", "");
            //ht.Add("@REMARK", REMARK);
            //ht.Add("@APP_REMARK", APP_REMARK);
            //ht.Add("@CREATED_BY", CREATED_BY);
            //ht.Add("@UPDATED_BY", UPDATED_BY);






            //ht.Add("@FUNC_ID", "FB2SB2100");




            //dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void updateData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Update TB_S_M_SUBSIDY_DEDU_2_TMP ");
            sb.Append(" Set END_DATE_A=@END_DATE_A,END_DATE_B=@END_DATE_B,CHG_AMT_B=@CHG_AMT_B,CHG_AMT_A=@CHG_AMT_A,CHG_STATUS=@CHG_STATUS,PROCESS_STATUS=@PROCESS_STATUS,REMARK=@REMARK,CREATED_BY=@CREATED_BY,CREATED_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),APPROVE_BY=@APPROVE_BY,FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and SALARY_ID=@SALARY_ID and START_DT_A=@START_DT and SEQ_NO=@SEQ_NO");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            //ht.Add("@START_DT_B", Convert.ToDateTime(START_DT_E));
            ht.Add("@END_DATE_A", Convert.ToDateTime(END_DT_A));
            ht.Add("@END_DATE_B", Convert.ToDateTime(START_DT_E));  //2019.11.01 fix
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_AMT_B", AMOUNT);

            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS.Substring(0, 1));
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@APPROVE_BY", "");

            ht.Add("@APP_REMARK", APP_REMARK);

            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@FUNC_ID", "FB2SB2100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void updateData3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDU_2_TMP");
            sb.Append("                          (START_DT_B, END_DATE_B, START_DT_A, END_DATE_A, CHG_AMT_B, CHG_AMT_A, CHG_STATUS, PROCESS_STATUS");
            sb.Append(" 						 , APPROVE_BY, APPROVE_DT, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID, EMP_ID, SALARY_ID, SEQ_NO)");
            sb.Append(" VALUES        (@START_DT_B,@END_DATE_B,@START_DT_A,@END_DATE_A,@CHG_AMT_B,@CHG_AMT_A,@CHG_STATUS,@PROCESS_STATUS");
            sb.Append(" 			,@APPROVE_BY,@APPROVE_DT,@REMARK,@APP_REMARK,@CREATED_BY, GETDATE(),@UPDATED_BY, GETDATE(),@FUNC_ID, @EMP_ID, @SALARY_ID, @SEQ_NO)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_B", START_DT);
            ht.Add("@END_DATE_B", START_DT_E);
            ht.Add("@START_DT_A", START_DT);
            ht.Add("@END_DATE_A", START_DT_E);
            ht.Add("@CHG_AMT_A", AMOUNT);
            ht.Add("@CHG_AMT_B", "0");

            ht.Add("@CHG_STATUS", "D");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_DT", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SB210");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getExistDataCheck1()
    {
        int t = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT count(*) as total_record from TB_S_M_SUBSIDY_DEDUCTIONS_D ");
            sb.Append(" where EMP_ID =@EMP_ID and SALARY_ID = @SALARY_ID ");
            sb.Append(" and SALARY_STATUS ='Y' and DATA_YM  >=   @START_DT_E and DATA_YM  <= @END_DT_A ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_E", Convert.ToDateTime(START_DT_S).ToString("yyyyMM"));
            ht.Add("@END_DT_A", Convert.ToDateTime(END_DT_A).ToString("yyyyMM"));

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

    public int getExistDataCheck2()
    {
        int t = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT count(*) as total_record from TB_S_M_SUBSIDY_DEDUCTIONS_D ");
            sb.Append(" where EMP_ID =@EMP_ID and SALARY_ID = @SALARY_ID ");
            sb.Append(" and SALARY_STATUS ='Y' and DATA_YM  > @END_DT_A ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@END_DT_A", Convert.ToDateTime(END_DT_A).ToString("yyyyMM"));

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

    public int getDeleteDataCheck()
    {
        int t = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  SELECT count(*) as total_record from TB_S_M_SUBSIDY_DEDUCTIONS_D ");
            sb.Append("  where EMP_ID =@EMP_ID and SALARY_ID = @SALARY_ID  and  SALARY_STATUS ='Y'  ");
            sb.Append("  and DATA_YM  >=   @START_DT_S and DATA_YM  <= @START_DT_E ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_S", Convert.ToDateTime(START_DT_S).ToString("yyyyMM"));
            ht.Add("@START_DT_E", Convert.ToDateTime(START_DT_E).ToString("yyyyMM"));

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

    public DataTable getMax_SEQ_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select MAX(SEQ_NO) as SEQ_NO from TB_S_M_SUBSIDY_DEDU_2_TMP where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}