using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA3210DAO 的摘要描述
/// </summary>
public class CFB2IA3210DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string SALARY_SYM { get; set; }
    public string SALARY_EYM { get; set; }
    public string COMPANY_CD { get; set; }
    public string EFFECT_DT { get; set; }
    public string AVG_SALARY { get; set; }
    public string A_OLD_INSAMT { get; set; }
    public string A_NEW_INSAMT { get; set; }
    public string B_OLD_INSAMT { get; set; }
    public string B_NEW_INSAMT { get; set; }
    public string C_OLD_INSAMT { get; set; }
    public string C_NEW_INSAMT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string BILLS_KIND { get; set; }
    public string FEES_YM { get; set; }
    public string TRACE_OR_CHANGE { get; set; }
    public string YNB { get; set; }
    public string IDENTITY_KIND { get; set; }
    public string LICENSE_ID { get; set; }
    public string COMP_PORCESS_YN { get; set; }
    public string APPROVE_BY { get; set; }
    public string DATA_YM { get; set; }
    public string SEQ_NO { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_YM { get; set; }
    public string INS_TYPE { get; set; }
    public string TRACE_KIND { get; set; }
    public string TEMP_ID { get; set; }
    public string TLICENSE_ID { get; set; } 
    
    public CFB2IA3210DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind, string comp_porcess_yn)
    {
        try
        {
            //if (sortExpression.Contains("COMPANY_CD"))
            //    sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("EMP_CD_NAME"))
                sortExpression = sortExpression.Replace("EMP_CD_NAME", "e.sub_desc");
            if (sortExpression.Contains("EMP_CHG_CD_NAME"))
                sortExpression = sortExpression.Replace("EMP_CHG_CD_NAME", "d.sub_desc");
            if (sortExpression.Contains("IDENTITY_KIND_NAME"))
                sortExpression = sortExpression.Replace("IDENTITY_KIND_NAME", "f.sub_desc");
            if (sortExpression.Contains("FAMILY_NAME"))
                sortExpression = sortExpression.Replace("FAMILY_NAME", "a.FAMILY_NAME");
            if (sortExpression.Contains("BILLS_INS_AMT"))
                sortExpression = sortExpression.Replace("BILLS_INS_AMT", "a.BILLS_INS_AMT");
            if (sortExpression.Contains("CHANG_TYPE"))
                sortExpression = sortExpression.Replace("CHANG_TYPE", "a.CHANG_TYPE");
            if (sortExpression.Contains("FEES_REMARK"))
                sortExpression = sortExpression.Replace("FEES_REMARK", "a.FEES_REMARK");
            if (sortExpression.Contains("FEES_SELF"))
                sortExpression = sortExpression.Replace("FEES_SELF", "a.FEES_SELF");
            if (sortExpression.Contains("FEES_CMP"))
                sortExpression = sortExpression.Replace("FEES_CMP", "a.FEES_CMP");
            if (sortExpression.Contains("FEES"))
                sortExpression = sortExpression.Replace("FEES", "a.FEES");
            if (sortExpression.Contains("TRACED_FEES_SELF"))
                sortExpression = sortExpression.Replace("TRACED_FEES_SELF", "a.TRACED_FEES_SELF");
            if (sortExpression.Contains("TRACED_FEES_CMP"))
                sortExpression = sortExpression.Replace("TRACED_FEES_CMP", "a.TRACED_FEES_CMP");
            if (sortExpression.Contains("TRACED_FEES"))
                sortExpression = sortExpression.Replace("TRACED_FEES", "a.TRACED_FEES");
            if (sortExpression.Contains("BILLS_TOT"))
                sortExpression = sortExpression.Replace("BILLS_TOT", "a.TRACED_FEES+a.FEES");
            if (sortExpression.Contains("INS_FEES"))
                sortExpression = sortExpression.Replace("INS_FEES", "a.INS_FEES");
            if (sortExpression.Contains("DIFF_AMT"))
                sortExpression = sortExpression.Replace("DIFF_AMT", "a.BILLS_FEES-a.INS_FEES");
            if (sortExpression.Contains("PROCESS_MEMO"))
                sortExpression = sortExpression.Replace("PROCESS_MEMO", "a.PROCESS_MEMO");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from ");
            sb.AppendLine(" (select  row_number() over( order by " + sortExpression + ") as RowNumber,a.LICENSE_ID,a.EMP_ID,a.EMP_NAME,d.sub_desc as EMP_CHG_CD_NAME ,e.sub_desc as EMP_CD_NAME,a.IDENTITY_KIND, ");
            sb.AppendLine("         a.BILLS_INS_AMT,a.FEES_CMP,a.TRACED_FEES_CMP,a.FEES_CMP+a.TRACED_FEES_CMP as BILLS_CMP_TOTAL,a.INS_CMP,a.FEES_CMP+a.TRACED_FEES_CMP-a.INS_CMP as DIFF_AMT ,a.PROCESS_MEMO_CMP ");
            
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" left join TB_H_M_COMPANY b on a.COMPANY_CD= b.COMPANY_CD");
            sb.AppendLine(" left join VW_H_EMP_DATA c on a.emp_id= c.emp_id");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.main_cd='EMP_CHG_CD' and d.sub_cd= c.EMP_CHG_CD /*在職區分*/ ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.main_cd='EMP_CD' and e.sub_cd= c.EMP_CD /*員工區分*/	");
            sb.AppendLine(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.main_cd='IDENTITY_KIND' and f.sub_cd= a.IDENTITY_KIND /*身分別*/");

            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' and COMP_PORCESS_YN = @COMP_PORCESS_YN and (a.FEES_CMP+a.TRACED_FEES_CMP-a.INS_CMP) <> 0");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }
            
            
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@COMP_PORCESS_YN", comp_porcess_yn);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind, string comp_porcess_yn)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(a.EMP_ID) total_record ");
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' and COMP_PORCESS_YN = @COMP_PORCESS_YN  and (a.FEES_CMP+a.TRACED_FEES_CMP-a.INS_CMP) <> 0");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }

            ht.Add("@COMP_PORCESS_YN", comp_porcess_yn);

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

    public void deleteBILLS_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_I_M_BILLS_COMPARE");
            sb.Append(" where BILLS_KIND = @BILLS_KIND and FEES_YM = @FEES_YM and COMPANY_CD = @COMPANY_CD and EMP_ID = @EMP_ID and LICENSE_ID = @LICENSE_ID");

            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getBoss()
    {
        string st = "";
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select DIRECT_HEAD_EMP_ID From TB_H_M_EMP");
        sb.AppendLine(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", SessionHandle.Current.emp_id);

        DataTable dt = dbConn.Query(sb, ht);
        if (dt.Rows.Count > 0)
        {
            st = dt.Rows[0]["DIRECT_HEAD_EMP_ID"].ToString();
        }

        return st;

    }

    public void insertTB_I_M_FEES_TRACEBACK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_I_M_FEES_TRACEBACK (SALARY_YM,EMP_ID,INS_TYPE,IDENTITY_KIND,LICENSE_ID
                                                          ,TRACE_KIND,TRACE_TYPE,TRACE_AMT,REMARK,APPROVE_DT
                                                          ,APPROVE_BY,APPROVE_STATUS,APP_REMARK,IS_YN,SALARY_DT
                                                          ,SALARY_YM1,OP_DT,CREATED_BY,CREATED_DT,UPDATED_BY
                                                          ,UPDATED_DT,FUNC_ID)
                         select @FEES_YM,EMP_ID,case when @BILLS_KIND = 'A' then 'B' 
                                                      when @BILLS_KIND = 'B' then 'A' else 'C' end , IDENTITY_KIND, LICENSE_ID,
                                'B',case when FEES_CMP+TRACED_FEES_CMP-INS_CMP > 0 then 'A' else 'B' end, ABS(FEES_CMP+TRACED_FEES_CMP-INS_CMP) ,'保險雇主追溯' , getdate(),
                                @APPROVE_BY,'Y','','N',@SALARY_DT,
                                '',@OP_DT,@CREATED_BY,getdate(),@UPDATED_BY,                       
                                getdate(),@FUNC_ID
                         from TB_I_M_BILLS_COMPARE
                         where BILLS_KIND = @BILLS_KIND
                         and   FEES_YM = @FEES_YM
                         and   COMPANY_CD = @COMPANY_CD   
                         and   COMP_PORCESS_YN = @COMP_PORCESS_YN                     
                         and   (FEES_CMP+TRACED_FEES_CMP-INS_CMP) <> 0 ");

            if (EMP_ID != "")
            {
                sb.Append(" and   EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (LICENSE_ID != "")
            {
                sb.Append(" and   LICENSE_ID like @LICENSE_ID");
                ht.Add("@LICENSE_ID", LICENSE_ID + '%');
            }
            

            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@SALARY_DT", DBNull.Value);
            ht.Add("@OP_DT", DBNull.Value);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);            
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@COMP_PORCESS_YN", COMP_PORCESS_YN);
            
           

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void insertTB_S_M_SUBSIDY_DEDUCTIONS_1()
    {
        try
        {
            string INS_TYPE = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,
                                                                 AMOUNT,IS_PLUS,IS_TAX,REMARK,SALARY_STATUS,
                                                                 SALARY_PROC_DT,SALARY_DT,CREATED_BY,CREATED_DT,UPDATED_BY,
                                                                 UPDATED_DT,FUNC_ID)
                         select x.DATA_YM,x.EMP_ID,x.EMP_NAME,x.SALARY_ID,
                        (select isnull(max(SEQ_NO),0) from TB_S_M_SUBSIDY_DEDUCTIONS_1 where DATA_YM = x.DATA_YM and SALARY_ID=x.SALARY_ID and EMP_ID = x.EMP_ID)+WKROW as SEQ_NO,
                        x.TRACE_AMT,c.IS_PLUS, c.IS_TAX ,'' , 'N' 
                        ,@SALARY_DT,@SALARY_DT,@CREATED_BY,getdate(),@UPDATED_BY, 
                        getdate(),@FUNC_ID 
                        from (
                        select distinct (select convert(char(6),dateadd(month,1,dbo.FN_S_SALARY_YM()+'01'),112)) as DATA_YM,a.EMP_ID , b.EMP_NAME, 
                                                        case when a.TRACE_TYPE = 'A' and a.INS_TYPE = 'A' then '3103'
									                         when a.TRACE_TYPE = 'A' and a.INS_TYPE = 'B' then '3104'
									                         when a.TRACE_TYPE = 'A' and a.INS_TYPE = 'C' then '3101'
									                         when a.TRACE_TYPE = 'B' and a.INS_TYPE = 'A' then '2103'
									                         when a.TRACE_TYPE = 'B' and a.INS_TYPE = 'B' then '2104'
									                         when a.TRACE_TYPE = 'B' and a.INS_TYPE = 'C' then '2101' end as SALARY_ID
                                                 ,TRACE_AMT	, ROW_NUMBER() OVER (PARTITION BY a.EMP_ID  ORDER BY a.EMP_ID) WKROW 			
                                                 from TB_I_M_FEES_TRACEBACK a
                                                 left join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID
						                         where a.SALARY_YM = @SALARY_YM and a.INS_TYPE = @INS_TYPE and a.TRACE_KIND = 'B' ");
            if (EMP_ID != "")
            {
                sb.Append(" and   a.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (LICENSE_ID != "")
            {
                sb.Append(" and  a.LICENSE_ID like @LICENSE_ID");
                ht.Add("@LICENSE_ID", LICENSE_ID + '%');
            }
            sb.Append(@" )x
                        left join TB_S_M_SALARY_ITEM c on x.SALARY_ID = c.SALARY_ID");

            if (BILLS_KIND == "A")
            {
                INS_TYPE = "B";
            }
            else if (BILLS_KIND == "B")
            {
                INS_TYPE = "A";
            }else if (BILLS_KIND == "D")
            {
                INS_TYPE = "C";
            }

            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@SALARY_YM", FEES_YM);
            ht.Add("@SALARY_DT", DBNull.Value);            
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateBILLS_COMPARE(string flag)
    {
        try
        {
            string YN = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update TB_I_M_BILLS_COMPARE 
                         set COMP_PORCESS_YN = @YN,UPDATED_BY = @UPDATED_BY ,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID
                         where BILLS_KIND  =@BILLS_KIND and FEES_YM = @FEES_YM and COMPANY_CD = @COMPANY_CD and COMP_PORCESS_YN = @COMP_PORCESS_YN and (FEES_CMP+TRACED_FEES_CMP-INS_CMP) <> 0");

            if (EMP_ID != "")
            {
                sb.Append(" and   EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (LICENSE_ID != "")
            {
                sb.Append(" and LICENSE_ID like @LICENSE_ID");
                ht.Add("@LICENSE_ID", LICENSE_ID + '%');
            }

            if (flag == "OK")
            {
                YN = "Y";
            }
            else if (flag == "NOT")
            {
                YN = "N";
            }

            ht.Add("@YN", YN);            
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@BILLS_KIND", BILLS_KIND);            
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@COMP_PORCESS_YN", COMP_PORCESS_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            
            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {            
            throw;
        }
        
    }

    public DataTable checkToSalary()
    {
        
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(@" select x.EMP_ID,c.SALARY_STATUS,x.FEES_YM,x.SALARY_ID,c.DATA_YM,c.SEQ_NO from (
                         Select a.*,b.INS_TYPE,b.TRACE_TYPE,
                                case when b.TRACE_TYPE = 'A' and b.INS_TYPE = 'A' then '3103'
									 when b.TRACE_TYPE = 'A' and b.INS_TYPE = 'B' then '3104'
									 when b.TRACE_TYPE = 'A' and b.INS_TYPE = 'C' then '3101'
									 when b.TRACE_TYPE = 'B' and b.INS_TYPE = 'A' then '2103'
									 when b.TRACE_TYPE = 'B' and b.INS_TYPE = 'B' then '2104'
									 when b.TRACE_TYPE = 'B' and b.INS_TYPE = 'C' then '2101' end as SALARY_ID 
                         From TB_I_M_BILLS_COMPARE a 
                         left join TB_I_M_FEES_TRACEBACK b on b.SALARY_YM = a.FEES_YM and b.EMP_ID = a.EMP_ID and a.IDENTITY_KIND = b.IDENTITY_KIND 
                                   and a.LICENSE_ID = b.LICENSE_ID and b.TRACE_KIND = 'B'
                         where a.BILLS_KIND  =@BILLS_KIND and a.FEES_YM = @FEES_YM and a.COMPANY_CD = @COMPANY_CD 
                         and a.COMP_PORCESS_YN = @COMP_PORCESS_YN and (a.FEES_CMP+a.TRACED_FEES_CMP-a.INS_CMP) <> 0 ");
        if (EMP_ID != "")
        {
            sb.Append(" and   a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        if (LICENSE_ID != "")
        {
            sb.Append(" and  a.LICENSE_ID like @LICENSE_ID");
            ht.Add("@LICENSE_ID", LICENSE_ID + '%');
        }

        sb.AppendLine(@") x 
                        left join TB_S_M_SUBSIDY_DEDUCTIONS_1 c on c.EMP_ID = x.EMP_ID and c.SALARY_ID = x.SALARY_ID
                        where c.DATA_YM = (select distinct (select convert(char(6),dateadd(month,1,dbo.FN_S_SALARY_YM()+'01'),112)) ) ");
        

        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);
        ht.Add("@COMP_PORCESS_YN", COMP_PORCESS_YN);

        return dbConn.Query(sb, ht);

    }

    public void delTB_S_M_SUBSIDY_DEDUCTIONS_1()
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_S_M_SUBSIDY_DEDUCTIONS_1 
                         where DATA_YM = @DATA_YM and EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and SEQ_NO = @SEQ_NO ");

            ht.Add("@EMP_ID", TEMP_ID);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@SEQ_NO", SEQ_NO);
            
            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable selectTB_I_M_FEES_TRACEBACK()
    {

        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(@" Select b.SALARY_YM,b.EMP_ID,b.INS_TYPE,b.IDENTITY_KIND,b.LICENSE_ID,b.TRACE_KIND
                         From TB_I_M_BILLS_COMPARE a 
                         left join TB_I_M_FEES_TRACEBACK b on b.SALARY_YM = a.FEES_YM and b.EMP_ID = a.EMP_ID and a.IDENTITY_KIND = b.IDENTITY_KIND 
                                   and a.LICENSE_ID = b.LICENSE_ID and b.TRACE_KIND = 'B'
                         where a.BILLS_KIND  =@BILLS_KIND and a.FEES_YM = @FEES_YM and a.COMPANY_CD = @COMPANY_CD 
                         and a.COMP_PORCESS_YN = @COMP_PORCESS_YN and (a.FEES_CMP+a.TRACED_FEES_CMP-a.INS_CMP) <> 0 ");
        if (EMP_ID != "")
        {
            sb.Append(" and   a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        if (LICENSE_ID != "")
        {
            sb.Append(" and  a.LICENSE_ID like @LICENSE_ID");
            ht.Add("@LICENSE_ID", LICENSE_ID + '%');
        }        


        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);
        ht.Add("@COMP_PORCESS_YN", COMP_PORCESS_YN);

        return dbConn.Query(sb, ht);

    }

    public void delTB_I_M_FEES_TRACEBACK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_I_M_FEES_TRACEBACK 
                         where SALARY_YM = @SALARY_YM and EMP_ID = @EMP_ID and INS_TYPE = @INS_TYPE
                         and IDENTITY_KIND = @IDENTITY_KIND and LICENSE_ID = @LICENSE_ID and TRACE_KIND = @TRACE_KIND  ");

            ht.Add("@EMP_ID", TEMP_ID);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", TLICENSE_ID);
            ht.Add("@TRACE_KIND", TRACE_KIND);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void Check_FeeA(string def_ym,string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_A");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeB(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_B");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeC(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_C");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeD(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_D");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable countRow(string BILLS_KIND, string FEES_YM, string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select count(*) row From TB_I_M_BILLS_COMPARE");
        sb.AppendLine(" where BILLS_KIND = @BILLS_KIND and COMPANY_CD=@COMPANY_CD and FEES_YM = @FEES_YM");

        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);

        return dbConn.QueryT(sb, ht);

    }
    public DataTable checkStatus(string BILLS_KIND, string FEES_YM, string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select TRACED_YN,CHANG_LEVEL_YN  From TB_I_M_BILLS_COMPARE");
        sb.AppendLine(" where BILLS_KIND = @BILLS_KIND and COMPANY_CD=@COMPANY_CD and FEES_YM = @FEES_YM");
        sb.AppendLine(" group by TRACED_YN,CHANG_LEVEL_YN");

        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);

        return dbConn.Query(sb, ht);

    }
    public void callEMP(string yyyymmdd)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_EMP_INCOMPANY");
            ht.Add("@pDesc", "FB2IA320");
            String aa = Convert.ToDateTime(yyyymmdd).AddMonths(1).AddDays(-1).ToShortDateString();
            ht.Add("@pDate",  Convert.ToDateTime(yyyymmdd).AddMonths(1).AddDays(-1).ToShortDateString());
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2IA320");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable company(string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select COMPANY_CD,COMPANY_SNAME From TB_H_M_COMPANY");
        sb.AppendLine(" where COMPANY_CD=@COMPANY_CD");
        ht.Add("@COMPANY_CD", COMPANY_CD);
        return dbConn.Query(sb, ht);

    }
    public DataTable getExcelData(string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from ");
            sb.AppendLine(" (select  row_number() over( order by a.emp_id) as RowNumber,b.COMPANY_SNAME,a.LICENSE_ID,a.EMP_ID,a.EMP_NAME,d.sub_desc as EMP_CHG_CD_NAME ,e.sub_desc as EMP_CD_NAME ");
            sb.AppendLine("        ,f.SUB_DESC as IDENTITY_KIND_NAME,a.FAMILY_NAME,a.BILLS_INS_AMT,a.CHANG_TYPE,a.FEES_REMARK,a.FEES_SELF,a.FEES_CMP");
            sb.AppendLine("        ,a.FEES,a.TRACED_FEES_SELF,a.TRACED_FEES_CMP,a.TRACED_FEES,(a.TRACED_FEES_SELF+a.FEES_SELF) as BILLS_TOT,a.INS_FEES,(a.FEES_SELF+a.TRACED_FEES_SELF-a.INS_FEES) as DIFF_AMT");
            sb.AppendLine("        ,a.TRACED_MEMO,a.TRACED_YMS,a.COMPFEES_YM ");
            sb.AppendLine("        ,a.PROCESS_MEMO,a.LAST_UPDATE_DT,a.BILLS_FEES,a.RATE,(a.BILLS_FEES-a.INS_FEES) as DIFF_AMT1	");
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" left join TB_H_M_COMPANY b on a.COMPANY_CD= b.COMPANY_CD");
            sb.AppendLine(" left join VW_H_EMP_DATA c on a.emp_id= c.emp_id");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.main_cd='EMP_CHG_CD' and d.sub_cd= c.EMP_CHG_CD /*在職區分*/ ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.main_cd='EMP_CD' and e.sub_cd= c.EMP_CD /*員工區分*/	");
            sb.AppendLine(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.main_cd='IDENTITY_KIND' and f.sub_cd= a.IDENTITY_KIND /*身分別*/");
            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' ");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }

            sb.AppendLine(" ) z");
            return dbConn.Query(sb, ht);
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

    public void update_BILLS_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_I_M_BILLS_COMPARE");
            //週一改 還要區分追溯處理否 或 異動投保等級否
            if (TRACE_OR_CHANGE == "1") //追溯處理
            {
                sb.AppendLine(" set TRACED_YN = @YNB ");
            }
            if (TRACE_OR_CHANGE == "2") //異動投保等級
            {
                sb.AppendLine(" set CHANG_LEVEL_YN = @YNB ");
            }
            sb.AppendLine(" , UPDATED_BY = @UPDATED_BY , UPDATED_DT = getdate() , FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where BILLS_KIND =@BILLS_KIND  and FEES_YM= @FEES_YM and COMPANY_CD = @COMPANY_CD and EMP_ID = @EMP_ID");
            sb.AppendLine(" and IDENTITY_KIND =@IDENTITY_KIND  and LICENSE_ID= @LICENSE_ID");

            ht.Add("@YNB", YNB);
            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);            
            ht.Add("@LICENSE_ID", LICENSE_ID);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.QueryT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}