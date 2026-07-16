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
/// wfb2SA2100 的摘要描述
/// </summary>
public class CFB2SA2100DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string SALARY_ID { get; set; }
    public string EFFECT_SDT_B { get; set; }
    public string EFFECT_EDT_B { get; set; }
    public string EFFECT_SDT_A { get; set; }
    public string EFFECT_EDT_A { get; set; }
    public string SEQ_NO { get; set; }
    public string SEQ_NO_B { get; set; }
    public string CHG_AMT_B { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string REMARK { get; set; }
    
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SA2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }

    public DataTable getAllSALARY_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_ID,SALARY_NAME ");
            sb.Append(" from TB_S_M_SALARY_ITEM ");
            sb.Append(" where IS_SALARY = 'Y'");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARY_ID(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct b.SALARY_ID,b.SALARY_NAME from ");
            sb.Append(" (select distinct SALARY_ID from TB_S_M_SALARY_TXN where EMP_ID=@EMP_ID ");
            sb.Append(" union select distinct SALARY_ID From TB_S_M_SALARY_TXN_TMP where EMP_ID=@EMP_ID) a");
            sb.Append(" inner join TB_S_M_SALARY_ITEM b");
            sb.Append("     on a.SALARY_ID = b.SALARY_ID");
            sb.Append(" where IS_SALARY='Y'");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string company_cd,
                            string emp_cd, string emp_status_cd, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t.EMP_ID");
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "t.COMPANY_CD");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "t.EMP_NAME");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "t.EMP_CD");
            if (sortExpression.Contains("JOIN_DT"))
                sortExpression = sortExpression.Replace("JOIN_DT", "t.JOIN_DT");
            if (sortExpression.Contains("EMP_STATUS"))
                sortExpression = sortExpression.Replace("EMP_STATUS", "vm.EMP_STATUS");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + "  ) As RowNumber ");
            sb.Append(" , t.EMP_ID, t.EMP_NAME, t.LEVEL_CD, t.GRADE_CD, t.EMP_CD, t.EMP_CD + '-' + d.SUB_DESC as DESC1 ");
            sb.Append(" , t.COMPANY_CD, t.COMPANY_CD + '-' + c.COMPANY_SNAME as DESC3, t.PJOB_CD, t.PJOB_CD + '-' + vm.PJOB_DESC as DESC2 ");
            sb.Append(" , t.JOIN_DT, IsNull(sum( s2.AMOUNT),0) as AMOUNT, vm.EMP_STATUS, vm.EMP_STATUS + '-' + m.SUB_DESC as DESC11 ");
            sb.Append(" from TB_H_M_EMP t  ");
            sb.Append(" left join VW_H_EMP_DATA vm on t.EMP_ID = vm.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on  t.COMPANY_CD = c.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D m on m.SYS_CD='HB' and m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD ");
            sb.Append(" left join ( ");
            sb.Append("    select EMP_ID,SALARY_ID,EFFECT_SDT,EFFECT_EDT,max(SEQ_NO) as SEQ_NO ");
            sb.Append("    from TB_S_M_SALARY_TXN ");
            sb.Append("    group by EMP_ID,SALARY_ID,EFFECT_SDT,EFFECT_EDT ");
            sb.Append("    )s1 on  t.EMP_ID = s1.EMP_ID and  s1.EFFECT_SDT<= convert(varchar,getdate(),111) and  s1.EFFECT_EDT>= convert(varchar,getdate(),111) ");
            sb.Append(" left join TB_S_M_SALARY_TXN s2 on t.EMP_ID = s2.EMP_ID and s1.SALARY_ID = s2.SALARY_ID ");
            sb.Append("    and s1.EFFECT_SDT = s2.EFFECT_SDT and s1.SEQ_NO = s2.SEQ_NO ");
            sb.Append(" where 1=1 ");

            //A.若聘用單位<>'' ==>  and t.COMPANY_CD =畫面.聘用單位. 
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and t.COMPANY_CD = @company_cd ");
                ht.Add("@company_cd", company_cd);
            }
            //B.員工區分<>'' ==>  and t.EMP_CD =畫面.員工區分. 
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and t.EMP_CD = @emp_cd ");
                ht.Add("@emp_cd", emp_cd);
            }
            //C.若在職狀態<>'' ==>   and vm.EMP_STATUS = 畫面.在職狀態.  
            if (emp_status_cd != "-1" && emp_status_cd != null)
            {
                sb.Append(" and vm.EMP_STATUS = @emp_status_cd ");
                ht.Add("@emp_status_cd", emp_status_cd);
            }
            //D.若工號<>'' ==>  and t.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t.EMP_ID like @emp_id ");
                ht.Add("@emp_id", emp_id + "%");
            }
            //E.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @emp_name ");
                ht.Add("@emp_name", emp_name + "%");
            }

            sb.Append(" group by t.EMP_ID, t.EMP_NAME, t.EMP_CD, t.LEVEL_CD, t.GRADE_CD");
            sb.Append(" , t.EMP_CD + '-' + d.SUB_DESC  , vm.EMP_STATUS ");
	        sb.Append(" , t.COMPANY_CD, t.COMPANY_CD + '-' + c.COMPANY_SNAME ");
	        sb.Append(" , t.PJOB_CD, t.PJOB_CD + '-' + vm.PJOB_Desc   , t.JOIN_DT ");
	        sb.Append(" , vm.EMP_STATUS + '-' + m.SUB_DESC ");

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            sb.Append(" Order By RowNumber");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string company_cd,
                            string emp_cd, string emp_status_cd, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from (select t.EMP_ID from TB_H_M_EMP t  ");
            sb.Append(" left join VW_H_EMP_DATA vm on t.EMP_ID = vm.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on  t.COMPANY_CD = c.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D m on m.SYS_CD='HB' and m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD ");
            sb.Append(" left join ( ");
            sb.Append("    select EMP_ID,SALARY_ID,EFFECT_SDT,EFFECT_EDT,max(SEQ_NO) as SEQ_NO ");
            sb.Append("    from TB_S_M_SALARY_TXN ");
            sb.Append("    group by EMP_ID,SALARY_ID,EFFECT_SDT,EFFECT_EDT ");
            sb.Append("    )s1 on  t.EMP_ID = s1.EMP_ID and  s1.EFFECT_SDT <= convert(varchar,getdate()) and  s1.EFFECT_EDT >= convert(varchar,getdate()) ");
            sb.Append(" left join TB_S_M_SALARY_TXN s2 on t.EMP_ID = s2.EMP_ID and s1.SALARY_ID = s2.SALARY_ID ");
            sb.Append("    and s1.EFFECT_SDT = s2.EFFECT_SDT and s1.SEQ_NO = s2.SEQ_NO ");
            sb.Append(" where 1=1 ");

            //A.若聘用單位<>'' ==>  and t.COMPANY_CD =畫面.聘用單位. 
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and t.COMPANY_CD = @company_cd ");
                ht.Add("@company_cd", company_cd);
            }
            //B.員工區分<>'' ==>  and t.EMP_CD =畫面.員工區分. 
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and t.EMP_CD = @emp_cd ");
                ht.Add("@emp_cd", emp_cd);
            }
            //C.若在職狀態<>'' ==>   and vm.EMP_STATUS = 畫面.在職狀態.  
            if (emp_status_cd != "-1" && emp_status_cd != null)
            {
                sb.Append(" and vm.EMP_STATUS = @emp_status_cd ");
                ht.Add("@emp_status_cd", emp_status_cd);
            }
            //D.若工號<>'' ==>  and t.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t.EMP_ID like @emp_id ");
                ht.Add("@emp_id", emp_id + "%");
            }
            //E.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @emp_name ");
                ht.Add("@emp_name", emp_name + "%");
            }

            sb.Append(" group by t.EMP_ID, t.EMP_NAME, t.LEVEL_CD, t.GRADE_CD, t.EMP_CD ");
            sb.Append(" , t.EMP_CD + '-' + d.SUB_DESC  , vm.EMP_STATUS ");
            sb.Append(" , t.COMPANY_CD, t.COMPANY_CD + '-' + c.COMPANY_SNAME ");
            sb.Append(" , t.PJOB_CD, t.PJOB_CD + '-' + vm.PJOB_Desc   , t.JOIN_DT ");
            sb.Append(" , vm.EMP_STATUS + '-' + m.SUB_DESC ");

            sb.Append(" ) as a ");

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

    public DataTable getEMPData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t.EMP_ID,t.EMP_NAME,t.EMP_CD,t.EMP_CD + '-' + e.SUB_DESC as EMP_CD_DESC ");
            sb.Append("     ,t.JOIN_DT,t.LEVEL_CD,t.GRADE_CD,t.COMPANY_CD ");
            sb.Append(" 	,t.COMPANY_CD + '-' + c.COMPANY_SNAME as COMPANY_SNAME ");
            sb.Append(" 	,t.PJOB_CD,t.PJOB_CD + '-' + v.PJOB_DESC as PJOB_CD_DESC,t.LEAVE_DT ");
            sb.Append(" 	,v.EMP_STATUS,v.EMP_STATUS + '-' + s.SUB_DESC as EMP_STATUS_DESC ");
            sb.Append(" from TB_H_M_EMP t ");
            sb.Append(" 	left join TB_H_M_COMPANY c on  t.COMPANY_CD = c.COMPANY_CD ");
            //sb.Append(" 	left join TB_9_M_COMM_D p on   p.SYS_CD='HB' and  p.MAIN_CD='PJOB_CD' and  t.PJOB_CD = p.SUB_CD ");
            sb.Append(" 	left join TB_9_M_COMM_D e on   e.SYS_CD='HB' and  e.MAIN_CD='EMP_CD' and  t.EMP_CD = e.SUB_CD ");
            sb.Append(" 	left join VW_H_EMP_DATA v on  t.EMP_ID = v.EMP_ID ");
            sb.Append(" 	left join TB_9_M_COMM_D s on   s.SYS_CD='HB' and  s.MAIN_CD='EMP_STATUS' and  v.EMP_STATUS = s.SUB_CD ");
            sb.Append(" where t.EMP_ID = @EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDetailData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string salary_id,
                         string process_status, string emp_status_cd, string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("  select ROW_NUMBER() OVER(ORDER BY SEQ,SALARY_ID,EFFECT_EDT_B DESC) As RowNumber, * from ");
            sb.Append(" ( ");
            sb.Append("  select t1.EMP_ID as EMP_ID ,t1.SALARY_ID  as SALARY_ID ,t1.SALARY_ID + '-' + IsNull(s.SALARY_NAME,'') as SALARY_NAME "); 
            sb.Append("               ,t1.AMOUNT as CHG_AMT_A ,t4.EFFECT_SDT_B ,t4.EFFECT_EDT_B ");
            sb.Append("               ,IsNull(t1.EFFECT_SDT,t4.EFFECT_SDT_A) as EFFECT_SDT_A ,IsNull(t1.EFFECT_EDT,t4.EFFECT_EDT_A) as EFFECT_EDT_A ");
            sb.Append("               ,IsNull(t4.SEQ_NO_B,t1.SEQ_NO) as SEQ_NO_B,0 as SEQ_NO,IsNull(t4.CHG_AMT_B,0) as CHG_AMT_B ,'Y' as PROCESS_STATUS ,'Y-已生效'as DESC1 ");
            sb.Append("               ,IsNull(t4.CHG_STATUS,'') as CHG_STATUS ");
            //sb.Append("               ,ISNULL(t4.CHG_STATUS+p.SUB_DESC ,'')as DESC2   ");
            sb.Append("               ,IIF(t4.CHG_STATUS is null,'', t4.CHG_STATUS+'-'+p.SUB_DESC ) as DESC2   ");
            sb.Append("                ,t1.APPROVE_BY as APPROVE_BY ");
            sb.Append("               ,t1.APPROVE_DT as APPROVE_DT ,IsNull(t4.REMARK,t1.REMARK )as REMARK,IsNull(t4.APP_REMARK,'') as APP_REMARK  ,'1' as SEQ ");
            sb.Append("  from TB_S_M_SALARY_TXN t1 ");    
            sb.Append("  left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID = s.SALARY_ID ");
            sb.Append("  left join (SELECT SALARY_ID,EMP_ID,EFFECT_SDT_B,SEQ_NO_B,max(SEQ_NO) as SEQ_NO ");
            sb.Append("                from TB_S_M_SALARY_TXN_TMP ");
            sb.Append("                where PROCESS_STATUS ='Y' ");
            sb.Append("                Group by SALARY_ID,EMP_ID,EFFECT_SDT_B,SEQ_NO_B)t3 on  t1.EMP_ID =t3.EMP_ID and t1.SALARY_ID = t3.SALARY_ID and t1.EFFECT_SDT = t3.EFFECT_SDT_B and t1.SEQ_NO = t3.SEQ_NO_B ");
            sb.Append("  left join TB_S_M_SALARY_TXN_TMP t4 on t1.EMP_ID =t4.EMP_ID and t1.SALARY_ID = t4.SALARY_ID and t1.EFFECT_SDT = t4.EFFECT_SDT_B and t3.SEQ_NO = t4.SEQ_NO ");
            sb.Append("  left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t4.CHG_STATUS = p.SUB_CD ");
            sb.Append("  where 1=1 and t1.EMP_ID = @EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            //A.若薪資項目<>'' ==>  and t1.SALARY_ID = 畫面.薪資項目;
            if (@salary_id != "-1")
            {
                sb.Append(" and t1.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            //B.若處理狀態<>''且 處理狀態 <>'Y' ==>  and t2.PROCESS_STATUS = 畫面.處理狀態;  

            if (process_status != "-1" && process_status != "Y")
            {
                sb.Append(" and 1 <> 1 ");
            }
            //if(@process_status!="") sb.Append(" and t2.PROCESS_STATUS = @PROCESS_STATUS ");


            //C.若生效起日<>'' ==>  and t1.EFFECT_EDT >= 畫面.生效起日;
            if (start_dt != "")
            {
                sb.Append(" and t1.EFFECT_EDT >= @EFFECT_SDT ");
                ht.Add("@EFFECT_SDT", start_dt);
            }

            //D0.若生效迄日='' 且非全部資料 ==>  重設迄日為9999/12/31;
            if (end_dt == "")
                end_dt = "9999/12/31";

            //D.若生效迄日<>'' ==>  and t1.EFFECT_SDT <= 畫面.生效迄日;
            if (end_dt != "")
            {
                sb.Append(" and t1.EFFECT_SDT <= @EFFECT_EDT ");
                //E.若生效迄日<>'' 且選取"有效" ==>  and t1.EFFECT_EDT >= 畫面.生效迄日;

                ht.Add("@EFFECT_EDT", end_dt);
            }
            sb.Append("  union all ");
            sb.Append("  select t2.EMP_ID as EMP_ID ,t2.SALARY_ID as SALARY_ID ,t2.SALARY_ID + '-' + IsNull(s.SALARY_NAME,'') as SALARY_NAME ");
            sb.Append("                    ,t2.CHG_AMT_A as CHG_AMT_A ,t2.EFFECT_SDT_B as EFFECT_SDT_B,t2.EFFECT_EDT_B as EFFECT_EDT_B ,t2.EFFECT_SDT_A as EFFECT_SDT_A ");
            sb.Append("                    ,t2.EFFECT_EDT_A as EFFECT_EDT_A,t2.SEQ_NO_B as SEQ_NO_B,t2.SEQ_NO as SEQ_NO,  t2.CHG_AMT_B  ");
            sb.Append("                    ,t2.PROCESS_STATUS as PROCESS_STATUS  ,t2.PROCESS_STATUS+ '-'+  d.SUB_DESC as DESC1,t2.CHG_STATUS as CHG_STATUS ");
            sb.Append("                     ,t2.CHG_STATUS+ '-'+  p.SUB_DESC as DESC2 ,t2.APPROVE_BY as APPROVE_BY ,t2.APPROVE_DT as APPROVE_DT ,t2.REMARK as REMARK ");
            sb.Append("                     ,t2.APP_REMARK as APP_REMARK, '2' as SEQ ");
            sb.Append("      from TB_S_M_SALARY_TXN_TMP t2 ");    
            sb.Append("      left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID ");    
            sb.Append("      left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD ");    
            sb.Append("      left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD ");
            sb.Append("      where 1=1 and t2.EMP_ID = @EMP_ID and t2.PROCESS_STATUS <>'Y' ");
            //  A.若薪資項目<>'' ==>  and t2.SALARY_ID =畫面.薪資項目
            if (@salary_id != "-1") sb.Append(" and t2.SALARY_ID = @SALARY_ID ");
            //  B.若處理狀態<>'' ==>  and t2.PROCESS_STATUS = 畫面.處理狀態;                     
            if (@process_status != "-1")
            {
                sb.Append(" and t2.PROCESS_STATUS = @PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            //C.若選取"有效" ==>  and t2.CHG_STATUS = NULL;
            sb.Append("  ) data ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            DataTable dt =  dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public int getDetailCount(int startRowIndex, int maximumRows, string emp_id, string salary_id,
                         string process_status, string emp_status_cd, string start_dt, string end_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record from ");
            sb.Append(" ( select t1.EMP_ID from TB_S_M_SALARY_TXN t1 ");
            sb.Append("  left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID = s.SALARY_ID ");
            sb.Append("  left join (SELECT SALARY_ID,EMP_ID,EFFECT_SDT_B,SEQ_NO_B,max(SEQ_NO) as SEQ_NO ");
            sb.Append("                from TB_S_M_SALARY_TXN_TMP ");
            sb.Append("                where PROCESS_STATUS ='Y' ");
            sb.Append("                Group by SALARY_ID,EMP_ID,EFFECT_SDT_B,SEQ_NO_B)t3 on  t1.EMP_ID =t3.EMP_ID and t1.SALARY_ID = t3.SALARY_ID and t1.EFFECT_SDT = t3.EFFECT_SDT_B and t1.SEQ_NO = t3.SEQ_NO_B ");
            sb.Append("  left join TB_S_M_SALARY_TXN_TMP t4 on t1.EMP_ID =t4.EMP_ID and t1.SALARY_ID = t4.SALARY_ID and t1.EFFECT_SDT = t4.EFFECT_SDT_B and t3.SEQ_NO = t4.SEQ_NO ");
            sb.Append("  left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t4.CHG_STATUS = p.SUB_CD ");
            sb.Append("  where 1=1 and t1.EMP_ID = @EMP_ID  ");

            ht.Add("@EMP_ID", emp_id);
            //A.若薪資項目<>'' ==>  and t1.SALARY_ID = 畫面.薪資項目;
            if (@salary_id != "-1")
            {
                sb.Append(" and t1.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            //B.若處理狀態<>''且 處理狀態 <>'Y' ==>  and t2.PROCESS_STATUS = 畫面.處理狀態;                      
            if (process_status != "-1" && process_status != "Y")
            {
                sb.Append(" and 1 <> 1 ");
            }
            //C.若生效起日<>'' ==>  and t1.EFFECT_EDT >= 畫面.生效起日;
            if (@start_dt != "")
            {
                sb.Append(" and t1.EFFECT_EDT >= @EFFECT_SDT ");
                ht.Add("@EFFECT_SDT", start_dt);
            }
            
            //D0.若生效迄日='' 且非全部資料 ==>  重設迄日為9999/12/31;
            if (end_dt == "")
                end_dt = "9999/12/31";

            //D.若生效迄日<>'' ==>  and t1.EFFECT_SDT <= 畫面.生效迄日;
            if (@end_dt != "")
            {
                sb.Append(" and t1.EFFECT_SDT <= @EFFECT_EDT ");
                //E.若生效迄日<>'' 且選取"有效" ==>  and t1.EFFECT_EDT >= 畫面.生效迄日;
                ht.Add("@EFFECT_EDT", end_dt);
            }

            sb.Append(" union all ");
            sb.Append(" select t2.EMP_ID from TB_S_M_SALARY_TXN_TMP t2 ");
            sb.Append("      left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID ");
            sb.Append("      left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append("      left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD ");
            sb.Append("      where 1=1 and t2.EMP_ID = @EMP_ID and t2.PROCESS_STATUS <>'Y' ");
            //  A.若薪資項目<>'' ==>  and t2.SALARY_ID =畫面.薪資項目
            if (@salary_id != "-1") sb.Append(" and t2.SALARY_ID = @SALARY_ID ");
            //  B.若處理狀態<>'' ==>  and t2.PROCESS_STATUS = 畫面.處理狀態;                     
            if (@process_status != "-1")
            {
                sb.Append(" and t2.PROCESS_STATUS = @PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            //C.若選取"有效" ==>  and t2.CHG_STATUS = NULL;

            sb.Append(" ) as DATA ");

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

    public DataTable getDetailFromSALARY_TXN()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t1.EMP_ID as EMP_ID ,t1.SALARY_ID  as SALARY_ID ,t1.SALARY_ID + '-' + s.SALARY_NAME as SALARY_NAME ");
            sb.Append(" ,t1.AMOUNT as CHG_AMT_B ,t1.AMOUNT as CHG_AMT_A,t1.EFFECT_SDT as EFFECT_SDT ,t1.EFFECT_EDT as EFFECT_EDT,t1.EFFECT_SDT as EFFECT_SDT_A ,t1.EFFECT_EDT as EFFECT_EDT_A  ,t1.SEQ_NO as SEQ_NO_B,0 as SEQ_NO");
            sb.Append(" ,'N' as PROCESS_STATUS,'U' as CHG_STATUS ");
            sb.Append(" ,t1.APPROVE_DT as APPROVE_DT ,t1.APPROVE_BY as APPROVE_BY ,t1.REMARK as REMARK ");
            sb.Append(" from TB_S_M_SALARY_TXN t1 ");
            sb.Append("     left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID = s.SALARY_ID ");
            sb.Append(" where 1=1 and t1.EMP_ID =@EMP_ID ");

            ht.Add("@EMP_ID", EMP_ID);

            if (SALARY_ID != "")
            {
                sb.Append(" and t1.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", SALARY_ID);
            }

            if (EFFECT_SDT_B != "")
            {
                sb.Append(" and t1.EFFECT_SDT = @EFFECT_SDT ");
                ht.Add("@EFFECT_SDT", EFFECT_SDT_B);
            }

            if (SEQ_NO != "")
            {
                sb.Append(" and t1.SEQ_NO = @SEQ_NO ");
                ht.Add("@SEQ_NO", SEQ_NO);
            }
            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDetailFromSALARY_TXN_TMP()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" select t2.EMP_ID as EMP_ID ,t2.SALARY_ID as SALARY_ID ,t2.SALARY_ID + '-' + s.SALARY_NAME as SALARY_NAME ");
            sb.Append(" ,t2.CHG_AMT_A as CHG_AMT_A ,t2.EFFECT_SDT_B as EFFECT_SDT,t2.EFFECT_EDT_B as EFFECT_EDT,t2.EFFECT_SDT_A as EFFECT_SDT_A,t2.EFFECT_EDT_A as EFFECT_EDT_A ");
            sb.Append(" ,t2.SEQ_NO_B as SEQ_NO_B,t2.SEQ_NO as SEQ_NO,t2.CHG_AMT_B as CHG_AMT_B ");
            sb.Append(" ,t2.PROCESS_STATUS as PROCESS_STATUS  ,t2.PROCESS_STATUS+ '-'+  d.SUB_DESC as DESC1,t2.CHG_STATUS as CHG_STATUS ");
            sb.Append(" ,t2.CHG_STATUS+ '-'+  p.SUB_DESC as DESC2,t2.APPROVE_DT as APPROVE_DT  ,t2.APPROVE_BY as APPROVE_BY,t2.REMARK as REMARK ");
            sb.Append(" ,t2.APP_REMARK as APP_REMARK ");
            sb.Append(" from TB_S_M_SALARY_TXN_TMP t2 ");
            sb.Append("     left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID ");
            sb.Append("     left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append("     left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD ");
            sb.Append(" where 1=1 and t2.PROCESS_STATUS <>'Y' and t2.EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            if (SALARY_ID != "")
            {
                sb.Append(" and t2.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", SALARY_ID);
            }
           
            if (EFFECT_SDT_B != "")
            {
                sb.Append(" and t2.EFFECT_SDT_B = @EFFECT_SDT_B ");
                ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            }

            if (SEQ_NO != "")
            {
                sb.Append(" and t2.SEQ_NO = SEQ_NO ");
                ht.Add("@SEQ_NO", SEQ_NO);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int checkSALARY_TXN_duplicate(string emp_id, string salary_id,string start_dt, string end_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_TXN t1 ");
            sb.Append(" where 1=1 and t1.EMP_ID =@EMP_ID ");
            sb.Append(" and t1.SALARY_ID = @SALARY_ID ");
            sb.Append(" and t1.EFFECT_EDT <> '9999/12/31' ");
            sb.Append(" and ((t1.EFFECT_SDT <= @EFFECT_SDT and t1.EFFECT_EDT >= @EFFECT_SDT) ");
            sb.Append("     or (t1.EFFECT_SDT >= @EFFECT_EDT and t1.EFFECT_EDT <= @EFFECT_EDT)) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_ID", salary_id);
            ht.Add("@EFFECT_SDT", start_dt);
            ht.Add("@EFFECT_EDT", end_dt);


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
    public int checkSALARY_TXN_duplicate_update()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_TXN t1 ");
            sb.Append(" where 1=1 and t1.EMP_ID =@EMP_ID ");
            sb.Append(" and t1.SALARY_ID = @SALARY_ID ");
            sb.Append(" and t1.EFFECT_SDT = @EFFECT_SDT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT_A);

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
    public void insertSALARY_TXN_TMP()
    {
        try
        {
            //新增 敘薪資料暫存檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //新增時,先找前一筆的金額,沒有就帶0 modify by Terry 20160115
            sb.AppendLine(@" declare @before_money int ;
                         select @before_money  = COUNT(*)  from TB_S_M_SALARY_TXN where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID 
                         and EFFECT_EDT='9999/12/31' ; 
                         if(@before_money > 0)
                         select @before_money = isnull(AMOUNT,0) from TB_S_M_SALARY_TXN where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID 
                         and EFFECT_EDT='9999/12/31' order by UPDATED_DT desc; 
                         --else set @before_money = 0;
                        else begin
						    select @before_money  = isnull(AMOUNT,0) from TB_S_M_SALARY_TXN where EMP_ID=@EMP_ID  and SALARY_ID=@SALARY_ID 
						    and EFFECT_EDT = (select MAX(EFFECT_EDT) from TB_S_M_SALARY_TXN where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID);                          
						 end
                         ");

            sb.Append("Insert into TB_S_M_SALARY_TXN_TMP (EMP_ID,SALARY_ID,EFFECT_SDT_B,EFFECT_EDT_B, ");
            sb.Append(" EFFECT_SDT_A,EFFECT_EDT_A,SEQ_NO,SEQ_NO_B,CHG_AMT_B,CHG_AMT_A,CHG_STATUS,PROCESS_STATUS, ");
            sb.Append(" APPROVE_BY,APPROVE_DT,REMARK,APP_REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@SALARY_ID,@EFFECT_SDT_B,@EFFECT_EDT_B,@EFFECT_SDT_A,@EFFECT_EDT_A, ");
            sb.Append(" (select isnull(max(SEQ_NO),0) + 1 from TB_S_M_SALARY_TXN_TMP where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and EFFECT_SDT_B = @EFFECT_SDT_B),");
            sb.Append(" @SEQ_NO_B,@before_money,@CHG_AMT_A,@CHG_STATUS, @PROCESS_STATUS,'',null,@REMARK,'',@CREATED_BY,GETDATE(),@CREATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            ht.Add("@EFFECT_EDT_B", (EFFECT_EDT_B == "" ? DBNull.Value.ToString() : EFFECT_EDT_B));
            ht.Add("@EFFECT_SDT_A", EFFECT_SDT_A);
            ht.Add("@EFFECT_EDT_A", EFFECT_EDT_A);
            ht.Add("@SEQ_NO_B", SEQ_NO_B);
            //ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            ht.Add("@REMARK", REMARK);

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateSALARY_TXN_TMP()
    {
        try
        {
            //修改 敘薪資料暫存檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_TXN_TMP ");
            sb.Append(" set EFFECT_SDT_A=@EFFECT_SDT_A,EFFECT_EDT_A = @EFFECT_EDT_A,CHG_AMT_A=@CHG_AMT_A,REMARK=@REMARK,PROCESS_STATUS=@PROCESS_STATUS ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT=GETDATE() ");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID and EFFECT_SDT_B=@EFFECT_SDT_B and SEQ_NO=@SEQ_NO");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@EFFECT_SDT_A", EFFECT_SDT_A);
            ht.Add("@EFFECT_EDT_A", EFFECT_EDT_A);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@REMARK", REMARK);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS);

            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteSALARY_TXN_TMP()
    {
        try
        {
            //刪除 敘薪資料暫存檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  delete  from TB_S_M_SALARY_TXN_TMP         ");
            sb.Append("  where EMP_ID =@EMP_ID and SALARY_ID = @SALARY_ID and  EFFECT_SDT_B  = @EFFECT_SDT_B and  SEQ_NO = @SEQ_NO ");  

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            ht.Add("@SEQ_NO", SEQ_NO);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

}