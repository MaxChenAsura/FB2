using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SE2100DAO 的摘要描述
/// </summary>
public class CFB2SE2100DAO : BaseDAO
{
    public string DATA_YEAR { get; set; }
	public CFB2SE2100DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string effect_ym)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY "+ sortExpression + ") As RowNumber,a.EFFECT_YM");
            sb.Append(" ,a.NOADJ_CNT,d.sub_desc as APPROVES_STATUS_NAME,e.EMP_NAME as MEM_CREATE_BY_NAME,a.MEM_CREATE_DT ");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=a.APPROVE_STATUS	");
            sb.Append(" left join TB_H_M_EMP e on e.EMP_ID = a.MEM_CREATE_BY /*不調薪對象生成人員*/	");
            sb.Append(" where 1=1 ");
            if (effect_ym.Trim() != "")
            {
                sb.Append("and a.EFFECT_YM = @effect_ym ");
                ht.Add("@effect_ym", effect_ym.Replace("/", ""));
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar )");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //檢查調薪資料是否完成或是否已執行過不調薪對象
    internal DataTable get_SALARY_ADJ_H(string pa_effect_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select APPROVE_STATUS,isnull(MEM_CREATE_BY,'') as MEM_CREATE_BY from TB_S_M_SALARY_ADJ_H");
            sb.Append(" where EFFECT_YM=@pa_effect_ym ");
            ht.Add("@pa_effect_ym", pa_effect_ym);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int GetCount(int startRowIndex, int maximumRows, string effect_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_SALARY_ADJ_H t");
            sb.Append(" where 1=1 ");

            if (effect_ym.Trim() != "")
            {
                sb.Append("and t.EFFECT_YM = @effect_ym ");
                ht.Add("@effect_ym", effect_ym.Replace("/", ""));
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
    #region 異動對象生成
    //新增 初任薪敘薪對象異動明細檔(TB_S_M_HIRING_SALARY_MEM_D)
    internal void Execute_Add_TB_S_M_HIRING_SALARY_MEM_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_HIRING_SALARY_MEM_D (DATA_YEAR,EMP_ID,EMP_NAME,EMP_CD,LEVEL_CD,GRADE_CD,WS_CD,PJOB_CD,JOIN_DT,BE_EMP_DT,DEPT_NO,DEPT_NAME_20");
            sb.AppendLine("            ,DEPT_NAME_30,DEPT_NAME_40,ABILITY_PAY_B,ABILITY_PAY_A,CHG_STATUS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("select distinct @DATA_YEAR as DATA_YEAR,vm.EMP_ID,vm.EMP_NAME,vm.EMP_CD,vm.LEVEL_CD,vm.GRADE_CD,vm.WS_CD,vm.PJOB_CD,vm.JOIN_DT,vm.BE_EMP_DT,vm.DEPT_NO,vm.DEPT_NAME");
            sb.AppendLine("       ,vm.DEPT_NAME_30,vm.DEPT_NAME_40,s1.AMOUNT as ABILITY_PAY_B");
            sb.AppendLine("       ,dbo.FN_S_ABILITY_PAY(t.WS_CD, e.EDUCATION_CD,e.GRADUATION_YEAR,t.LEVEL_CD,t.GRADE_CD,t.SEX_CD,t.ARMY_CD) as ABILITY_PAY_A,'N' as CHG_STATUS");
            sb.AppendLine("       ,@CREATED_BY as CREATED_BY,GETDATE() as CREATED_DT,@UPDATED_BY as UPDATED_BY,GETDATE() as UPDATED_DT,@FUNC_ID as FUNC_ID"); 
            sb.AppendLine("from TB_S_M_HIRING_SALARY_TMP_H h ");
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_D h2 on  h.DATA_YEAR = h2.DATA_YEAR"); 
            sb.AppendLine("left join TB_H_M_EMP t on t.EMP_CD ='1'  and t.LEVEL_CD in (select distinct LEVEL_CD from TB_S_M_HIRING_SALARY where DATA_YEAR =@DATA_YEAR)");
            sb.AppendLine("     and ((t.JOIN_DT >=h.START_DT and  t.JOIN_DT <=h.APPROVE_DT ) or (t.BE_EMP_DT >=h.START_DT and t.JOIN_DT <=h.APPROVE_DT  ) )");  
            sb.AppendLine("left join VW_H_EMP_DATA vm on t.EMP_ID = vm.EMP_ID and vm.EMP_STATUS = '01'");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and  d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD"); 
            sb.AppendLine("left join TB_S_M_SALARY_TXN s1 on t.EMP_ID = s1.EMP_ID and s1.SALARY_ID='1001' and CONVERT(varchar(8), s1.EFFECT_EDT, 112) = '99991231'");
            sb.AppendLine("left join TB_H_M_EMP_EDUCATION e on t.EMP_ID = e.EMP_ID and e.IS_SALARY_SCHOOL ='Y' ");
            sb.AppendLine("left join TB_9_M_COMM_D m on  m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD");                                      
            sb.AppendLine("where 1=1 and  h.DATA_YEAR = @DATA_YEAR  and  h.PROCESS_STATUS =  'Y'");
            sb.AppendLine("order By vm.EMP_ID ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取TB_S_M_SALARY_TX的EFFECT_SDT
    public DataTable Execute_Get_TB_S_M_SALARY_TX_EFFECT_SDT()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select t.EMP_ID,t.SALARY_ID,t.AMOUNT,CONVERT(varchar(8), t.EFFECT_SDT, 112) as EFFECT_SDT,t.EFFECT_EDT");
            sb.AppendLine("       ,(select CONVERT(varchar(8), @DATA_YEAR+p.CODE_VAL1, 112)");
            sb.AppendLine("         from TB_9_M_PARAMETER p ");
            sb.AppendLine("         where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT') as DATA_YEAR_0701");
            sb.AppendLine("from TB_S_M_SALARY_TXN t ");
            sb.AppendLine("where  t.EMP_ID in( select EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR =@DATA_YEAR )");
            sb.AppendLine("and t.SALARY_ID = '1001'");
            sb.AppendLine("and t.EFFECT_SDT <=(select CONVERT(varchar(8), @DATA_YEAR+p.CODE_VAL1, 112)"); 
            sb.AppendLine("                    from TB_9_M_PARAMETER p ");
            sb.AppendLine("                    where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT')");
            sb.AppendLine("and t.EFFECT_EDT >=(select APPROVE_DT ");
            sb.AppendLine("                    from TB_S_M_HIRING_SALARY_TMP_H");
            sb.AppendLine("                    where DATA_YEAR=@DATA_YEAR)");

            ht.Add("@DATA_YEAR", @DATA_YEAR);
            return dbConn.QueryT(sb, ht,true);
        }
        catch
        {
            throw;
        }
    }
    //更新 敘薪資料檔(TB_S_M_SALARY_TXN) 
    internal void Execute_Update_TB_S_M_SALARY_TXN(string EFFECT_SDT, string DATA_YEAR_DATE, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_SALARY_TXN ");
            sb.AppendLine(" Set EFFECT_EDT = @EFFECT_EDT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and EFFECT_SDT=@EFFECT_SDT");

            ht.Add("@EFFECT_EDT", DATA_YEAR_DATE);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", "1001");
            ht.Add("@EFFECT_SDT", EFFECT_SDT);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 敘薪資料檔(TB_S_M_SALARY_TXN) 
    internal void Execute_Add_TB_S_M_SALARY_TXN(string EFFECT_SDT, string DATA_YEAR_0701,string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_TXN(EMP_ID,SALARY_ID,AMOUNT,EFFECT_SDT,EFFECT_EDT,SEQ_NO,APPROVE_BY,APPROVE_DT,REMARK");
            sb.AppendLine("                              ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("select distinct t.EMP_ID,'1001' as SALARY_ID,t2.ABILITY_PAY_A");
            sb.AppendLine("                ,@DATA_YEAR_0701 as EFFECT_SDT ,'99991231' as EFFECT_EDT");
            sb.AppendLine("                ,isnull((select MAX(SEQ_NO)+1 from TB_S_M_SALARY_TXN where EMP_ID =@EMP_ID and SALARY_ID = '1001' and EFFECT_SDT =@DATA_YEAR_0701),1) as SEQ_NO");
            sb.AppendLine("                ,@APPROVE_BY as APPROVE_BY,GETDATE() as APPROVE_DT,'初任薪敘薪對象異動' as REMARK");
            sb.AppendLine("                ,@CREATED_BY as CREATED_BY,GETDATE() as CREATED_DT,@UPDATED_BY as UPDATED_BY,GETDATE() as UPDATED_DT,@FUNC_ID as FUNC_ID");
            sb.AppendLine("from TB_S_M_SALARY_TXN t ,TB_S_M_HIRING_SALARY_MEM_D t2");
            sb.AppendLine("where t.EMP_ID =@EMP_ID and t.SALARY_ID = '1001' and t.EFFECT_SDT =@EFFECT_SDT");
            sb.AppendLine("and t2.EMP_ID =@EMP_ID and t2.DATA_YEAR=@DATA_YEAR");

            ht.Add("@DATA_YEAR_0701", DATA_YEAR_0701);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 初任薪試算主檔(TB_S_M_HIRING_SALARY_TMP_H)
    internal void Execute_Update_TB_S_M_HIRING_SALARY_TMP_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_HIRING_SALARY_TMP_H ");
            sb.AppendLine(" Set MEM_CREATE_BY = @MEM_CREATE_BY,MEM_CREATE_DT = GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@MEM_CREATE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public string Process_mark(string effect_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append("SP_S_WFB2SE210");
            //ht.Add("@pFuncID", "FB2SE210");
            //ht.Add("@pBaseYM", effect_ym);
            //ht.Add("@pUserID", SessionHandle.Current.emp_id);
            sb.Append("SP_S_FB2SE210_NOADJLIST");
            ht.Add("@qry_effect_ym", effect_ym);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion


    public DataTable GetDtlData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string effect_ym)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("PJOB_CD_NAME"))
                sortExpression = sortExpression.Replace("PJOB_CD_NAME", "a.PJOB_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,"); //a.EMP_ID
            sb.AppendLine("a.EMP_ID,b.EMP_NAME,b.JOIN_DT,(a.PJOB_CD+'-'+c.PJOB_DESC) as PJOB_CD_NAME,a.LEVEL_PAY_OLD,a.ABILITY_PAY_OLD,a.LEVEL_PAY_NEW,a.ABILITY_PAY_NEW");
            sb.AppendLine("from TB_S_M_SALARY_ADJ_D2 a");
            sb.AppendLine("left join TB_H_M_EMP b on b.EMP_ID=a.EMP_ID /*不調薪對象生成人員*/");
            sb.AppendLine("left join VW_TB_H_M_PJOB c on c.PJOB_CD=a.PJOB_CD"); 
            sb.AppendLine("where a.EFFECT_YM =@effec_ym");
            ht.Add("@effec_ym", effect_ym);

            if (emp_id.Trim() != "")
            {
                sb.AppendLine("and a.EMP_ID like '%'+@EMP_ID+'%' ");
                ht.Add("@EMP_ID", emp_id);
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
    public int GetDtlCount(int startRowIndex, int maximumRows, string emp_id, string effect_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_S_M_SALARY_ADJ_D2 t");
            sb.AppendLine("where t.EFFECT_YM =@effect_ym");
            ht.Add("@effect_ym", effect_ym);

            if (emp_id.Trim() != "")
            {
                sb.AppendLine("and t.EMP_ID like '%'+@EMP_ID+'%' ");
                ht.Add("@EMP_ID", emp_id);
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
}