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
/// CFB2SC2300DAO 的摘要描述
/// </summary>
public class CFB2SC2300DAO : BaseDAO
{
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string SALARY_ID { get; set; }
    public string PAY_KIND { get; set; }
    public string SEQ_NO { get; set; }
    public string REMARK { get; set; }
    public string CHG_AMT_B { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APP_REMARK { get; set; }
    public string CFN_PAY { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public int temp_row { get; set; }

    public CFB2SC2300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region "Initial Page"
    public DataTable getSALARY_DT_By_Fn(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_DT(@p_salary_type) as SALARY_DT ");
            ht.Add("@p_salary_type", salary_type);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.AppendLine(" from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = @SYS_CD ");
            sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "")
            {
                sb.AppendLine(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCOMPANY_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COMPANY_CD ,COMPANY_CD+'-'+COMPANY_SNAME as COMPANY_SNAME ");
            sb.AppendLine(" from TB_H_M_COMPANY ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSALARY_ID(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable paykind(string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select SALARY_ID,SALARY_NAME From VW_SALARYAND9999");
        sb.Append(" where SALARY_ID=@PAY_KIND");
        ht.Add("@PAY_KIND", PAY_KIND);
        return dbConn.Query(sb, ht);

    }
    #endregion

    #region "grid1 "
    public DataTable getDataA(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string data_ym
                            , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay, string salary_id)
    {
        try
        {
             //月薪查詢
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("exec SP_SC230_SELECT  @salary_dt,@salary_type,@data_ym,@company_cd,@salary_id, @emp_id, @emp_name ,@cfn_pay,@pay_kind,@startRowIndex, @maximumRows,@sortExpression ; ");
            sb.Append(" select * from TB_S_S_SC230_TEMP; ");     
            
            ht.Add("@salary_dt", salary_dt);
            ht.Add("@salary_type", salary_type);
            ht.Add("@data_ym", data_ym);
            ht.Add("@company_cd", company_cd);
            ht.Add("@salary_id", salary_id);
            ht.Add("@emp_id", emp_id);
            ht.Add("@emp_name", emp_name);
            ht.Add("@cfn_pay", cfn_pay);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@sortExpression", sortExpression);

            DataTable dt = dbConn.Query(sb, ht);
            temp_row = dt.Rows.Count;

            return dt;


/*
            sb.AppendLine(@" Select * from "                                                                                                   
                           + "  (select ROW_NUMBER() OVER(ORDER BY "+ sortExpression+" ) As RowNumber,*    ");                                  
             sb.AppendLine(@"                 
                            from (                                                                                                           
                               select distinct t2.SALARY_DT, t2.salary_ym as DATA_YM, 'A' as SALARY_TYPE, t2.EMP_ID, t2.EMP_NAME, t2.COMPANY_CD, t2.EMP_CD
                                     ,t1.CFN_PAY  
                                     ,'A-月薪類' as SALARY_TYPE_DESC                                                      
		                                ,t2.COMPANY_CD +'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                     
		                                ,t2.EMP_CD +'-'+d.SUB_DESC as EMP_CD_DESC
		                                , '9999' as PAY_KIND                                       
		                                ,'9999-月薪' as PAY_KIND_DESC                                                        
		                                ,t2.JOIN_DT ,t2.LEAVE_DT 
		                                ,isnull(tr.AMOUNT,0) AMOUNT
		                                , t1.PAY_ID as PAY_ID                     
	                                    ,CONVERT(varchar(100), t2.SALARY_DT, 111) + 'A'+ t2.EMP_ID + '9999' as qdatakey                       
                                from (
										select * from TB_S_M_EMP_RESULT where SALARY_DT = @SALARY_DT
									 ) t2
                                left join [dbo].[FN_S_TABLE_SALARY_PAY_TOTAL](@SALARY_DT,@SALARY_TYPE) tr on t2.emp_id=tr.emp_id
                                left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                
                                left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                    
                                left join( select distinct p1.SALARY_DT,p1.EMP_ID,p1.CFN_PAY ,p1.PAY_ID,p1.pay_kind,SALARY_ID from TB_S_S_SALARY_PAY p1  
                                            where CONVERT(varchar(100), p1.SALARY_DT , 111) = @SALARY_DT and p1.SALARY_TYPE = @SALARY_TYPE 
                                             ) t1 on t2.SALARY_DT =t1.SALARY_DT and t2.EMP_ID =t1.EMP_ID  
                               where 1=1 and  CONVERT(varchar(100), t2.SALARY_DT , 111) = @SALARY_DT
            ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            if (data_ym != "")
            {
                sb.AppendLine(" and t2.SALARY_YM = @DATA_YM ");
                ht.Add("@DATA_YM", data_ym.Replace("/", ""));
            }
            if (company_cd != "")
            {
                sb.AppendLine(" and t2.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (pay_kind != "")
            {
                sb.AppendLine(" and t1.PAY_KIND = @PAY_KIND ");
                ht.Add("@PAY_KIND", pay_kind);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and t2.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (cfn_pay != "")
            {
                sb.AppendLine(" and t1.CFN_PAY = @CFN_PAY ");
                ht.Add("@CFN_PAY", cfn_pay);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and t1.SALARY_ID = @SALARY_ID ");              
                ht.Add("@SALARY_ID", salary_id);
            }

            //if (salary_id != "")
            //{
            //    sb.AppendLine(" and exists (select EMP_ID from TB_S_S_SALARY_PAY t3 ");
            //     sb.AppendLine(" where t3.SALARY_TYPE='A' and t1.EMP_ID = t3.EMP_ID ");
            //     sb.AppendLine("  and t1.PAY_KIND = t3.PAY_KIND and t1.SALARY_DT = t3.SALARY_DT and t3.SALARY_ID = @SALARY_ID ) ");
            //    ht.Add("@SALARY_ID", salary_id);
            //}
            sb.AppendLine(" )A                                                                      ");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

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
    public DataTable getDataExceptA(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string data_ym
                           , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay, string salary_id)
    {
        try
        {
         
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * from                                                                                                ");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,*                                    ");
            sb.AppendLine(" from (                                                                                                           ");
            sb.AppendLine("  select  t1.SALARY_DT, t1.DATA_YM, t1.SALARY_TYPE, t1.EMP_ID, t2.EMP_NAME, t2.COMPANY_CD, t2.EMP_CD, t1.CFN_PAY           ");
            sb.AppendLine("         ,t1.SALARY_TYPE+'-'+d2.SUB_DESC as SALARY_TYPE_DESC                                                  ");
            sb.AppendLine("		    ,t2.COMPANY_CD +'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                 ");
            sb.AppendLine("		    ,t2.EMP_CD +'-'+d.SUB_DESC as EMP_CD_DESC, t1.PAY_KIND as PAY_KIND                                  ");
            sb.AppendLine("		    ,t1.PAY_KIND +'-'+ p.SALARY_NAME as PAY_KIND_DESC                                 ");
            sb.AppendLine("		    ,e.JOIN_DT ,e.LEAVE_DT ,SUM(t1.AMOUNT * t1.IS_PLUS) as AMOUNT,t1.PAY_ID as PAY_ID                    ");
            sb.AppendLine("	        ,CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.PAY_KIND as qdatakey       ");
            sb.AppendLine("    from TB_S_S_SALARY_PAY t1                                                                                 ");
            sb.AppendLine("    left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT and t1.EMP_ID = t2.EMP_ID and t1.SALARY_TYPE = t2.SALARY_TYPE and t1.PAY_KIND = t2.PAY_KIND ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD            ");
            sb.AppendLine("    left join TB_9_M_COMM_D d2 on d2.SYS_CD ='SC' and d2.MAIN_CD='SALARY_TYPE' and t1.SALARY_TYPE = d2.SUB_CD ");
            sb.AppendLine("    left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                ");
            sb.AppendLine("    left join VW_H_EMP_DATA e on e.EMP_ID = t1.EMP_ID                                                         ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM s on  t1.SALARY_ID =s.SALARY_ID                                              ");
            sb.AppendLine("    left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                              ");
            sb.AppendLine("   where 1=1 and  CONVERT(varchar(100), t1.SALARY_DT , 111) = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE    ");
            sb.AppendLine("     and s.PAY_OBJECT = 'E'                                                                                   ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            if (data_ym != "")
            {
                sb.AppendLine(" and t1.DATA_YM = @DATA_YM ");
                ht.Add("@DATA_YM", data_ym.Replace("/", ""));
            }
            if (company_cd != "")
            {
                sb.AppendLine(" and t2.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (pay_kind != "")
            {
                sb.AppendLine(" and t1.PAY_KIND = @PAY_KIND ");
                ht.Add("@PAY_KIND", pay_kind);
            }
            if (salary_name != "")
            {
                sb.AppendLine(" and t1.SALARY_NAME like '%'+ @SALARY_NAME +'%' ");
                ht.Add("@SALARY_NAME", salary_name);
            }
            if (emp_id != "")
            {
                //sb.AppendLine(" and t1.EMP_ID like '%'+ @EMP_ID +'%' ");
                sb.AppendLine(" and t1.EMP_ID =  @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (cfn_pay != "")
            {
                sb.AppendLine(" and t1.CFN_PAY = @CFN_PAY ");
                ht.Add("@CFN_PAY", cfn_pay);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and exists (select EMP_ID from TB_S_S_SALARY_PAY t3 ");
                sb.AppendLine(" where t1.SALARY_TYPE = t3.SALARY_TYPE and t1.EMP_ID = t3.EMP_ID ");
                sb.AppendLine("  and t1.PAY_KIND = t3.PAY_KIND and t1.SALARY_DT = t3.SALARY_DT and t3.SALARY_ID = @SALARY_ID ) ");
                ht.Add("@SALARY_ID", salary_id);
            }
            sb.AppendLine("      Group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t2.EMP_NAME,t2.COMPANY_CD        ");
            sb.AppendLine("	    		,c.COMPANY_SNAME,e.JOIN_DT ,e.LEAVE_DT ,t2.EMP_CD,d.SUB_DESC,d2.SUB_DESC,t1.CFN_PAY,t1.PAY_ID,t1.PAY_KIND,t1.DATA_YM,p.SALARY_NAME ");
            sb.AppendLine(" )A                                                                      ");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
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
    public int getCountA(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string data_ym
                           , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay, string salary_id)
    {
        try
        {
            return temp_row;
            //月薪類
 /*           int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SC230_SELECT");
            ht.Add("@p_SALARY_DT", salary_dt);
            ht.Add("@p_SALARY_TYPE", salary_type);
            ht.Add("@p_DATA_YM", data_ym);
            ht.Add("@p_COMPANY_CD", company_cd);
            ht.Add("@p_SALARY_ID", salary_id);
            ht.Add("@p_EMP_ID", emp_id);
            ht.Add("@p_EMP_NAME", emp_name);
            ht.Add("@p_CFN_PAY", cfn_pay);
            ht.Add("@p_PAY_KIND", pay_kind);
            ht.Add("@p_startRowIndex", startRowIndex);
            ht.Add("@p_maximumRows", maximumRows);
            ht.Add("@p_sortExpression", "EMP_ID");

            DataTable dt = dbConn.QuerySP(sb, ht, true);
            int i = dt.Rows.Count;
            return dt.Rows.Count;
*/
/*            

            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine(" from ( ");
            sb.AppendLine(@" select distinct t2.SALARY_DT, t2.salary_ym as DATA_YM, 'A' as SALARY_TYPE, t2.EMP_ID, t2.EMP_NAME, t2.COMPANY_CD, t2.EMP_CD
                                    ,t1.CFN_PAY  
                            from (
										select * from TB_S_M_EMP_RESULT where SALARY_DT = @SALARY_DT
								 ) t2
                            left join [dbo].[FN_S_TABLE_SALARY_PAY_TOTAL](@SALARY_DT,@SALARY_TYPE) tr on t2.emp_id=tr.emp_id  
                            left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                
                            left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                    
                            left join( select distinct p1.SALARY_DT,p1.EMP_ID,p1.CFN_PAY ,p1.PAY_ID,p1.pay_kind,SALARY_ID from TB_S_S_SALARY_PAY p1  
                                        where CONVERT(varchar(100), p1.SALARY_DT , 111) = @SALARY_DT and p1.SALARY_TYPE = @SALARY_TYPE
                                            ) t1 on t2.SALARY_DT =t1.SALARY_DT and t2.EMP_ID =t1.EMP_ID  
                            where 1=1 and  CONVERT(varchar(100), t2.SALARY_DT , 111) = @SALARY_DT 
           ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            if (data_ym != "")
            {
                sb.AppendLine(" and t2.SALARY_YM = @DATA_YM ");
                ht.Add("@DATA_YM", data_ym.Replace("/", ""));
            }
            if (company_cd != "")
            {
                sb.AppendLine(" and t2.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (pay_kind != "")
            {
                sb.AppendLine(" and t1.PAY_KIND = @PAY_KIND ");
                ht.Add("@PAY_KIND", pay_kind);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and t2.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (cfn_pay != "")
            {
                sb.AppendLine(" and t1.CFN_PAY = @CFN_PAY ");
                ht.Add("@CFN_PAY", cfn_pay);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and t1.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            //if (salary_id != "")
            //{
            //    sb.AppendLine(" and exists (select EMP_ID from TB_S_S_SALARY_PAY t3 ");
            //    sb.AppendLine(" where t3.SALARY_TYPE='A' and t1.EMP_ID = t3.EMP_ID ");
            //    sb.AppendLine("  and t1.PAY_KIND = t3.PAY_KIND and t1.SALARY_DT = t3.SALARY_DT and t3.SALARY_ID = @SALARY_ID ) ");
            //    ht.Add("@SALARY_ID", salary_id);
            //}
            sb.AppendLine(" )a ");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
 */
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getCountExceptA(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string data_ym
                          , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay, string salary_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine(" from ( ");
            sb.AppendLine("  select t1.SALARY_DT, t1.DATA_YM, t1.SALARY_TYPE, t1.EMP_ID, t2.EMP_NAME, t2.COMPANY_CD, t2.EMP_CD, t1.CFN_PAY           ");
            sb.AppendLine("         ,t1.SALARY_TYPE+'-'+d2.SUB_DESC as SALARY_TYPE_DESC                                                  ");
            sb.AppendLine("		    ,t2.COMPANY_CD +'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                 ");
            sb.AppendLine("		    ,t2.EMP_CD +'-'+d.SUB_DESC as EMP_CD_DESC, t1.PAY_KIND as PAY_KIND                                   ");
            sb.AppendLine("		    ,t1.PAY_KIND +'-'+ p.SALARY_NAME as PAY_KIND_DESC                                 ");
            sb.AppendLine("		    ,e.JOIN_DT ,e.LEAVE_DT ,SUM(t1.AMOUNT * t1.IS_PLUS) as AMOUNT,t1.PAY_ID as PAY_ID                    ");
            sb.AppendLine("	        ,CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID as qdatakey                   ");
            sb.AppendLine("    from TB_S_S_SALARY_PAY t1                                                                                 ");
            sb.AppendLine("    left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT and t1.EMP_ID = t2.EMP_ID and t1.SALARY_TYPE = t2.SALARY_TYPE and t1.PAY_KIND = t2.PAY_KIND  ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD            ");
            sb.AppendLine("    left join TB_9_M_COMM_D d2 on d2.SYS_CD ='SC' and d2.MAIN_CD='SALARY_TYPE' and t1.SALARY_TYPE = d2.SUB_CD ");
            sb.AppendLine("    left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                ");
            sb.AppendLine("    left join VW_H_EMP_DATA e on e.EMP_ID = t1.EMP_ID                                                         ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM s on  t1.SALARY_ID =s.SALARY_ID                                              ");
            sb.AppendLine("    left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                              ");
            sb.AppendLine("   where 1=1 and  CONVERT(varchar(100), t1.SALARY_DT , 111) = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE    ");
            sb.AppendLine("     and s.PAY_OBJECT = 'E'                                                                                   ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            if (data_ym != "")
            {
                sb.AppendLine(" and t1.DATA_YM = @DATA_YM ");
                ht.Add("@DATA_YM", data_ym.Replace("/", ""));
            }
            if (company_cd != "")
            {
                sb.AppendLine(" and t2.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (pay_kind != "")
            {
                sb.AppendLine(" and t1.PAY_KIND = @PAY_KIND ");
                ht.Add("@PAY_KIND", pay_kind);
            }
            if (salary_name != "")
            {
                sb.AppendLine(" and t1.SALARY_NAME like '%'+ @SALARY_NAME +'%' ");
                ht.Add("@SALARY_NAME", salary_name);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and t1.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (cfn_pay != "")
            {
                sb.AppendLine(" and t1.CFN_PAY = @CFN_PAY ");
                ht.Add("@CFN_PAY", cfn_pay);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and exists (select EMP_ID from TB_S_S_SALARY_PAY t3 ");
                sb.AppendLine(" where t1.SALARY_TYPE = t3.SALARY_TYPE and t1.EMP_ID = t3.EMP_ID ");
                sb.AppendLine("  and t1.PAY_KIND = t3.PAY_KIND and t1.SALARY_DT = t3.SALARY_DT and t3.SALARY_ID = @SALARY_ID ) ");
                ht.Add("@SALARY_ID", salary_id);
            }
            sb.AppendLine("      Group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t2.EMP_NAME,t2.COMPANY_CD        ");
            sb.AppendLine("	    		,c.COMPANY_SNAME,e.JOIN_DT ,e.LEAVE_DT ,t2.EMP_CD,d.SUB_DESC,d2.SUB_DESC,t1.CFN_PAY,t1.PAY_ID,t1.PAY_KIND,t1.DATA_YM,p.SALARY_NAME ");
            sb.AppendLine(" )a ");
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

    public string delete_ofDelete_Data(CFB2SC2300DAO fb2sc)
    {
        try
        {
            //刪除薪資明細計算暫存檔 TB_S_S_SALARY_PAY_TMP
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" delete from TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID = @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    public string delete_ofAdd_Data(CFB2SC2300DAO fb2sc)
    {
        try
        {
            //新增 薪資明細計算暫存檔(TB_S_S_SALARY_PAY_TMP)
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", "0");
            ht.Add("@CHG_STATUS", "D");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "Excute"

    /// <summary>
    /// 檢查TB_S_S_SALARY_PAY_TMP 有無相同異動狀況的資料，資料存在回傳true，資料不存在回傳false
    /// </summary>
    /// <param name="checkkey"></param>
    /// <param name="chg_status"></param>
    /// <returns></returns>
    public bool checkIsRepeat_CHH_STATUS(string checkkey,string chg_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP ");
            //sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT, 111) + SALARY_TYPE + EMP_ID + PAY_KIND = @QDATAKEY ");
            sb.AppendLine("  where 1=1 ");
            sb.AppendLine(@"    and SALARY_DT = substring(@QDATAKEY,1,10)
                                and SALARY_TYPE = substring(@QDATAKEY,11,1) 
                                and EMP_ID = substring(@QDATAKEY,12,5)   
                                and PAY_KIND =  substring(@QDATAKEY,17,4)   
                                ");

            sb.AppendLine("  and CHG_STATUS = @CHG_STATUS and PROCESS_STATUS in ('Y','N') ");
            ht.Add("@QDATAKEY", checkkey);
            ht.Add("@CHG_STATUS", chg_status);
            DataTable dt = dbConn.Query(sb, ht, true);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getExcuteData1And2(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select distinct t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.SALARY_NAME,t1.PAY_KIND,t1.DATA_YM  ");
            sb.AppendLine(" 	            ,t1.IS_PLUS,t1.IS_TAX,t3.SEQ_NO,t3.REMARK                                                 ");
            sb.AppendLine(" 	            ,CASE WHEN t3.CHG_AMT_B is not null then t3.CHG_AMT_B else t1.AMOUNT end as CHG_AMT_B     ");
            sb.AppendLine("                 ,CASE WHEN t3.CHG_AMT_A is not null then t3.CHG_AMT_A else 0 end as CHG_AMT_A             ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                               ");
            sb.AppendLine("   left join TB_S_S_SALARY_PAY_TMP t3                                                                      ");
            sb.AppendLine("     on t1.SALARY_DT = t3.SALARY_DT and t1.SALARY_TYPE = t3.SALARY_TYPE and t1.EMP_ID = t3.EMP_ID          ");
            sb.AppendLine("    and t1.SALARY_ID = t3.SALARY_ID and t1.PAY_KIND = t3.PAY_KIND and t3.PROCESS_STATUS<>'Y'               ");
            //sb.AppendLine("  where 1=1 and  CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.PAY_KIND = @QDATAKEY         ");
            sb.AppendLine("  where 1=1 ");
            sb.AppendLine(@"    and t1.SALARY_DT = substring(@QDATAKEY,1,10)
                                and t1.SALARY_TYPE = substring(@QDATAKEY,11,1) 
                                and t1.EMP_ID = substring(@QDATAKEY,12,5)   
                                and t1.PAY_KIND =  substring(@QDATAKEY,17,4)   
                                ");
            sb.AppendLine("  Order By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID                                              ");
            ht.Add("@QDATAKEY", deleteitem);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    #region "WFB2SC2300Execute1 - 暫不發薪"
    //更新
    public string Execute1_SEQ_NO_isNotNULL_Data(CFB2SC2300DAO fb2sc, string txt_remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine(" set CHG_STATUS = @CHG_STATUS,PROCESS_STATUS = @PROCESS_STATUS ,APPROVE_DT = @APPROVE_DT ");
            sb.AppendLine("     ,APPROVE_BY = @APPROVE_BY, REMARK = @REMARK, APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID = @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CHG_STATUS", "C");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK + ";" + txt_remark);
            ht.Add("@APP_REMARK", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    //新增
    public string Execute1_SEQ_NO_isNULL_Data(CFB2SC2300DAO fb2sc, string txt_remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", "C");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", txt_remark);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "WFB2SC2300Execute2 - 確定發薪"
    //更新
    public string Execute2_SEQ_NO_isNotNULL_Data(CFB2SC2300DAO fb2sc, string txt_remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine(" set CHG_STATUS = @CHG_STATUS,PROCESS_STATUS = @PROCESS_STATUS ,APPROVE_DT = @APPROVE_DT ");
            sb.AppendLine("     ,APPROVE_BY = @APPROVE_BY, REMARK = @REMARK, APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID = @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CHG_STATUS", "R");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK + ";" + txt_remark);
            ht.Add("@APP_REMARK", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    //新增
    public string Execute2_SEQ_NO_isNULL_Data(CFB2SC2300DAO fb2sc, string txt_remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", "0");
            ht.Add("@CHG_STATUS", "R");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", txt_remark);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "WFB2SC2300Execute3 & 4 - 轉積欠代墊 & 離職轉所得"
    //找到第一次發薪日期
    public DataTable checkSecondSalary()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top 1 convert(varchar,SALARY_DT,111) SALARY_DT from TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM and SALARY_TYPE = 'A' and PAY_KIND = '9999' ");

            ht.Add("@SALARY_YM", DATA_YM);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getExcuteData3(string executeitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND,SUM(t1.IS_PLUS*t1.AMOUNT ) as AMT                             ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                                                         ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID =s.SALARY_ID                                                                       ");
            sb.AppendLine("  where 1=1 and CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.PAY_KIND = @QDATAKEY and s.PAY_OBJECT ='E' ");
            sb.AppendLine("    and convert(varchar(8),t1.SALARY_DT,111)+ t1.DATA_YM+ t1.SALARY_TYPE+ t1.EMP_ID+ t1.SALARY_ID+ t1.PAY_KIND                       ");
            sb.AppendLine("        not in (                                                                                                                     ");
            sb.AppendLine("                select convert(varchar(8),SALARY_DT,112)+DATA_YM+SALARY_TYPE+EMP_ID+SALARY_ID+PAY_KIND                               ");
            sb.AppendLine("                  from  TB_S_S_SALARY_PAY_TMP                                                                                        ");
            sb.AppendLine("                 where CONVERT(varchar(100), SALARY_DT, 111) + SALARY_TYPE + EMP_ID + PAY_KIND= @QDATAKEY                            ");
            sb.AppendLine("                   and s.PAY_OBJECT ='E' and PROCESS_STATUS<>'Y'                                                                     ");
            sb.AppendLine("               )                                                                                                                     ");
            sb.AppendLine("  group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND                                                            ");
            sb.AppendLine("UNION                                                                                                                                ");
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND,SUM(t1.CHG_AMT_A*s.IS_PLUS) as AMT                            ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP t1                                                                                                     ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID =s.SALARY_ID                                                                       ");
            sb.AppendLine("  where 1=1 and t1.PROCESS_STATUS<>'Y' and t1.CHG_STATUS <> 'D' and s.PAY_OBJECT ='E'                                                ");
            //sb.AppendLine("    and CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID +t1.PAY_KIND = @QDATAKEY                               ");
            sb.AppendLine(@"    and t1.SALARY_DT = substring(@QDATAKEY,1,10)
                                and t1.SALARY_TYPE = substring(@QDATAKEY,11,1) 
                                and t1.EMP_ID = substring(@QDATAKEY,12,5)   
                                and t1.PAY_KIND =  substring(@QDATAKEY,17,4)   
                                ");
            sb.AppendLine("  group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND                                                            ");
            ht.Add("@QDATAKEY", executeitem);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getExcuteData4(string executeitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND,SUM(t1.IS_PLUS*t1.AMOUNT ) as AMT                             ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                                                         ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID =s.SALARY_ID                                                                       ");
            sb.AppendLine("  where 1=1 and CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.PAY_KIND = @QDATAKEY and s.PAY_OBJECT ='E' ");
            sb.AppendLine("    and convert(varchar(8),t1.SALARY_DT,111)+ t1.DATA_YM+ t1.SALARY_TYPE+ t1.EMP_ID+ t1.SALARY_ID+ t1.PAY_KIND                       ");
            sb.AppendLine("        not in (                                                                                                                     ");
            sb.AppendLine("                select convert(varchar(8),SALARY_DT,112)+DATA_YM+SALARY_TYPE+EMP_ID+SALARY_ID+PAY_KIND                               ");
            sb.AppendLine("                  from  TB_S_S_SALARY_PAY_TMP                                                                                        ");
            sb.AppendLine("                 where CONVERT(varchar(100), SALARY_DT, 111) + SALARY_TYPE + EMP_ID + PAY_KIND = @QDATAKEY                           ");
            sb.AppendLine("                   and s.PAY_OBJECT ='E' and PROCESS_STATUS<>'Y'                                                                     ");
            sb.AppendLine("               )                                                                                                                     ");
            sb.AppendLine("  group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND                                                            ");
            sb.AppendLine("UNION                                                                                                                                ");
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND,SUM(t1.CHG_AMT_A*s.IS_PLUS ) as AMT                           ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP t1                                                                                                     ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID =s.SALARY_ID                                                                       ");
            sb.AppendLine("  where 1=1 and t1.PROCESS_STATUS<>'Y' and t1.CHG_STATUS <> 'D' and s.PAY_OBJECT ='E'                                                ");
            //sb.AppendLine("    and CONVERT(varchar(100), t1.SALARY_DT, 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.PAY_KIND= @QDATAKEY                               ");
            sb.AppendLine(@"    and t1.SALARY_DT = substring(@QDATAKEY,1,10)
                                and t1.SALARY_TYPE = substring(@QDATAKEY,11,1) 
                                and t1.EMP_ID = substring(@QDATAKEY,12,5)   
                                and t1.PAY_KIND =  substring(@QDATAKEY,17,4)   
                                ");
            sb.AppendLine("  group By t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID,t1.PAY_KIND                                                            ");
            ht.Add("@QDATAKEY", executeitem);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //新增
    public string Execute3_add_Data(CFB2SC2300DAO fb2sc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", "1041");
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", "0");
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", "A");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    //新增
    public string Execute4_add_Data(CFB2SC2300DAO fb2sc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", "2001");
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", "0");
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", "B");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #endregion

    #region "grid 2"
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string emp_id,string pay_kind)
    {
        try
        {
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "A.SALARY_ID");
            if (sortExpression.Contains("CHG_AMT_B"))
                sortExpression = sortExpression.Replace("CHG_AMT_B", "A.CHG_AMT_B");
            if (sortExpression.Contains("CHG_AMT_A"))
                sortExpression = sortExpression.Replace("CHG_AMT_A", "A.CHG_AMT_A");
            if (sortExpression.Contains("DATA_SRC"))
                sortExpression = sortExpression.Replace("DATA_SRC", "A.DATA_SRC");
            if (sortExpression.Contains("IS_PLUS"))
                sortExpression = sortExpression.Replace("IS_PLUS", "A.IS_PLUS");
            if (sortExpression.Contains("IS_TAX"))
                sortExpression = sortExpression.Replace("IS_TAX", "A.IS_TAX");
            if (sortExpression.Contains("CFN_PAY"))
                sortExpression = sortExpression.Replace("CFN_PAY", "A.CFN_PAY");
            if (sortExpression == "")
            {
                sortExpression = "A.SALARY_DT,A.SALARY_TYPE,A.EMP_ID,A.SALARY_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * from                                                                                                        ");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                            ");
            sb.AppendLine("   A.SALARY_DT, A.SALARY_TYPE,A.EMP_ID, A.SALARY_ID, A.SALARY_NAME, A.CHG_AMT_B, A.DATA_SRC, A.SEQ_NO ,A.DATA_YM      ");
            sb.AppendLine("   , A.IS_PLUS, A.IS_TAX, A.CFN_PAY, A.PROCESS_STATUS,  A.CHG_STATUS                                                  ");
            sb.AppendLine("   , A.CHG_AMT_A, A.REMARK, A.APPROVE_BY,A.APPROVE_BY_NAME, A.APPROVE_DT, A.APP_REMARK, A.EMP_SRC                                       ");
            sb.AppendLine("   , A.SALARY_ID_NAME ,A.PAY_KIND                                                                                     ");
            sb.AppendLine("   , A.DATA_SRC+'-'+ A.DATA_SRC_DESC as DATA_SRC_DESC                                                                 ");
            sb.AppendLine("   , A.PROCESS_STATUS+'-'+ A.PROCESS_STATUS_DESC as PROCESS_STATUS_DESC ,PAY_KIND_DESC                                ");
            sb.AppendLine("   , A.CHG_STATUS_DESC                                                           ");
            sb.AppendLine("   , CONVERT(varchar(100), A.SALARY_DT , 111)+ A.SALARY_TYPE + A.EMP_ID + A.SALARY_ID + A.PAY_KIND as qdatakey2       ");
            sb.AppendLine("    from (                                                                                                            ");
            sb.AppendLine("       SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID, 0 as SEQ_NO ,t1.DATA_YM                              ");                              
            sb.AppendLine("              ,t1.SALARY_NAME as SALARY_NAME,t1.AMOUNT as CHG_AMT_A,t1.DATA_SRC                                              ");
            sb.AppendLine("              ,d.SUB_DESC as DATA_SRC_DESC,t1.IS_TAX,t1.CFN_PAY,'Y' as PROCESS_STATUS                                        ");
            sb.AppendLine("              ,'已生效'  as PROCESS_STATUS_DESC                                                                              ");
            sb.AppendLine("              ,CASE WHEN t1.PAY_KIND ='9999' then '9999-月薪資' else t1.PAY_KIND+'-'+ s.SALARY_NAME end as PAY_KIND_DESC     ");
            sb.AppendLine("              ,CASE WHEN t1.IS_PLUS = '1' then '加項' when t1.IS_PLUS = '-1' then '減項' else '' end as IS_PLUS              ");
            sb.AppendLine("              ,'' as CHG_STATUS,'' CHG_STATUS_DESC, 0 as CHG_AMT_B ,'' as REMARK                                             ");
            sb.AppendLine("              ,'' as APPROVE_BY,'' as APPROVE_BY_NAME,NULL as APPROVE_DT,'' as APP_REMARK ,'一般'  as EMP_SRC ,t1.PAY_KIND ");        
            sb.AppendLine("              ,t1.SALARY_ID+'-'+isnull(t1.SALARY_NAME,'') as SALARY_ID_NAME                                                             ");
            sb.AppendLine("         from TB_S_S_SALARY_PAY t1                                                                                         ");  
            sb.AppendLine("         left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='DATA_SRC' and  t1.DATA_SRC = d.SUB_CD                ");
            sb.AppendLine("         left join VW_SALARYAND9999 s on  t1.PAY_KIND =s.SALARY_ID                                                      ");
            sb.AppendLine("         where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID and t1.PAY_KIND = @PAY_KIND");
            sb.AppendLine("   UNION                                                                                                                            ");
            sb.AppendLine("     select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID, t1.SEQ_NO,t1.DATA_YM                                                ");
            sb.AppendLine("         ,t4.SALARY_NAME as SALARY_NAME,t1.CHG_AMT_A as CHG_AMT_A,'4' as DATA_SRC                                                   ");
            sb.AppendLine("         ,'人工調整' as DATA_SRC_DESC,t4.IS_TAX,'Y' as CFN_PAY,isnull(t1.PROCESS_STATUS,'Y') as PROCESS_STATUS           ");
            sb.AppendLine("         ,CASE WHEN t1.PROCESS_STATUS is null then '已生效' else f.SUB_DESC end as PROCESS_STATUS_DESC                              ");
            sb.AppendLine("         ,CASE WHEN t1.PAY_KIND ='9999' then '9999-月薪資' else t1.PAY_KIND+'-'+ s.SALARY_NAME end as PAY_KIND_DESC                 ");
            sb.AppendLine("         ,CASE WHEN t4.IS_PLUS = '1' then '加項' when t4.IS_PLUS = '-1' then '減項' else '' end as IS_PLUS                          ");
            sb.AppendLine("         ,t1.CHG_STATUS,t1.CHG_STATUS+'-'+e.SUB_DESC as CHG_STATUS_DESC,t1.CHG_AMT_B as CHG_AMT_B ,t1.REMARK                      ");
            sb.AppendLine("         ,t1.APPROVE_BY,t1.APPROVE_BY +'-'+ v.EMP_NAME as APPROVE_BY_NAME ,t1.APPROVE_DT,t1.APP_REMARK ,'一般'  as EMP_SRC ,t1.PAY_KIND ");
            sb.AppendLine("         ,t1.SALARY_ID+'-'+isnull(t4.SALARY_NAME,'') as SALARY_ID_NAME                                                                         ");
            sb.AppendLine("      from TB_S_S_SALARY_PAY_TMP t1                                                                                                 ");
            sb.AppendLine("      left join TB_S_M_SALARY_ITEM t4 on t1.SALARY_ID = t4.SALARY_ID                                                                ");
            sb.AppendLine("      left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='CHG_STATUS' and  t1.CHG_STATUS = e.SUB_CD                        ");
            sb.AppendLine("      left join TB_9_M_COMM_D f on  f.SYS_CD ='SA' and  f.MAIN_CD='PROCESS_STATUS' and  t1.PROCESS_STATUS = f.SUB_CD                ");
            sb.AppendLine("      left join VW_SALARYAND9999 s on  t1.PAY_KIND =s.SALARY_ID                                                                  ");
            sb.AppendLine("      left join VW_H_EMP_DATA v on t1.APPROVE_BY = v.EMP_ID                                                                         ");
            sb.AppendLine("     where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID and t1.PAY_KIND = @PAY_KIND ");
            sb.AppendLine("       and t1.PROCESS_STATUS <> 'Y' ");
            sb.AppendLine("      ) A                                                                                                    ");

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@PAY_KIND", pay_kind);
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public int getCount2(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string emp_id, string pay_kind)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record                               ");
            sb.AppendLine("    from (                                                                                                            ");
            sb.AppendLine("       SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID, 0 as SEQ_NO ,t1.DATA_YM                              ");
            sb.AppendLine("         from TB_S_S_SALARY_PAY t1                                                                                         ");
            sb.AppendLine("         where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID and t1.PAY_KIND = @PAY_KIND");
            sb.AppendLine("   UNION                                                                                                                            ");
            sb.AppendLine("     select t1.SALARY_DT,t1.SALARY_TYPE,t1.EMP_ID,t1.SALARY_ID, t1.SEQ_NO,t1.DATA_YM                                                ");
            sb.AppendLine("      from TB_S_S_SALARY_PAY_TMP t1                                                                                                 ");
            sb.AppendLine("     where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID and t1.PAY_KIND = @PAY_KIND");
            sb.AppendLine("       and t1.PROCESS_STATUS <> 'Y' ");
            sb.AppendLine("      ) A                                                                                                    ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@PAY_KIND", pay_kind);
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
    public string deleteDtl_OFadd()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", "0");
            ht.Add("@CHG_STATUS", "D");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }
    public string deleteDtl_TB_S_S_SALARY_PAY_TMP(string deleteitem, string seq_no)
    {
        try
        {
            //刪除薪資明細計算暫存檔 TB_S_S_SALARY_PAY_TMP
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" delete from TB_S_S_SALARY_PAY_TMP ");
            //sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111)+ SALARY_TYPE + EMP_ID + SALARY_ID + PAY_KIND = @qdatakey2 ");
            sb.AppendLine("  where 1=1 ");
            sb.AppendLine(@"    and SALARY_DT = substring(@qdatakey2,1,10)
                                and SALARY_TYPE = substring(@qdatakey2,11,1) 
                                and EMP_ID = substring(@qdatakey2,12,5)   
                                and SALARY_ID =  substring(@qdatakey2,17,4)   
                                and PAY_KIND = substring(@qdatakey2,21,4)  ");
            sb.AppendLine("    and SEQ_NO = @SEQ_NO ");
            ht.Add("@qdatakey2", deleteitem);
            ht.Add("@SEQ_NO", seq_no);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region "取得序號"
    public DataTable getSEQ_NO(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select MAX(SEQ_NO) as SEQ_NO ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("    and SALARY_TYPE =@SALARY_TYPE ");
            sb.AppendLine("    and DATA_YM =@DATA_YM ");
            sb.AppendLine("    and EMP_ID =@EMP_ID ");
            sb.AppendLine("    and SALARY_ID =@SALARY_ID ");
            sb.AppendLine("    and PAY_KIND =@PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", salary_id);
            ht.Add("@PAY_KIND", PAY_KIND);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSEQ_NO2(string qdatakey2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select MAX(SEQ_NO)as SEQ_NO");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP ");
            //sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111)+ SALARY_TYPE + EMP_ID + SALARY_ID + PAY_KIND = @qdatakey2 ");
            sb.AppendLine("  where 1=1 ");
            sb.AppendLine(@"    and SALARY_DT = substring(@qdatakey2,1,10)
                                and SALARY_TYPE = substring(@qdatakey2,11,1) 
                                and EMP_ID = substring(@qdatakey2,12,5)   
                                and SALARY_ID =  substring(@qdatakey2,17,4)   
                                and PAY_KIND = substring(@qdatakey2,21,4)  ");
            ht.Add("@qdatakey2", qdatakey2);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "grid 2 Add"

    public DataTable getAddInitialData_isA(string salary_dt, string salary_type, string emp_id, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select distinct t1.SALARY_DT,t1.SALARY_TYPE +'-'+ f.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,t2.COMPANY_CD,c.COMPANY_SNAME as COMPANY_SNAME,t2.EMP_CD          ");
            sb.AppendLine("        ,d.SUB_DESC as EMP_CD_DESC,t2.JOIN_DT ,t2.LEAVE_DT , 'N-未生效' as PROCESS_STATUS, '4-人工調整' as DATA_SRC,'N-新增' as CHG_STATUS ");
            sb.AppendLine("        , t1.DATA_YM,t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME,t1.PAY_KIND,t1.PAY_KIND  +'-'+ p.SALARY_NAME as PAY_KIND_DESC,t1.IS_PLUS,t1.IS_TAX  ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                                                         ");
            sb.AppendLine("   left join TB_S_M_EMP_RESULT t2 on t1.DATA_YM = t2.SALARY_YM and t1.EMP_ID = t2.EMP_ID and t1.SALARY_DT = t2.SALARY_DT             ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                                    ");
            sb.AppendLine("   left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                                        ");
            sb.AppendLine("   left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                                                        ");
            sb.AppendLine("   left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                                                        ");
            sb.AppendLine("    left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = f.SUB_CD ");
            sb.AppendLine("  where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID  and t1.PAY_KIND=@PAY_KIND    ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@PAY_KIND", pay_kind);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getAddInitialData_isNotA(string salary_dt, string salary_type, string emp_id, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE  +'-'+f.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,t2.COMPANY_CD,c.COMPANY_SNAME as COMPANY_SNAME,t2.EMP_CD          ");
            sb.AppendLine("        ,d.SUB_DESC as EMP_CD_DESC,v.JOIN_DT ,v.LEAVE_DT , 'N-未生效' as PROCESS_STATUS, '4-人工調整' as DATA_SRC,'N-新增' as CHG_STATUS ");
            sb.AppendLine("        , t1.DATA_YM,t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME,t1.PAY_KIND,t1.PAY_KIND  +'-'+ p.SALARY_NAME as PAY_KIND_DESC,t1.IS_PLUS,t1.IS_TAX  ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                                                         ");
            sb.AppendLine("   left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT and t1.EMP_ID = t2.EMP_ID and t1.SALARY_TYPE = t2.SALARY_TYPE and t1.PAY_KIND = t2.PAY_KIND ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                                    ");
            sb.AppendLine("   left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                                        ");
            sb.AppendLine("   left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                                                        ");
            sb.AppendLine("   left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                                                        ");
            sb.AppendLine("   left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = f.SUB_CD ");
            sb.AppendLine("   left join VW_H_EMP_DATA v on v.EMP_ID = t1.EMP_ID ");                             
            sb.AppendLine("  where 1=1 and  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.EMP_ID = @EMP_ID and t1.PAY_KIND=@PAY_KIND   ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@PAY_KIND", pay_kind);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable checkSALARY_ID(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SALARY_NAME,IS_PLUS,IS_TAX ");
            sb.AppendLine(" from TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(1) as total from TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("    and SALARY_TYPE =@SALARY_TYPE ");
            sb.AppendLine("    and DATA_YM =@DATA_YM ");
            sb.AppendLine("    and EMP_ID =@EMP_ID ");
            sb.AppendLine("    and SALARY_ID =@SALARY_ID ");
            sb.AppendLine("    and PAY_KIND =@PAY_KIND ");
            sb.AppendLine("    and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            DataTable dt = dbConn.Query(sb, ht);
            return Convert.ToInt32(dt.Rows[0]["total"]);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void addDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, APP_REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @APP_REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", "0");
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", "N");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "grid 2 Mod"
    public DataTable getModInitialData_PROCESS_STATUS_isY_A(string dtldatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE  +'-'+ f.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,t2.JOIN_DT ,t2.LEAVE_DT,t1.IS_PLUS,t1.IS_TAX       ");
            sb.AppendLine("      ,t1.DATA_SRC,t1.AMOUNT as CHG_AMT_B, t1.AMOUNT as CHG_AMT_A                                            ");
            sb.AppendLine("      ,t1.CREATED_BY,t1.CREATED_DT                                                                           ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+t1.SALARY_NAME as SALARY_NAME                                                       ");
            sb.AppendLine("      ,t2.COMPANY_CD+'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                    ");
            sb.AppendLine("      ,t2.EMP_CD +'-'+ d.SUB_DESC as EMP_CD_DESC                                                             ");
            sb.AppendLine("      ,t1.DATA_SRC+'-'+e.SUB_DESC as DATA_SRC_DESC                                                           ");
            sb.AppendLine("      , 'N-未生效' as PROCESS_STATUS                                                                         ");
            sb.AppendLine("      , 'U-修改'  as CHG_STATUS                                                                              ");
            sb.AppendLine("      , 'N' as PROCESS_STATUS                                                                                ");
            sb.AppendLine("      , 'U'  as CHG_STATUS                                                                                   ");
            sb.AppendLine("      , '' as APPROVE_DT, '' as APPROVE_BY, '' as REMARK, '' as APP_REMARK                                   ");
            sb.AppendLine("      ,t1.PAY_KIND,t1.PAY_KIND  +'-'+ p.SALARY_NAME as PAY_KIND_DESC, t1.DATA_YM                                                                               ");
            sb.AppendLine("      ,t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME                                                          ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+ item.SALARY_NAME as SALARY_NAME,t1.SALARY_ID  ,'0' as SEQ_NO                       ");
            sb.AppendLine("    from TB_S_S_SALARY_PAY t1                                                                                ");
            sb.AppendLine("    left join TB_S_M_EMP_RESULT t2 on t1.DATA_YM = t2.SALARY_YM and t1.EMP_ID = t2.EMP_ID  and t1.SALARY_DT = t2.SALARY_DT ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD           ");
            sb.AppendLine("    left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='DATA_SRC' and  t1.DATA_SRC = e.SUB_CD       ");
            sb.AppendLine("    left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = f.SUB_CD ");
            sb.AppendLine("    left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                               ");
            sb.AppendLine("    left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                               ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM item on t1.SALARY_ID = item.SALARY_ID                                       ");
            sb.AppendLine("    left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                               ");
            //sb.AppendLine("   where CONVERT(varchar(100), t1.SALARY_DT , 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.SALARY_ID + t1.PAY_KIND = @dtldatakey  ");
            sb.AppendLine(@"   where  1=1  
                                  and t1.SALARY_DT = substring(@dtldatakey,1,10)
                                   and t1.SALARY_TYPE = substring(@dtldatakey,11,1) 
                                   and t1.EMP_ID = substring(@dtldatakey,12,5)   
                                   and t1.SALARY_ID =  substring(@dtldatakey,17,4)   
                                   and t1.PAY_KIND = substring(@dtldatakey,21,4)  ");
            ht.Add("@dtldatakey", dtldatakey);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getModInitialData_PROCESS_STATUS_isY_notA(string dtldatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_DT,t1.SALARY_TYPE  +'-'+ f.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,v.JOIN_DT ,v.LEAVE_DT,t1.IS_PLUS,t1.IS_TAX         ");
            sb.AppendLine("      ,t1.DATA_SRC,t1.AMOUNT as CHG_AMT_B, t1.AMOUNT as CHG_AMT_A                                            ");
            sb.AppendLine("      ,t1.CREATED_BY,t1.CREATED_DT                                                                           ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+t1.SALARY_NAME as SALARY_NAME                                                       ");
            sb.AppendLine("      ,t2.COMPANY_CD+'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                    ");
            sb.AppendLine("      ,t2.EMP_CD +'-'+ d.SUB_DESC as EMP_CD_DESC                                                             ");
            sb.AppendLine("      ,t1.DATA_SRC+'-'+e.SUB_DESC as DATA_SRC_DESC                                                           ");
            sb.AppendLine("      , 'N-未生效' as PROCESS_STATUS                                                                         ");
            sb.AppendLine("      , 'U-修改'  as CHG_STATUS                                                                              ");
            sb.AppendLine("      , 'N' as PROCESS_STATUS                                                                                ");
            sb.AppendLine("      , 'U'  as CHG_STATUS                                                                                   ");
            sb.AppendLine("      , '' as APPROVE_DT, '' as APPROVE_BY, '' as REMARK, '' as APP_REMARK                                   ");
            sb.AppendLine("      , t1.PAY_KIND,t1.PAY_KIND  +'-'+p.SALARY_NAME as PAY_KIND_DESC, t1.DATA_YM                                 ");
            sb.AppendLine("      , t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME                                                         ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+ item.SALARY_NAME as SALARY_NAME,t1.SALARY_ID ,'0' as SEQ_NO                        ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY t1                                                                                 ");
            sb.AppendLine("   left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT and t1.EMP_ID = t2.EMP_ID and t1.SALARY_TYPE = t2.SALARY_TYPE and t1.PAY_KIND = t2.PAY_KIND ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD            ");
            sb.AppendLine("   left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='DATA_SRC' and  t1.DATA_SRC = e.SUB_CD        ");
            sb.AppendLine("    left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = f.SUB_CD ");
            sb.AppendLine("   left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                ");
            sb.AppendLine("   left join VW_H_EMP_DATA v on v.EMP_ID = t1.EMP_ID                                                         ");
            sb.AppendLine("   left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                                ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM item on t1.SALARY_ID = item.SALARY_ID                                        ");
            sb.AppendLine("   left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                                ");
            //sb.AppendLine("   where CONVERT(varchar(100), t1.SALARY_DT , 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.SALARY_ID + t1.PAY_KIND = @dtldatakey  ");
            sb.AppendLine(@"   where  1=1  
                                  and t1.SALARY_DT = substring(@dtldatakey,1,10)
                                   and t1.SALARY_TYPE = substring(@dtldatakey,11,1) 
                                   and t1.EMP_ID = substring(@dtldatakey,12,5)   
                                   and t1.SALARY_ID =  substring(@dtldatakey,17,4)   
                                   and t1.PAY_KIND = substring(@dtldatakey,21,4)  ");
            ht.Add("@dtldatakey", dtldatakey);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getModInitialData_PROCESS_STATUS_isNotY_A(string dtldatakey, string seq_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select distinct t1.SALARY_DT,t1.SALARY_TYPE  +'-'+ h.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,v.JOIN_DT ,v.LEAVE_DT,s.IS_PLUS,s.IS_TAX                                ");
            sb.AppendLine("       ,t1.PROCESS_STATUS,t1.CHG_STATUS,t1.CHG_AMT_B,t1.CHG_AMT_A                                                       ");
            sb.AppendLine("       ,t1.CREATED_BY,t1.CREATED_DT                                                                                                 ");
            sb.AppendLine("       ,t1.SALARY_ID +'-'+s.SALARY_NAME as SALARY_NAME                                                                             ");
            sb.AppendLine("       ,t2.COMPANY_CD+'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                                          ");
            sb.AppendLine("       ,t2.EMP_CD +'-'+ d.SUB_DESC as EMP_CD_DESC                                                                                   ");
            sb.AppendLine("       ,'4'+'-'+e.SUB_DESC as DATA_SRC_DESC                                                                                 ");
            sb.AppendLine("       ,t1.PROCESS_STATUS +'-'+ f.SUB_DESC as PROCESS_STATUS_DESC                                                                   ");
            sb.AppendLine("       ,t1.CHG_STATUS +'-'+ g.SUB_DESC as CHG_STATUS_DESC                                                                           ");
            sb.AppendLine("       ,t1.APPROVE_DT,t1.APPROVE_BY, t1.REMARK,t1.APP_REMARK                                                                        ");
            sb.AppendLine("       ,t1.PAY_KIND,t1.PAY_KIND  +'-'+ p.SALARY_NAME as PAY_KIND_DESC, t1.DATA_YM                                                        ");
            sb.AppendLine("      , t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME                                                                                ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t1.SALARY_ID ,t1.SEQ_NO                                                   ");
            sb.AppendLine("    from TB_S_S_SALARY_PAY_TMP t1                                                                                              ");
            sb.AppendLine("    left join TB_S_M_EMP_RESULT t2 on t1.DATA_YM = t2.SALARY_YM and t1.EMP_ID = t2.EMP_ID  and t1.SALARY_DT = t2.SALARY_DT     ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID = s.SALARY_ID                                                                     ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                                   ");
            sb.AppendLine("    left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='DATA_SRC' and  e.SUB_CD = '4'                            ");//t1.DATA_SRC   
            sb.AppendLine("    left join TB_9_M_COMM_D g on  g.SYS_CD ='SA' and  g.MAIN_CD='CHG_STATUS' and  t1.CHG_STATUS = g.SUB_CD                           ");
            sb.AppendLine("    left join TB_9_M_COMM_D f on  f.SYS_CD ='SA' and  f.MAIN_CD='PROCESS_STATUS' and  t1.PROCESS_STATUS = f.SUB_CD                   ");
            sb.AppendLine("    left join TB_9_M_COMM_D h on  h.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = h.SUB_CD ");
            sb.AppendLine("    left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                                       ");
            sb.AppendLine("   left join VW_H_EMP_DATA v on v.EMP_ID = t1.EMP_ID                                                                                ");
            sb.AppendLine("    left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                                                       ");
            sb.AppendLine("    left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                               ");
            //sb.AppendLine("   where CONVERT(varchar(100), t1.SALARY_DT , 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.SALARY_ID + t1.PAY_KIND = @dtldatakey           ");
            sb.AppendLine("    where 1=1 ");
            sb.AppendLine(@"    and t1.SALARY_DT = substring(@dtldatakey,1,10)
                                and t1.SALARY_TYPE = substring(@dtldatakey,11,1) 
                                and t1.EMP_ID = substring(@dtldatakey,12,5)   
                                and t1.SALARY_ID =  substring(@dtldatakey,17,4)   
                                and t1.PAY_KIND = substring(@dtldatakey,21,4)  ");
            sb.AppendLine("     and t1.SEQ_NO = @SEQ_NO ");
            ht.Add("@dtldatakey", dtldatakey);
            ht.Add("@SEQ_NO", seq_no);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getModInitialData_PROCESS_STATUS_isNotY_notA(string dtldatakey, string seq_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select distinct t1.SALARY_DT,t1.SALARY_TYPE  +'-'+ h.SUB_DESC as SALARY_TYPE_DESC,t1.EMP_ID,t2.EMP_NAME,v.JOIN_DT ,v.LEAVE_DT,s.IS_PLUS,s.IS_TAX ");
            sb.AppendLine("       ,t1.PROCESS_STATUS,t1.CHG_STATUS,t1.CHG_AMT_B,t1.CHG_AMT_A                                                       ");
            sb.AppendLine("       ,t1.CREATED_BY,t1.CREATED_DT                                                                                                 ");
            sb.AppendLine("       ,t1.SALARY_ID +'-'+s.SALARY_NAME as SALARY_NAME                                                                             ");
            sb.AppendLine("       ,t2.COMPANY_CD+'-'+c.COMPANY_SNAME as COMPANY_SNAME                                                                          ");
            sb.AppendLine("       ,t2.EMP_CD +'-'+ d.SUB_DESC as EMP_CD_DESC                                                                                   ");
            sb.AppendLine("       ,'4'+'-'+e.SUB_DESC as DATA_SRC_DESC                                                                                 ");
            sb.AppendLine("       ,t1.PROCESS_STATUS +'-'+ f.SUB_DESC as PROCESS_STATUS_DESC                                                                   ");
            sb.AppendLine("       ,t1.CHG_STATUS +'-'+ g.SUB_DESC as CHG_STATUS_DESC                                                                           ");
            sb.AppendLine("       ,t1.APPROVE_DT,t1.APPROVE_BY, t1.REMARK,t1.APP_REMARK                                                                        ");
            sb.AppendLine("       ,t1.PAY_KIND,t1.PAY_KIND  +'-'+ p.SALARY_NAME as PAY_KIND_DESC, t1.DATA_YM                                                        ");
            sb.AppendLine("      , t2.DEPT_NO +'-'+ dept.DEPT_NAME as DEPT_NAME                                                                                ");
            sb.AppendLine("      ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t1.SALARY_ID ,t1.SEQ_NO                                                   ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP t1                                                                                              ");
            sb.AppendLine("   left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT and t1.EMP_ID = t2.EMP_ID and t1.SALARY_TYPE = t2.SALARY_TYPE and t1.PAY_KIND = t2.PAY_KIND ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s on t1.SALARY_ID = s.SALARY_ID                                                                     ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t2.EMP_CD = d.SUB_CD                                   ");
            sb.AppendLine("   left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='DATA_SRC' and   e.SUB_CD = '4'                              ");//t1.DATA_SRC =
            sb.AppendLine("   left join TB_9_M_COMM_D g on  g.SYS_CD ='SA' and  g.MAIN_CD='CHG_STATUS' and  t1.CHG_STATUS = g.SUB_CD                           ");
            sb.AppendLine("   left join TB_9_M_COMM_D f on  f.SYS_CD ='SA' and  f.MAIN_CD='PROCESS_STATUS' and  t1.PROCESS_STATUS = f.SUB_CD                   ");
            sb.AppendLine("    left join TB_9_M_COMM_D h on  h.SYS_CD ='SC' and  f.MAIN_CD='SALARY_TYPE' and  t1.SALARY_TYPE = h.SUB_CD ");
            sb.AppendLine("   left join TB_H_M_COMPANY c on t2.COMPANY_CD = c.COMPANY_CD                                                                       ");
            sb.AppendLine("   left join VW_H_EMP_DATA v on v.EMP_ID = t1.EMP_ID                                                                                ");
            sb.AppendLine("   left join VW_H_DEPT_DATA dept on t2.DEPT_NO = dept.DEPT_NO                                                                       ");
            sb.AppendLine("    left join VW_SALARYAND9999 p on  t1.PAY_KIND = p.SALARY_ID                                               ");
            //sb.AppendLine("   where CONVERT(varchar(100), t1.SALARY_DT , 111) + t1.SALARY_TYPE + t1.EMP_ID + t1.SALARY_ID + t1.PAY_KIND = @dtldatakey  ");
            sb.AppendLine("    where 1=1 ");
            sb.AppendLine(@"    and t1.SALARY_DT = substring(@dtldatakey,1,10)
                                and t1.SALARY_TYPE = substring(@dtldatakey,11,1) 
                                and t1.EMP_ID = substring(@dtldatakey,12,5)   
                                and t1.SALARY_ID =  substring(@dtldatakey,17,4)   
                                and t1.PAY_KIND = substring(@dtldatakey,21,4)  ");
            sb.AppendLine("     and t1.SEQ_NO = @SEQ_NO ");
            ht.Add("@dtldatakey", dtldatakey);
            ht.Add("@SEQ_NO", seq_no);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public void modDtlData_add()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_S_S_SALARY_PAY_TMP (SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SEQ_NO, CHG_AMT_B, CHG_AMT_A ");
            sb.AppendLine(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_DT, APPROVE_BY, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");//APP_REMARK,
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @DATA_YM, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE, @SEQ_NO, @CHG_AMT_B, @CHG_AMT_A ");
            sb.AppendLine(" ,@CHG_STATUS, @PROCESS_STATUS, @APPROVE_DT, @APPROVE_BY ,@REMARK, @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");//@APP_REMARK,

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", Convert.ToInt32(SEQ_NO) + 1);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", "U");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            //ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@PAY_TYPE", "Y");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void modDtlData_update()
    {
        try
        {
            // 更新 薪資明細計算暫存檔(TB_S_S_SALARY_PAY_TMP)
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine(" set CHG_AMT_B = @CHG_AMT_B, CHG_AMT_A = @CHG_AMT_A, CHG_STATUS = @CHG_STATUS,PROCESS_STATUS = @PROCESS_STATUS ,APPROVE_DT = @APPROVE_DT ");
            sb.AppendLine("     ,APPROVE_BY = @APPROVE_BY, REMARK = @REMARK, UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");//APP_REMARK = @APP_REMARK, 
            sb.AppendLine(" where SALARY_DT = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and DATA_YM = @DATA_YM ");
            sb.AppendLine("   and EMP_ID = @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@REMARK", REMARK);
            //ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC230");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    public DataTable checkStatus(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_STATUS from VW_H_EMP_DATA where emp_id = @emp_id ");

            ht.Add("@emp_id", emp_id);
           
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}